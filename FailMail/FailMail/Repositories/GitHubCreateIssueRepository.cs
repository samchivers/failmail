using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using FailMail.FailMail.Interfaces;
using FailMail.Models;
using Octokit;

namespace FailMail.FailMail.Repositories
{
    class GitHubCreateIssueRepository : ICreateIssue
    {
        private readonly GitHubClient _gitHubClient;

        public GitHubCreateIssueRepository(IGitHubAuth gitHubAuthRepository)
        {
            _gitHubClient = gitHubAuthRepository.CreateClient();
        }

        /// <summary>
        /// Attempt to create the issue with the data provided 
        /// from the email
        /// </summary>
        /// <param name="issue"></param>
        /// <returns>bool</returns>
        public async Task<bool> CreateIssue(IssueModel issue)
        {
            // Check repo name is valid
            var validRepoName = await ValidateGithubRepositoryName(issue.RepositoryName);

            var validUsername = await ValidateGithubUsername(issue.Owner);

            if (validRepoName == false || validUsername == false)
            {
                return false;
            }

            // Create Issue

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task<bool> ValidateGithubUsername(string username)
        {
            if (username == "") return false;

            return true;
        }

        /// <summary>
        /// Validate the repository name is that the GitHub auth'd user
        /// has the rights to add an issue against 
        /// </summary>
        /// <param name="repoName"></param>
        /// <returns>bool</returns>
        private async Task<bool> ValidateGithubRepositoryName(string repoName)
        {
            if (repoName == "") return false;

            var userOrOrganisationRepositoryList = ConfigurationManager.AppSettings["GITHUB_USER_OR_ORGANISATION_REPOSITORY_LIST"].ToLower();

            var userOrOrganisationName = ConfigurationManager.AppSettings["GITHUB_USER_OR_ORGANISATION_NAME"].ToLower();

            if (userOrOrganisationRepositoryList == "user")
            {
                var repositories = await _gitHubClient.Repository.GetAllForUser(userOrOrganisationName);

                return repositories.Any(repo => string.Equals(repo.Name, repoName.Trim(), StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                var repositories = await _gitHubClient.Repository.GetAllForOrg(userOrOrganisationName);

                return repositories.Any(repo => string.Equals(repo.Name, repoName.Trim(), StringComparison.CurrentCultureIgnoreCase));
            } 
        }
    }
}
