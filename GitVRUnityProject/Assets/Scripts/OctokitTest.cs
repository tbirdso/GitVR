﻿using GitHubTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctokitTest : MonoBehaviour
{
    private GitHubInterface gitHubInterface;

    public bool DebugText = false;

    // Start is called before the first frame update
    void Start()
    {
        gitHubInterface = new GitHubInterface(TestCredentials.username, TestCredentials.password);

        if (DebugText)
        {
            gitHubInterface.UpdateCurrentActiveUser("apmason13");
            string DebugString = "";
            DebugString += "Hourly Request Limit: " + gitHubInterface.GetHourlyRequestLimit().ToString() + "\n";
            DebugString += "Request Reset Time: " + gitHubInterface.GetRequestLimitResetTime() + "\n";
            DebugString += "\n";
            DebugString += "Current User's Name: " + gitHubInterface.GetCurrentActiveUsersName() + "\n";
            DebugString += "Current User's # of Public Repos: " + gitHubInterface.GetCurrentActiveUsersNumberOfPublicRepos() + "\n";
            DebugString += "Current User's GitHub URL: " + gitHubInterface.GetCurrentActiveUsersGitHubURL() + "\n";
            DebugString += "\n";
            DebugString += "Current User's Public(?) Repositories: " + "\n";
            foreach (var x in gitHubInterface.GetCurrentActiveUsersRepositories())
            {
                DebugString += "Repo: " + x.FullName + "\n";
                DebugString += "  Branches:" + "\n";
                foreach (var y in gitHubInterface.GetBranchesFromRepo(x))
                {
                    DebugString += "    " + y.Name + "\n";
                }
                DebugString += "\n";

                DebugString += "  Commits:" + "\n";
                foreach (var z in gitHubInterface.GetCommitsFromRepo(x))
                {
                    if (z.Parents != null)
                    {
                        DebugString += "Parents: ";
                        foreach (var a in z.Parents)
                        {
                            DebugString += a.Url + " ";
                        }
                    }
                    DebugString += "Message: " + z.Commit.Message + "\n\n\n";
                }
                DebugString += "\n";
            }

            DebugString += "Remaining Requests: " + gitHubInterface.GetRemainingRequest().ToString();

            UnityEngine.Debug.Log(DebugString);
        }

        foreach (var x in gitHubInterface.GetCommitsFromRepo("numato", "samplecode"))
        {
            print(x);
        }        
        

    }

    // Update is called once per frame
    void Update()
    {

    }
}
