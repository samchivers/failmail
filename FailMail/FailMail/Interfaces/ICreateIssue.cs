using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FailMail.Models;

namespace FailMail.FailMail.Interfaces
{
    public interface ICreateIssue
    {
        /// <summary>
        /// Create the Issue from the IssueModel
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        Task<bool> CreateIssue(IssueModel issue);
    }
}
