using System;
using Octokit;
using System.Threading.Tasks;
using UnityEngine;

namespace GitHubTools
{
    class GitHubInterface
    {
        // variables
        private GitHubClient client;
        private ApiInfo apiInfo;

        //constructors
        public GitHubInterface()
        {
            Initialize();
        }

        public GitHubInterface(string username, string password)
        {
            Initialize();
            client.Credentials = new Credentials(username, password);
        }

        // Constructor helper functions
        private void Initialize()
        {
            client = new GitHubClient(new ProductHeaderValue("GitVR"));
        }

        // misc helper functions
        private void updateApiInfo()
        {
            apiInfo = client.GetLastApiInfo();
        }

        //public void getInfo()
        //{
        //    // If the ApiInfo isn't null, there will be a property called RateLimit
        //    var rateLimit = apiInfo?.RateLimit;

        //    var howManyRequestsCanIMakePerHour = rateLimit?.Limit;
        //    var howManyRequestsDoIHaveLeft = rateLimit?.Remaining;
        //    var whenDoesTheLimitReset = rateLimit?.Reset; // UTC time


        //    Console.WriteLine(rateLimit);
        //    Console.WriteLine(howManyRequestsCanIMakePerHour);
        //    Console.WriteLine(howManyRequestsDoIHaveLeft);
        //    Console.WriteLine(whenDoesTheLimitReset);
        //}
        public int getHourlyRequestLimit()
        {
            updateApiInfo();
            if (apiInfo.RateLimit != null)
            {
                return apiInfo.RateLimit.Limit;
            }
            return -1;
        }

        public int getRemainingRequest()
        {
            updateApiInfo();
            if (apiInfo.RateLimit != null)
            {
                return apiInfo.RateLimit.Remaining;
            }
            return -1;
        }

        public string getUserInfo(string userid)
        {
            var user = client.User.Get(userid);
            return user.Result.Name + " has " + user.Result.PublicRepos + " public repositories - go check out their profile at " + user.Result.Url;
        }
    }
}

