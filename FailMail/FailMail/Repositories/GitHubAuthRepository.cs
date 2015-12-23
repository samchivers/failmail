using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FailMail.FailMail.Interfaces;
using Octokit;

namespace FailMail.FailMail.Repositories
{
    public class GitHubAuthRepository : IGitHubAuth
    {
        public GitHubClient Client = new GitHubClient(new ProductHeaderValue("FailMail"));

        // Personal Access Token from GitHub
        private readonly string _personalAccessToken = ConfigurationManager.AppSettings["GITHUB_PERSONAL_ACCESS_TOKEN"];

        /// <summary>
        /// Instantiates GitHub's client object which is used to make calls to their API
        /// </summary>
        /// <returns>GitHub client object</returns>
        public GitHubClient CreateClient()
        {
            // Client object requires credentials to auth with GitHub
            var tokenAuth = new Credentials(_personalAccessToken);
            Client.Credentials = tokenAuth;

            // Return client object with credentials set 
            return Client;
        }
    }
}
