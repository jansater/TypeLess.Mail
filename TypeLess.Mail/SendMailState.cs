using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeLess.Mail
{
    public enum SendMailState
    {
        Ok = 0,
        FormatError = 1,
        SmtpError = 2,
        Error = 3
    }
}
