﻿
using System;
using System.Text;
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
        public System.Net.Mail.SmtpDeliveryFormat DeliveryFormat { get; set; }

        public int SMTPSSLPort { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("Server: {0}", SMTPServer ?? "");
            sb.AppendLine();

            if (SMTPAuthentication) {
                sb.AppendFormat("Auth: {0}/{1}", SMTPUsername ?? "", "*******");
            }
            sb.AppendLine();
            if (SMTPEnableSSL)
            {
                sb.AppendFormat("SSL Port: {0}", SMTPSSLPort);
            }
            else {
                sb.AppendFormat("Port: {0}", SMTPort);
            }
            sb.AppendLine();
            sb.AppendFormat("Template Directory: {0}", TemplateDirectory ?? "");
            sb.AppendLine();
            sb.AppendFormat("Charset: {0}", CharSet ?? "");
            sb.AppendLine();
            sb.AppendFormat("Subject encoding: {0}", SubjectEncoding == null ? "" : SubjectEncoding.EncodingName);
            sb.AppendLine();
            sb.AppendFormat("Body encoding: {0}", BodyEncoding == null ? "" : BodyEncoding.EncodingName);

            return sb.ToString();
        }
    }
}
