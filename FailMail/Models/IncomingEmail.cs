using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FailMail.Models
{
    public class IncomingEmail
    {
        public string Signature { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string BodyPlain { get; set; }
        public string BodyHtml { get; set; }
    }
}