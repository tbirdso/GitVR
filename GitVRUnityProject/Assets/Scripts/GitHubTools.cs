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
        
        // changes the client credentials and updates the current user
        public void login(string username, string password)
        {
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



        // returns a string containing the login username for the current user 
        internal string getCurrentUsersLogin()
        {
            return currentUser.Login;
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

        // returns the current users number of public repos
        public int getCurrentUsersNumberOfPublicRepos()
        {
            return currentUser.PublicRepos;
        }

        // returns the url link to current users github page
        public string getCurrentUsersGitHubURL()
        {
            return currentUser.HtmlUrl;
        }

        // returns a list of the current users repositories
        public IReadOnlyList<Repository> getCurrentUsersRepositories()
        {
            return client.Repository.GetAllForUser(currentUser.Login).Result;
        }

        // returns a list of branches for a repository given a username and repo
        public IReadOnlyList<Branch> getBranchesFromRepo(string username, Repository repo)
        {
            return client.Repository.Branch.GetAll(username, repo.Name).Result;
        }

        // returns a list of branches for a repository given a repo
        public IReadOnlyList<Branch> getBranchesFromRepo(Repository repo)
        {
            return client.Repository.Branch.GetAll(repo.Id).Result;
        }

        // returns a list of all commits for a repository given a username and repo
        public IReadOnlyList<GitHubCommit> getCommitsFromRepo(string username, Repository repo)
        {
            return client.Repository.Commit.GetAll(username, repo.Name).Result;
        }

        // returns a list of all commits for a repository given a repository
        public IReadOnlyList<GitHubCommit> getCommitsFromRepo(Repository repo)
        {
            return client.Repository.Commit.GetAll(repo.Id).Result;
        }
    }
}

