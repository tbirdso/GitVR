using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBranch : MonoBehaviour
{
    public string Name;
    public TreeNode HeadNode;
    public int BranchLength;
    public string LastUpdated;

    public TreeNode getNthNodeInBranch(int n)
    {
        TreeNode retNode = HeadNode;

        for (int i = 0; i < n && retNode.Parent != null; i++)
        {
            retNode = retNode.Parent;
        }

        return retNode;
    }
}
