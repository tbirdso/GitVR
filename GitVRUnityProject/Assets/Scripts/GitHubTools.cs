using System;
using Octokit;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

namespace GitHubTools
{
    class GitHubInterface
    {
        // variables
        private GitHubClient client;
        private ApiInfo apiInfo;
        private User currentUser;

        //constructors
        public GitHubInterface()
        {
            updateCurrentUser();
        }

        public GitHubInterface(string username, string password)
        {
            Initialize();
            client.Credentials = new Credentials(username, password);
            updateCurrentUser();
        }

        public GitHubInterface(string accessToken)
        {
            Initialize();
            client.Credentials = new Credentials(accessToken);
            updateCurrentUser();
        }

        // Constructor helper functions
        private void Initialize()
        {
            client = new GitHubClient(new ProductHeaderValue("GitVR"));
        }

        private void updateCurrentUser()
        {
            currentUser = client.User.Current().Result;
        }

        // misc helper functions

        // updates ApiInfo to the latest available
        private void updateApiInfo()
        {
            apiInfo = client.GetLastApiInfo();
        }

        // returns the max number of requests per hour
        public int getHourlyRequestLimit()
        {
            updateApiInfo();
            if (apiInfo.RateLimit != null)
            {
                return apiInfo.RateLimit.Limit;
            }
            return -1;
        }

        // returns the number remaining requests
        public int getRemainingRequest()
        {
            updateApiInfo();
            if (apiInfo.RateLimit != null)
            {
                return apiInfo.RateLimit.Remaining;
            }
            return -1;
        }

        // returns the time that the remaining requests will reset back to request limit
        public DateTimeOffset getRequestLimitResetTime()
        {
            updateApiInfo();
            if (apiInfo.RateLimit != null)
            {
                return apiInfo.RateLimit.Reset;
            }
            return new DateTimeOffset();
        }

        // returns the current users First and Last Name
        public string getCurrentUsersName()
        {
            //updateCurrentUser();
            return currentUser.Name;
        }

        public int getCurrentUsersNumberOfPublicRepos()
        {
            return currentUser.PublicRepos;
        }

        public string getCurrentUsersGitHubURL()
        {
            return currentUser.HtmlUrl;
        }
    }
}

