using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FailMail.Models
{
    public class IncomingEmailModel
    {
        public string Id { get; set; }

        public string Sender { get; set; }

        public string Subject { get; set; }

        public string BodyPlain { get; set; }

        public string BodyHtml { get; set; }
    }
}