using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using FailMail.FailMail.Helpers;
using FailMail.FailMail.Interfaces;
using FailMail.Models;
using Octokit;

namespace FailMail.FailMail.Repositories
{
    class GitHubCreateIssueRepository : ICreateIssue
    {
        /// <summary>
        /// GitHub Client object
        /// </summary>
        private readonly GitHubClient _gitHubClient;

        /// <summary>
        /// Are issues going to be placed against a user, 
        /// or an organisation? set in AppSettings
        /// </summary>
        private readonly string _userOrOrganisationRepositoryList;

        /// <summary>
        /// User, or Organisation name owner of the repo. Set
        /// in AppSettings
        /// </summary>
        private readonly string _userOrOrganisationName;

        public GitHubCreateIssueRepository(IGitHubAuth gitHubAuthRepository)
        {
            _gitHubClient = gitHubAuthRepository.CreateClient();

            _userOrOrganisationName = ConfigurationManager.AppSettings["GITHUB_USER_OR_ORGANISATION_NAME"].ToLower();

            _userOrOrganisationRepositoryList = ConfigurationManager.AppSettings["GITHUB_USER_OR_ORGANISATION_REPOSITORY_TYPE"].ToLower();
        }

        /// <summary>
        /// Attempt to create the issue with the data provided 
        /// from the email
        /// </summary>
        /// <param name="issue"></param>
        /// <returns>bool</returns>
        public async Task<bool> CreateIssue(IssueModel issue)
        {
            var validRepoName = await ValidateGithubRepositoryName(issue.RepositoryName);

            var validUsername = await ValidateGithubUsername(issue.Assignee);

            if (validRepoName == false || validUsername == false)
            {
                return false;
            }

            return await CreateGitHubIssue(issue);
        }

        /// <summary>
        /// Validate username against user-provided list, and then that
        /// it's a real github username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>bool</returns>
        private async Task<bool> ValidateGithubUsername(string username)
        {
            if (username == "") return false;

            var validUsernames = ConfigurationManager.AppSettings["GITHUB_VALID_USERNAMES"].Split(',').ToList();

            var appValidUsername = validUsernames.Contains(username);

            if (!appValidUsername) return false;

            try
            {
                var user = await _gitHubClient.User.Get(username);

                return user != null;
            }
            catch (Exception e)
            {
                throw new Exception("ValidateUsernameException", e);
            }

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

            if (_userOrOrganisationRepositoryList == "user")
            {
                try
                {
                    var repositories = await _gitHubClient.Repository.GetAllForUser(_userOrOrganisationName);

                    return repositories.Any(repo => string.Equals(repo.Name, repoName.Trim(), StringComparison.CurrentCultureIgnoreCase));
                }
                catch (Exception e)
                {
                    throw new Exception("ValidateRepositoryException", e);
                }
            }
            else
            {
                try
                {
                    var repositories = await _gitHubClient.Repository.GetAllForOrg(_userOrOrganisationName);

                    return repositories.Any(repo => string.Equals(repo.Name, repoName.Trim(), StringComparison.CurrentCultureIgnoreCase));
                }
                catch (Exception e)
                {
                    throw new Exception("ValidateRepositoryException", e);
                }
            }
        }

        /// <summary>
        /// Create an issue against the provided repository, owned by the username provided and with
        /// the labels requested
        /// </summary>
        /// <param name="issue"></param>
        /// <returns>bool</returns>
        private async Task<bool> CreateGitHubIssue(IssueModel issue)
        {
            var gitHubIssue = new NewIssue(issue.Title)
            {
                Body = ParseEmailHelper.ConvertHtmlToMarkdown(issue.Body),
                Assignee = issue.Assignee
            };

            // Adding labels after due to private set 
            // on Octokit NewIssue.Labels property
            foreach (var label in issue.Labels)
            {
                gitHubIssue.Labels.Add(label);
            }

            try
            {
                var issueResponse = await _gitHubClient.Issue.Create(_userOrOrganisationName, issue.RepositoryName, gitHubIssue);

                return issueResponse != null;
            }
            catch (Exception e)
            {
                throw new Exception("CreateIssueException",e);
            }
        }
    }
}
