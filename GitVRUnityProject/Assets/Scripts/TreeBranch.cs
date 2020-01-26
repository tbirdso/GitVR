using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeBranch : MonoBehaviour, ITreeBranch
{
    public string BranchName { get; set; }
    public ITreeNode HeadNode { get; set; }
    public ITreeNode BaseNode { get; set; }
    public int BranchLength {
        get
        {
            int counter = 1;
            ITreeNode rover = HeadNode;
            while(rover.Parent != null && !rover.CommitString.Equals(BaseNode.CommitString))
            {
                counter++;
                rover = rover.Parent;
            }

            return counter;
        }
    }
    public DateTime LastModified {
        get
        {
            return HeadNode.LastModified;
        }

    }
    public string Comment { get; set; }

    public ITreeNode getNthNode(int n)
    {
        if (n > BranchLength) return null;

        ITreeNode retNode = HeadNode;

        for (int i = 0; i < n && retNode.Parent != null && !retNode.CommitString.Equals(BaseNode); i++)
        {
            retNode = retNode.Parent;
        }

        return retNode;
    }

    public void makeParent()
    {
        ITreeNode rover = HeadNode;

        while(rover.Parent != null && !rover.CommitString.Equals(BaseNode.CommitString))
        {
            if (rover.Parent.Children == null) rover.Parent.Children = new List<ITreeNode>();

            IEnumerable<ITreeNode> queryResult = rover.Parent.Children.Where(child => child.CommitString.Equals(rover.CommitString));
            if (queryResult.Count() == 0) rover.Parent.Children.Add(rover);

            rover = rover.Parent;
        }
    }
}

public interface ITreeBranch
{
    string BranchName { get; set; }
    ITreeNode HeadNode { get; set; }
    ITreeNode BaseNode { get; set; }
    int BranchLength { get; }
    DateTime LastModified { get; }
    string Comment { get; set; }
    
    ITreeNode getNthNode(int n);
    void makeParent();
}