using System;
using System.Web;
using FailMail.FailMail.Helpers;
using FailMail.FailMail.Interfaces;
using FailMail.Models;

namespace FailMail.FailMail.Repositories
{
    class MailgunParseEmailRepository : IParseEmail
    {
        /// <summary>
        /// Parse the POST data from a mail client into the
        /// IncomingEmailModel
        /// </summary>
        /// <param name="request"></param>
        /// <returns>IncomingEmailModel</returns>
        public IncomingEmailModel ParseIncomingPostData(HttpRequestBase request)
        {
            return new IncomingEmailModel
            {
                Id = request["signature"],
                Sender = request["sender"],
                Subject = request["subject"],
                // Dangerous content already parsed out by MailGun
                BodyPlain = request.Unvalidated["body-plain"],
                // Dangerous content already parsed out by MailGun
                BodyHtml = request.Unvalidated["body-html"]
            };
        }

        /// <summary>
        /// Create the IssueModel from the IncomingEmailModel
        /// </summary>
        /// <param name="incomingEmailModel"></param>
        /// <returns>IssueModel</returns>
        public IssueModel FillIssueModel(IncomingEmailModel incomingEmailModel)
        {
            return new IssueModel
            {
                Assignee = ParseEmailHelper.ParseOwner(incomingEmailModel.BodyPlain),
                Body = ParseEmailHelper.ConvertHtmlToMarkdown(incomingEmailModel.BodyHtml),
                Labels = ParseEmailHelper.ParseLabels(incomingEmailModel.BodyPlain),
                Title = ParseEmailHelper.RemoveFowardOrReplyCharactersFromSubject(incomingEmailModel.Subject),
                RepositoryName = ParseEmailHelper.ParseTargetRepository(incomingEmailModel.BodyPlain),
                CreatedAt = DateTime.Now
            };
        }


    }
}
