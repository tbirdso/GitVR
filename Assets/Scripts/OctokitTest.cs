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
        gitHubInterface = new GitHubInterface(TestCredentials.username, TestCredentials.password);
        print(gitHubInterface.getUserInfo("apmason13"));
        print("Hourly Request Limit: " + gitHubInterface.getHourlyRequestLimit().ToString());
        print("Remaining Requests: " + gitHubInterface.getRemainingRequest().ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
