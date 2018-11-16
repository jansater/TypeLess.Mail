using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Mail
{
    public class TypeLessMail
    {
        public EmailSettings Settings { get; set; }
        public Contact From { get; set; }
        public IEnumerable<Contact> To { get; set; }
        public Meeting Meeting { get; set; }

        public TypeLessMail()
        {
            Settings = new EmailSettings();
            Attachments = new List<Attachment>();
            To = new List<Contact>();
            Headers = new List<Tuple<string, string>>();
        }

        public string Subject { get; set; }
        public string Content { get; set; }
        public string Body { get; set; }
        public IList<Attachment> Attachments { get; set; }

        public Contact ReplyTo { get; set; }
        public List<Tuple<string, string>> Headers { get; set; }
        
    }
}
