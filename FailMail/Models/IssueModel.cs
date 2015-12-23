using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace FailMail.Models
{
    public class IssueModel
    {
        public string Owner { get; set; }
        public string Body { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public List<string> Labels { get; set; }
        public string Title { get; set; }
        public string RepositoryName { get; set; }
    }
}
