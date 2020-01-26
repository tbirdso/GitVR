using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGitTree : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 BranchDistance;

    public int NUM_NODES = 30;
    List<ITreeNode> nodes = new List<ITreeNode>();
    List<ITreeBranch> branches = new List<ITreeBranch>();
    IGitTree gitTree;

    // Start is called before the first frame update
    void Start()
    {
        MakeTwoBranchTree();
        PrintTestProperties();
        CheckMethods();
        CheckBuild();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MakeTwoBranchTree()
    {
        ITreeNode lastNode = null;

        for (var i = 0; i < NUM_NODES - 2; i++)
        {
            nodes.Add(new TreeNode()
            {
                CommitString = i.ToString(),
                Parent = lastNode
            });

            lastNode = nodes[nodes.Count - 1];
        }

        lastNode = nodes[NUM_NODES - 10];
        for (var i = NUM_NODES - 2; i < NUM_NODES; i++)
        {
            nodes.Add(new TreeNode()
            {
                CommitString = i.ToString(),
                Parent = lastNode
            });

            lastNode = nodes[nodes.Count - 1];
        }

        branches.Add(new TreeBranch()
        {
            BranchName = "master",
            BaseNode = nodes[0],
            HeadNode = nodes[NUM_NODES - 3]
        });

        branches.Add(new TreeBranch()
        {
            BranchName = "working",
            BaseNode = nodes[NUM_NODES - 10],
            HeadNode = nodes[NUM_NODES - 1]
        });

        foreach (ITreeBranch branch in branches)
            branch.makeParent();

        gitTree = new GitTree()
        {
            RepoName = "SampleRepo",
            Branches = branches,

        };
    }

    private void MakeSingleBranchTree()
    {
        ITreeNode lastNode = null;

        for (var i = 0; i < NUM_NODES; i++)
        {
            nodes.Add(new TreeNode()
            {
                CommitString = i.ToString(),
                Parent = lastNode
            });

            lastNode = nodes[nodes.Count - 1];
        }

        branches.Add(new TreeBranch()
        {
            BranchName = "master",
            BaseNode = nodes[0],
            HeadNode = nodes[NUM_NODES - 1]
        });

        gitTree = new GitTree()
        {
            RepoName = "SampleRepo",
            Branches = branches,

        };
    }

    void PrintTestProperties()
    {
        Debug.Log("Repo name: " + gitTree.RepoName);

        foreach (ITreeBranch branch in gitTree.Branches)
        {
            Debug.Log("Branch name: " + branch.BranchName + " Branch length: " + branch.BranchLength.ToString() + " Comment: " + branch.Comment);
            Debug.Log("Branch name: " + branch.BranchName + " Branch head node: " + branch.HeadNode.CommitString + " Branch base node: " + branch.BaseNode.CommitString);

        }
    }

    void CheckMethods()
    {
        for(var i = 0; i < NUM_NODES; i++)
        {
            if (gitTree.getNthNodeInBranch(i, "master") == null) Debug.Log("Node " + i.ToString() + " does not exist in master!");
            ITreeNode node = gitTree.getNode(i.ToString());
            if (node == null || !node.CommitString.Equals(i.ToString())) Debug.Log("Did not find node " + i.ToString() + " in tree");
        }

        if (gitTree.getBranch("working") == null) Debug.Log("Could not find working branch in tree");
        Debug.Log("Most recent branch is " + gitTree.getMostRecentBranch().BranchName);
    }

    void CheckBuild()
    {
        Vector3 StartPos = new Vector3(0, 0, 0);
        gitTree.buildTree(StartPos,prefab, BranchDistance);
    }
}
