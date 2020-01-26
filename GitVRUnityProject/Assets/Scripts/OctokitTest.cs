using GitHubTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctokitTest : MonoBehaviour
{
    private GitHubInterface gitHubInterface;

    // Start is called before the first frame update
    void Start()
    {
        gitHubInterface = new GitHubInterface(TestCredentials.AccessToken);

        print("Hourly Request Limit: " + gitHubInterface.getHourlyRequestLimit().ToString());
        print("Remaining Requests: " + gitHubInterface.getRemainingRequest().ToString());
        print("Request Reset Time: " + gitHubInterface.getRequestLimitResetTime());
        print("\n");
        print("Current User's Name: " + gitHubInterface.getCurrentUsersName());
        print("Current User's # of Public Repos: " + gitHubInterface.getCurrentUsersNumberOfPublicRepos());
        print("Current User's GitHub URL: " + gitHubInterface.getCurrentUsersGitHubURL());
        print("\n");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
