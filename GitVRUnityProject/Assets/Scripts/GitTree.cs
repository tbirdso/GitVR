using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GitTree : MonoBehaviour, IGitTree
{
    public string RepoName { get; set; }
    public TreeNode HeadNode { get; set; }
    public List<TreeBranch> Branches { get; set; }

    public TreeBranch getMostRecentBranch()
    {
        print("FIXME: Implement branches");
        return new TreeBranch();
    }

    public TreeBranch getBranch(string branchName)
    {
        print("FIXME: Implement branches");
        return new TreeBranch();
    }

    public TreeNode getNthNodeInBranch(string branchName)
    {
        print("FIXME: Implement branches");
        return new TreeNode();
    }
}
