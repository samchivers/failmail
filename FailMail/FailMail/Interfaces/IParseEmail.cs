using System.Web;
using FailMail.Models;

namespace FailMail.FailMail.Interfaces
{
    public interface IParseEmail
    {
        /// <summary>
        /// Parse the POST data from a mail client into the
        /// IncomingEmailModel
        /// </summary>
        /// <param name="request"></param>
        /// <returns>IncomingEmailModel</returns>
        IncomingEmailModel ParseIncomingPostData(HttpRequestBase request);

        /// <summary>
        /// Create the IssueModel from the IncomingEmailModel
        /// </summary>
        /// <param name="incomingEmailModel"></param>
        /// <returns>IssueModel</returns>
        IssueModel FillIssueModel(IncomingEmailModel incomingEmailModel);
    }
}
