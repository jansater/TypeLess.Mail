using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Mail
{
    public class SendMailResult
    {
        public Exception Exception { get; set; }
        public SendMailState State { get; set; }
        public string ExtraLog { get; set; }
    }
}
