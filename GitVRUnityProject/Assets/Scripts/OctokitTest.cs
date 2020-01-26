﻿using GitHubTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctokitTest : MonoBehaviour
{
    private GitHubInterface gitHubInterface;

    public bool Debug = false;

    // Start is called before the first frame update
    void Start()
    {
        gitHubInterface = new GitHubInterface(TestCredentials.AccessToken);

        if (Debug)
        {
            string DebugString = "";
            DebugString += "Hourly Request Limit: " + gitHubInterface.getHourlyRequestLimit().ToString() + "\n";
            DebugString += "Request Reset Time: " + gitHubInterface.getRequestLimitResetTime() + "\n";
            DebugString += "\n";
            DebugString += "Current User's Name: " + gitHubInterface.getCurrentUsersName() + "\n";
            DebugString += "Current User's # of Public Repos: " + gitHubInterface.getCurrentUsersNumberOfPublicRepos() + "\n";
            DebugString += "Current User's GitHub URL: " + gitHubInterface.getCurrentUsersGitHubURL() + "\n";
            DebugString += "\n";
            DebugString += "Current User's Public(?) Repositories: " + "\n";
            foreach (var x in gitHubInterface.getCurrentUsersRepositories())
            {
                DebugString += x.FullName + "\n";
                DebugString += "  Branches:" + "\n";
                foreach (var y in gitHubInterface.getBranchesFromRepo(gitHubInterface.getCurrentUsersLogin(), x))
                {
                    DebugString += "    " + y.Name + "\n";
                }
                DebugString += "\n";

                DebugString += "  Commits:" + "\n";
                foreach (var z in gitHubInterface.getCommitsFromRepo(x))
                {
                    DebugString += "Message: " + z.Commit.Message + "\n";
                }
                DebugString += "\n";
            }

            DebugString += "Remaining Requests: " + gitHubInterface.getRemainingRequest().ToString();

            print(DebugString);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
