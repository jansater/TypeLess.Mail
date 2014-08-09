
using System;
using TypeLess;

namespace TypeLess.Mail
{
    public class EmailSettings 
    {
        public string SMTPServer { get; set; }
        public bool SMTPAuthentication { get; set; }
        public bool SMTPEnableSSL { get; set; }
        public string SMTPUserEmail { get; set; }
        public string SMTPUsername { get; set; }
        public string SMTPPassword { get; set; }
        public int SMTPort { get; set; }
        public System.Text.Encoding SubjectEncoding { get; set; }
        public System.Text.Encoding BodyEncoding { get; set; }
        public string CharSet { get; set; }
        public string TemplateDirectory { get; set; }

        public System.Net.Mail.SmtpDeliveryMethod DeliveryMethod { get; set; }

        public int SMTPSSLPort { get; set; }
    }
}
