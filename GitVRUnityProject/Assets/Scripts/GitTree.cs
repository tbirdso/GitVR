using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GitTree : MonoBehaviour, IGitTree
{
    public string RepoName { get; set; }
    public string Owner { get; set; }
    public string Description { get; set; }
    public List<ITreeBranch> Branches { get; set; }
    public List<ITreeNode> Nodes { get; set; }
    public ITreeNode HeadNode { get; set; }

    public void makeBranchesFromData()
    {
        // Form master branch first
        IEnumerable<ITreeBranch> queryResult = Branches.Where(b => b.BranchName.Equals("master"));
        if (queryResult.Count() < 1) throw new NullReferenceException("Could not find the master branch!");

        ITreeNode rover = queryResult.ElementAt(0).HeadNode;
        while(rover.Parent != null)
        {
            rover = rover.Parent;
        }
        queryResult.ElementAt(0).BaseNode = rover;

        //Trace each branch to its fork from the master branch
        foreach(ITreeBranch branch in Branches.Where(b => !b.BranchName.Equals("master")))
        {
            rover = branch.HeadNode;
            while(rover.Parent != null && getNodeInBranch(rover.CommitString, "master") == null)
            {
                rover = rover.Parent;
            }

            branch.BaseNode = rover;
        }

    }

    public ITreeBranch getMostRecentBranch()
    {
        ITreeBranch mostRecentBranch = Branches.ElementAt(0);
        foreach (var branch in Branches) {
            if (branch.LastModified > mostRecentBranch.LastModified)
            {
                mostRecentBranch = branch;
            }
        }

        return mostRecentBranch;
    }

    public ITreeBranch getBranch(string branchName)
    {
        IEnumerable<ITreeBranch> queryResult = Branches.Where<ITreeBranch>(branch => branch.BranchName.Equals(branchName));

        return (queryResult.Count<ITreeBranch>() > 0 ? queryResult.ElementAt(0) : null);
    }

    public ITreeNode getNthNodeInBranch(int n, string branchName)
    {
        ITreeBranch branch = getBranch(branchName);

        return branch.getNthNode(n);
    }

    public ITreeNode getNodeInBranch(string commitString, string branchName)
    {
        ITreeBranch branch = getBranch(branchName);

        ITreeNode rover = branch.HeadNode;

        while(rover != branch.BaseNode && rover != null)
        {
            if (rover.CommitString.Equals(commitString)) return rover;

            rover = rover.Parent;
        }

        return (rover != null && rover.CommitString.Equals(commitString)) ? rover : null;
    }

    public ITreeNode getNode(string commitString)
    {
        /* PROCEDURE:
         * Goto HeadNode
         * Search down master branch
         * Search up left branch
         * Search up right branch
         */
        try
        {
            return searchDownRecursive(HeadNode, commitString);
        }
        catch(Exception)
        {
            return null;
        }
    }

    public void appendChildToNode(ITreeNode child, string parentString)
    {
        ITreeNode node = getNode(parentString);

        if( node.Children.Where(c => c.CommitString.Equals(parentString)).Count() == 0)
        {
            node.Children.Add(child);
        }

    }

    public void appendParentToNode(ITreeNode parent, string childString)
    {
        ITreeNode node = getNode(childString);

        if (node.Parent == null) node.Parent = parent;
    }

    /// <summary>
    /// Build tree starting at StartPosition
    /// </summary>
    /// <param name="StartPosition"></param>
    /// <param name="NodePrefab"></param>
    /// <param name="distance"></param>
    /// <exception cref="NullReferenceException"></exception>
    public void buildTree(Vector3 StartPosition, GameObject NodePrefab, Vector3 BranchDelta)
    {
        /* PROCEDURE:
         * 1. Calculate height of tallest node in master branch
         * 2. Foreach node in master branch instantiate
         * 3. Goto HeadNode
         * 4. While not at tree base iterate and instantiate nodes
         */

        IEnumerable<ITreeBranch> queryResult = Branches.Where(branch => branch.BranchName.Equals("master"));
        if (queryResult.Count() < 0) throw new NullReferenceException("Could not find branch 'master' in the tree!");
        ITreeBranch masterBranch = queryResult.ElementAt(0);

        ITreeNode rover = masterBranch.HeadNode;
        Vector3 tallestNodePosition = new Vector3(StartPosition.x, StartPosition.y + BranchDelta.y * masterBranch.BranchLength, StartPosition.z);
        Vector3 roverPosition = tallestNodePosition;

        // Start by making the latest node
        buildUpRecursive(masterBranch.HeadNode, tallestNodePosition, NodePrefab, BranchDelta, null);

        // Recursively instantiate each layer of the master branch
        while (rover.Parent != null)
        {
            buildUpRecursive(rover.Parent, roverPosition, NodePrefab, BranchDelta, rover);

            roverPosition.y -= BranchDelta.y;
            rover = rover.Parent;
        }

        return;
    }

    private void buildUpRecursive(ITreeNode node, Vector3 targetPos, GameObject NodePrefab, Vector3 BranchDelta, ITreeNode exclude)
    {
        node.NodeObject = (GameObject)Instantiate(NodePrefab, targetPos, Quaternion.identity);

        if (node != null && node.Children != null)
        {
            foreach (ITreeNode child in node.Children.Where(child => exclude == null || !child.CommitString.Equals(exclude.CommitString)))
            {
                // FIXME: generate position of branching nodes
                buildUpRecursive(child, new Vector3(targetPos.x + BranchDelta.x, targetPos.y + BranchDelta.y, targetPos.z + BranchDelta.z),
                    NodePrefab, BranchDelta, null);
            }
        }

        return;
    }

    private ITreeNode searchDownRecursive(ITreeNode node, string target)
    {
        ITreeNode retNode = null;

        if (node.CommitString.Equals(target)) return node;

        // If the parent node is what we are looking for then return it
        if (node.Parent != null && node.Parent.CommitString.Equals(target))
            return node.Parent;

        if (node.Parent != null)
        {
            // It is most likely that we are searching for a node related to
            // the current branch so try to keep searching down the current branch
            retNode = searchDownRecursive(node.Parent, target);
        }

        if(retNode == null && node.Parent != null)
        {
            // If was not found in history of the current branch then do a depth-first upwards search
            // through other branches
            retNode = searchUpRecursive(node.Parent, target, node);
            if (retNode != null) return retNode;
        }

        return retNode;
    }

    private ITreeNode searchUpRecursive(ITreeNode node, string target, ITreeNode exclude)
    {
        ITreeNode retNode = null;

        // Search upwards through children so that we move from oldest to newest nodes depth-first
        if (node.Children != null)
        {
            foreach (ITreeNode child in node.Children)
            {
                if (exclude != null && child.CommitString.Equals(exclude.CommitString)) continue;

                if (child.CommitString.Equals(target)) return child;

                retNode = searchUpRecursive(child, target, null);
                if (retNode != null) return retNode;
            }
        }

        return null;
    }
}

public interface IGitTree
{
    string RepoName { get; set; }
    string Description { get; set; }
    string Owner { get; set; }
    List<ITreeNode> Nodes { get; set; }
    List<ITreeBranch> Branches { get; set; }
    ITreeNode HeadNode { get; set; }

    void makeBranchesFromData();

    ITreeBranch getMostRecentBranch();
    ITreeBranch getBranch(string branchName);
    ITreeNode getNthNodeInBranch(int n, string branchName);
    ITreeNode getNodeInBranch(string commitString, string branchName);
    ITreeNode getNode(string commitString);

    void appendChildToNode(ITreeNode child, string parentString);
    void appendParentToNode(ITreeNode parent, string childString);

    // TODO: redesign with necessary parameters
    void buildTree(Vector3 StartPosition, GameObject NodePrefab, Vector3 BranchDelta);

}
