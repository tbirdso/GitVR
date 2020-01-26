using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeNode : MonoBehaviour, ITreeNode
{
    // Hash string representing the commit ID tag
    public string CommitString { get; set; }
    
    public string Author { get; set; }

    // List representing links to successors, where the length of the list is equivalent to
    // the number of branches emerging from the node
    public List<ITreeNode> Children { get; set; }

    // Object representing the parent node. There can only be one true "parent" node.
    public ITreeNode Parent { get; set; }

    public string ParentID { get; set; }

    // GameObject for node visualization
    public GameObject NodeObject { get; set; }
    
    public DateTime LastModified { get; set; }
    public string Message { get; set; }
}

public interface ITreeNode
{
    string CommitString { get; set; }
    string Author { get; set; }
    DateTime LastModified { get; set; }
    string Message { get; set; }
    List<ITreeNode> Children { get; set; }
    ITreeNode Parent { get; set; }
    string ParentID { get; set; }
    GameObject NodeObject { get; set; }
}