using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTree : MonoBehaviour
{
    IGitTree gitTree;
    public int number_to_generate;
    public int distance;
    private TreeBranch treeBranch;
    public GameObject node;

    public List<TreeNode> treeNodes;

    // Start is called before the first frame update
    void Start()
    {
        gitTree = new GitTree();
        treeBranch = new TreeBranch();

        for (int i = 0; i < number_to_generate; i++)
        {
            treeNodes.Add(new TreeNode() {CommitString=i.ToString() });
        }

        for(int i = 0; i < number_to_generate-1; i++)
        {
            treeNodes[i].Parent = treeNodes[i++];
        }

        treeBranch.HeadNode = treeNodes[number_to_generate - 1];
        treeBranch.BaseNode = treeNodes[0];
        treeBranch.BranchName = "master";

        gitTree.RepoName = "GitVR";
        gitTree.HeadNode = treeNodes[0];
        gitTree.Branches = new List<ITreeBranch>(){ treeBranch};

        print("Count: " + gitTree.Branches.Count);
        for(int i = 0; i < gitTree.Branches.Count; i++)
        {
            int branchLength = gitTree.Branches[i].BranchLength;

            for(int n = 0; n < branchLength; n++)
            {
                Instantiate(node, new Vector3(0, n * distance, 0), Quaternion.identity);
                print("Hi");
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
