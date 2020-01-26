using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode : MonoBehaviour
{
    // Hash string representing the commit ID tag
    public string CommitString;
    
    // List representing links to successors, where the length of the list is equivalent to
    // the number of branches emerging from the node
    public List<TreeNode> Children;

    // Object representing the parent node. There can only be one true "parent" node.
    public TreeNode Parent;
    
}
