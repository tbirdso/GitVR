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

        /*
         * 
         * change user functions
         * 
         */

        // changes the client credentials and updates the currentLoggedInuser
        public void Login(string username, string password)
        {
            client.Credentials = new Credentials(username, password);
            updateCurrentLoggedInUser();
        }

        // forces an api call to change currentLoggedInUser
        private void updateCurrentLoggedInUser()
        {
            currentLoggedInUser = client.User.Current().Result;
        }

        // changes currentActiveUser to currentLoggedInUser
        public void SetCurrentActiveUserToCurrentLoggedInUser()
        {
            currentActiveUser = currentLoggedInUser;
        }

        // sets the currentActiveUser to the specified username
        public void UpdateCurrentActiveUser(string username)
        {
            currentActiveUser = client.User.Get(username).Result;
        }

        /*
         * 
         * misc helper functions
         * 
         */

        // updates ApiInfo to the latest available
        private void UpdateApiInfo()
        {
            apiInfo = client.GetLastApiInfo();
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

        // returns the current users First and Last Name
        public string GetCurrentActiveUsersName()
        {
            //updateCurrentUser();
            return currentActiveUser.Name;
        }

        // returns the current users number of public repos
        public int GetCurrentActiveUsersNumberOfPublicRepos()
        {
            return currentActiveUser.PublicRepos;
        }

        // returns the url link to current users github page
        public string GetCurrentActiveUsersGitHubURL()
        {
            return currentActiveUser.HtmlUrl;
        }

        // returns a list of the current users repositories
        public IReadOnlyList<Repository> GetCurrentActiveUsersRepositories()
        {
            return client.Repository.GetAllForUser(currentActiveUser.Login).Result;
        }


        /*
         * 
         * any user/repo/etc getter functions
         * 
         */


        // returns the repo from its name
        public Repository GetRepository(string username, string repo)
        {
            return client.Repository.Get(username, repo).Result;
        }

        // returns a list of branches for a repository given a username and repo
        public IReadOnlyList<Branch> GetBranchesFromRepo(string username, string repo)
        {
            return client.Repository.Branch.GetAll(username, repo).Result;
        }

        // returns a list of branches for a repository given a repo
        public IReadOnlyList<Branch> GetBranchesFromRepo(Repository repo)
        {
            return client.Repository.Branch.GetAll(repo.Id).Result;
        }

        // returns a list of all commits for a repository given a username and repo
        public IReadOnlyList<GitHubCommit> GetCommitsFromRepo(string username, string repo)
        {
            return client.Repository.Commit.GetAll(username, repo).Result;
        }

        // returns a list of all commits for a repository given a repository
        public IReadOnlyList<GitHubCommit> GetCommitsFromRepo(Repository repo)
        {
            return client.Repository.Commit.GetAll(repo.Id).Result;
        }

        /*
         * 
         * Miscellaneous Helper Functions
         * 
         */

        // returns the commit summary from a commit
        public string GetSummaryFromCommit(Commit commit)
        {
            string message = commit.Message;
            if (string.IsNullOrEmpty(message)) return message;
            for(int i = 0; i < message.Length; i++)
            {
                if (message[i] == '\n')
                {
                    return message.Substring(0, i);
                }
            }
            return message;
        }

        public void testFunction()
        {
            client.Repository.Create(new NewRepository("ThomathyBirdsongVR"));
            //var x = GetCommitsFromRepo(GetCurrentActiveUsersRepositories()[0])[0].Commit.Parents;
            //if (x != null)
            //{
            //    foreach (var y in x)
            //    {
            //        Debug.Log(y.Label);
            //    }
            //}
        }
    }
}

