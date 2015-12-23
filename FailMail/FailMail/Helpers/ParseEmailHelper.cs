using System;
using System.Collections.Generic;
using System.Linq;

namespace FailMail.FailMail.Helpers
{
    public class ParseEmailHelper
    {
        /// <summary>
        /// Examine the email message returned for the presence of [Labels:one,two,three]
        /// and parse the results into a list in order to pass them through to GitHub
        /// </summary>
        /// <param name="message"></param>
        /// <returns>List of labels, or empty list if there are none</returns>
        public static List<string> ParseLabels(string message)
        {
            var labels = new List<string>();

            if (message.Contains("[Labels:") != true) return labels;

            var startChar = message.IndexOf("[Labels", StringComparison.Ordinal) + "[Labels:".Length;

            var restOfMessage = message.Substring(startChar);

            var endChar = restOfMessage.IndexOf("]", StringComparison.Ordinal) + startChar;

            var labelsList = message.Substring(startChar, endChar - startChar);

            labels = labelsList.Split(',').Select(l => l.Trim()).ToList();

            return labels;
        }

        /// <summary>
        /// Examine the email message returned for the presence of [Owner:githubusername]
        /// and parse the results into a string 
        /// </summary>
        /// <param name="message"></param>
        /// <returns>GitHub username</returns>
        public static string ParseOwner(string message)
        {
            if (message.Contains("[Owner:") != true) return "";

            var startChar = message.IndexOf("[Owner", StringComparison.Ordinal) + "[Owner:".Length;

            var restOfMessage = message.Substring(startChar);

            var endChar = restOfMessage.IndexOf("]", StringComparison.Ordinal) + startChar;

            return message.Substring(startChar, endChar - startChar).Trim();
        }

        public static string ParseTargetRepository(string message)
        {
            if (message.Contains("[Repo:") != true) return "";

            var startChar = message.IndexOf("[Repo", StringComparison.Ordinal) + "[Repo:".Length;

            var restOfMessage = message.Substring(startChar);

            var endChar = restOfMessage.IndexOf("]", StringComparison.Ordinal) + startChar;

            return message.Substring(startChar, endChar - startChar).Trim();
        }

        /// <summary>
        /// Convert HTML to GitHub flavoured markdown for the body of the issue report
        /// </summary>
        /// <param name="bodyHtml"></param>
        /// <returns>Markdown version of HTML email body</returns>
        public static string ConvertHtmlToMarkdown(string bodyHtml)
        {
            const string unknownTagsConverter = "pass_through";

            var config = new ReverseMarkdown.Config(unknownTagsConverter, true);

            var converter = new ReverseMarkdown.Converter(config);

            return converter.Convert(bodyHtml);
        }

        /// <summary>
        /// Remove RE: or FW: from the start of the subject line
        /// </summary>
        /// <param name="subject"></param>
        /// <returns>Subject minus RE: or FW:</returns>
        public static string RemoveFowardOrReplyCharactersFromSubject(string subject)
        {
            var needToExamineSubject = subject.ContainsAny("RE:", "FW:");

            return needToExamineSubject ? subject.Substring(3).Trim() : subject.Trim();
        }
    }
}