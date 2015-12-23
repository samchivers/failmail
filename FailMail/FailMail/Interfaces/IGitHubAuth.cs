using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace FailMail.FailMail.Interfaces
{
    public interface IGitHubAuth
    {
        /// <summary>
        /// Create gitHub client object to access their API
        /// </summary>
        /// <returns>GitHub client</returns>
        GitHubClient CreateClient();
    }
}
