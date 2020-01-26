using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GitTree : MonoBehaviour, IGitTree
{
    public string RepoName { get; set; }
    public List<ITreeBranch> Branches { get; set; }
    public ITreeNode HeadNode
    {
        get
        {
            IEnumerable<ITreeBranch> queryResult = Branches.Where(branch => branch.BranchName.Equals("master"));
            return (queryResult.Count() > 0 ? queryResult.ElementAt(0).HeadNode : null);
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

    public ITreeNode getNode(string commitString)
    {
        /* PROCEDURE:
         * Goto HeadNode
         * Search down master branch
         * Search up left branch
         * Search up right branch
         */

        return searchDownRecursive(HeadNode, commitString);
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

interface IGitTree
{
    string RepoName { get; set; }
    List<ITreeBranch> Branches { get; set; }
    ITreeNode HeadNode { get; }

    ITreeBranch getMostRecentBranch();
    ITreeBranch getBranch(string branchName);
    ITreeNode getNthNodeInBranch(int n, string branchName);
    ITreeNode getNode(string commitString);

    // TODO: redesign with necessary parameters
    void buildTree(Vector3 StartPosition, GameObject NodePrefab, Vector3 BranchDelta);

}
