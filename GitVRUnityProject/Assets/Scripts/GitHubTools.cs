﻿using System;
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
        private User currentLoggedInUser;
        private User currentActiveUser;

        //constructors
        public GitHubInterface()
        {
            Initialize();
            InitializePhaseTwo();
        }

        public GitHubInterface(string username, string password)
        {
            Initialize();
            client.Credentials = new Credentials(username, password);
            InitializePhaseTwo();
        }

        public GitHubInterface(string accessToken)
        {
            Initialize();
            client.Credentials = new Credentials(accessToken);
            InitializePhaseTwo();
        }

        // Constructor helper functions
        private void Initialize()
        {
            client = new GitHubClient(new ProductHeaderValue("GitVR"));
        }

        private void InitializePhaseTwo()
        {
            updateCurrentLoggedInUser();
            currentActiveUser = currentLoggedInUser;
        }

        // changes the client credentials and updates the current user
        public void Login(string username, string password)
        {
            client.Credentials = new Credentials(username, password);
            updateCurrentLoggedInUser();
        }

        // forces a api call to change currentLoggedInUser
        private void updateCurrentLoggedInUser()
        {
            currentLoggedInUser = client.User.Current().Result;
        }

        // changes currentActiveUser to currentLoggedInUser
        public void SetCurrentUserToLoggedInUser()
        {
            currentActiveUser = currentLoggedInUser;
        }

        public void UpdateCurrentActiveUser(string username)
        {
            currentLoggedInUser = client.User.Get(username).Result;
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
        public string GetCurrentActiveUsersLogin()
        {
            return currentActiveUser.Login;
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
        public string GetCurrentActiveUsersName()
        {
            //updateCurrentUser();
            return currentActiveUser.Name;
        }

        // returns the current users number of public repos
        public int GetCurrentActiveUsersNumberOfPublicRepos()
        {
            return currentLoggedInUser.PublicRepos;
        }

        // returns the url link to current users github page
        public string GetCurrentActiveUsersGitHubURL()
        {
            return currentLoggedInUser.HtmlUrl;
        }

        // returns a list of the current users repositories
        public IReadOnlyList<Repository> GetCurrentActiveUsersRepositories()
        {
            return client.Repository.GetAllForUser(currentLoggedInUser.Login).Result;
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

