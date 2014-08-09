using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        public string Subject { get; set; }
        public string Content { get; set; }
        public string Body { get; set; }
        public IList<Attachment> Attachments { get; set; }

        public Contact ReplyTo { get; set; }
    }
}
