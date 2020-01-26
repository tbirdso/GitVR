using GitHubTools;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GitManager : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 dist;
    private GitHubInterface gitHubInterface;
    private IGitTree GithubTree;

    // Start is called before the first frame update
    void Start()
    {
        GithubTree = GetGithubData();
        GithubTree.buildTree(new Vector3(0, 0, 0), prefab, dist);
    }

    public IGitTree GetGithubData()
    {
        gitHubInterface = new GitHubInterface(TestCredentials.username, TestCredentials.password);
        gitHubInterface.UpdateCurrentActiveUser("microsoft");

        var x = gitHubInterface.GetRepository("microsoft", "WPF-Samples");

        IGitTree RepoData = new GitTree()
        {
            RepoName = x.Name,
            Owner = x.Owner.Login,
            Description = x.Description,
        };

        List<ITreeNode> nodes = new List<ITreeNode>();
        var commitList = gitHubInterface.GetCommitsFromRepo(x);
        foreach (var z in commitList.Reverse())
        {
            // FIXME: what if the parent is not present at the time we are parsing but is added later?
            var vals = (from p in z.Parents select p.Sha);
            string parentid = (vals.Count() > 0) ? vals.First() : null;

            var data = (from node in nodes where node.CommitString.Equals(parentid) select node);
            ITreeNode parent = (data.Count() > 0) ? data.First() : null;
            nodes.Add(new TreeNode()
            {
                CommitString = z.Sha,
                Author = z.Commit.Author.Name,
                Message = z.Commit.Message,
                ParentID = parentid,
                Parent = parent
            });
        }
        RepoData.HeadNode = nodes.Last();

        List<ITreeBranch> branches = new List<ITreeBranch>();
        foreach (var y in gitHubInterface.GetBranchesFromRepo(x))
        {
            List<ITreeNode> queryResult = (from node in nodes where node.CommitString.Equals(y.Commit.Sha) select node).ToList<ITreeNode>();
            if (queryResult.Count() > 0) {
                branches.Add(new TreeBranch()
                {
                    BranchName = y.Name,
                    HeadNode = queryResult.ElementAt(0)
                });
            }
        }

        RepoData.Nodes = nodes;
        RepoData.Branches = branches;
        RepoData.makeBranchesFromData();
        foreach (ITreeBranch branch in RepoData.Branches)
            branch.makeParent();

        return RepoData;
    }
}
