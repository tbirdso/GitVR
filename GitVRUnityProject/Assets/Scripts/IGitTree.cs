using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IGitTree
{
    string RepoName { get; set; }
    TreeNode HeadNode { get; set; }
    List<TreeBranch> Branches { get; set; }
    
    TreeBranch getMostRecentBranch();
    TreeBranch getBranch(string branchName);
    TreeNode getNthNodeInBranch(string branchName);

}
