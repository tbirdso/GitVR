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
            UpdateCurrentUser();
        }

        public GitHubInterface(string username, string password)
        {
            Initialize();
            client.Credentials = new Credentials(username, password);
            UpdateCurrentUser();
        }
        
        // changes the client credentials and updates the current user
        public void Login(string username, string password)
        {
            client.Credentials = new Credentials(username, password);
        } 

        public GitHubInterface(string accessToken)
        {
            Initialize();
            client.Credentials = new Credentials(accessToken);
            UpdateCurrentUser();
        }

        // Constructor helper functions
        private void Initialize()
        {
            client = new GitHubClient(new ProductHeaderValue("GitVR"));
        }

        private void UpdateCurrentUser()
        {
            currentUser = client.User.Current().Result;
        }

        private void UpdateCurrentUser(string username)
        {
            currentUser = client.User.Get(username).Result;
        }

        // misc helper functions

        // updates ApiInfo to the latest available
        private void UpdateApiInfo()
        {
            apiInfo = client.GetLastApiInfo();
        }

        /*
         * 
         * Current user getter functions
         * 
         */

        // returns a string containing the login username for the current user 
        internal string GetCurrentUsersLogin()
        {
            return currentUser.Login;
        }

        // returns the max number of requests per hour
        public int GetHourlyRequestLimit()
        {
            UpdateApiInfo();
            if (apiInfo.RateLimit != null)
            {
                return apiInfo.RateLimit.Limit;
            }
            return -1;
        }

        // returns the number remaining requests
        public int GetRemainingRequest()
        {
            UpdateApiInfo();
            if (apiInfo.RateLimit != null)
            {
                return apiInfo.RateLimit.Remaining;
            }
            return -1;
        }

        // returns the time that the remaining requests will reset back to request limit
        public DateTimeOffset GetRequestLimitResetTime()
        {
            UpdateApiInfo();
            if (apiInfo.RateLimit != null)
            {
                return apiInfo.RateLimit.Reset;
            }
            return new DateTimeOffset();
        }

        // returns the current users First and Last Name
        public string GetCurrentUsersName()
        {
            //updateCurrentUser();
            return currentUser.Name;
        }

        // returns the current users number of public repos
        public int GetCurrentUsersNumberOfPublicRepos()
        {
            return currentUser.PublicRepos;
        }

        // returns the url link to current users github page
        public string GetCurrentUsersGitHubURL()
        {
            return currentUser.HtmlUrl;
        }

        // returns a list of the current users repositories
        public IReadOnlyList<Repository> GetCurrentUsersRepositories()
        {
            return client.Repository.GetAllForUser(currentUser.Login).Result;
        }


        /*
         * 
         * any user/repo/etc getter functions
         * 
         */


        // returns a list of branches for a repository given a username and repo
        public IReadOnlyList<Branch> GetBranchesFromRepo(string username, Repository repo)
        {
            return client.Repository.Branch.GetAll(username, repo.Name).Result;
        }

        // returns a list of branches for a repository given a repo
        public IReadOnlyList<Branch> GetBranchesFromRepo(Repository repo)
        {
            return client.Repository.Branch.GetAll(repo.Id).Result;
        }

        // returns a list of all commits for a repository given a username and repo
        public IReadOnlyList<GitHubCommit> GetCommitsFromRepo(string username, Repository repo)
        {
            return client.Repository.Commit.GetAll(username, repo.Name).Result;
        }

        // returns a list of all commits for a repository given a repository
        public IReadOnlyList<GitHubCommit> GetCommitsFromRepo(Repository repo)
        {
            return client.Repository.Commit.GetAll(repo.Id).Result;
        }
    }
}

