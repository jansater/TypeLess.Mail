using HtmlAgilityPack;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Mail
{
    public class MailBuilder :
        IMailBuilder,
        IMailConfiguration,
        IPartialMailI,
        IPartialMailII,
        IPartialMailIII,
        IPartialMailIIII,
        IMailReadyToSend
    {
        private TypeLessMail _mail;
        private ITemplateService _templateService;

        private MailBuilder(ITemplateService _razorTemplateService)
        {
            _templateService = _razorTemplateService;
            _mail = new TypeLessMail();

            _mail.Settings.SMTPAuthentication = false;
            _mail.Settings.SMTPEnableSSL = false;
            _mail.Settings.SMTPort = 25;
            _mail.Settings.SMTPSSLPort = 587;
            _mail.Settings.SMTPServer = "localhost";
            _mail.Settings.TemplateDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates");
            _mail.Settings.SubjectEncoding = Encoding.UTF8;
            _mail.Settings.BodyEncoding = Encoding.UTF8;
            _mail.Settings.CharSet = "UTF-8";
            _mail.Settings.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        }

        public static IMailBuilder Create(ITemplateService service)
        {
            return new MailBuilder(service);
        }

        public static IMailBuilder Create()
        {
            return new MailBuilder(null);
        }

        public IMailConfiguration Configure
        {
            get { return this; }
        }

        public IMailConfiguration SMTPServer(string server)
        {
            server.If("smtp server").IsNull.IsEmptyOrWhitespace.ThenThrow();

            _mail.Settings.SMTPServer = server;
            return this;
        }

        public IMailConfiguration RequiresSMTPAuthentication(bool enable, string username, string password)
        {
            if (enable)
            {
                username.If("username").Or(password, "password").IsNull.IsEmptyOrWhitespace.ThenThrow();
                _mail.Settings.SMTPAuthentication = true;
                _mail.Settings.SMTPUsername = username;
                _mail.Settings.SMTPPassword = password;
            }

            return this;
        }

        public IMailConfiguration TemplateDirectory(string directory)
        {
            directory.If("username").IsNull.IsEmptyOrWhitespace.ThenThrow();
            _mail.Settings.TemplateDirectory = directory;
            return this;
        }

        IMailConfiguration IMailConfiguration.EnableSSL(bool enable, int? sslPort)
        {
            _mail.Settings.SMTPEnableSSL = enable;
            if (sslPort.HasValue) {
                _mail.Settings.SMTPSSLPort = sslPort.Value;
            }
            return this;
        }

        public IMailConfiguration SmtpDefaultFromEmail(string email)
        {
            email.If("smtp from email").IsNull.IsNotValidEmail.ThenThrow();
            _mail.Settings.SMTPUserEmail = email;
            return this;
        }

        public IMailConfiguration SmtpPort(int port)
        {
            port.If("smtp port").IsSmallerThanOrEqualTo(0).ThenThrow();
            _mail.Settings.SMTPort = port;
            return this;
        }

        public IMailConfiguration SmtpSSLPort(int port)
        {
            port.If("smtp port").IsSmallerThanOrEqualTo(0).ThenThrow();
            _mail.Settings.SMTPSSLPort = port;
            return this;
        }


        public IMailConfiguration SubjectEncoding(Encoding encoding)
        {
            encoding.If("subject encoding").IsNull.ThenThrow();
            _mail.Settings.SubjectEncoding = encoding;
            return this;
        }

        public IMailConfiguration BodyEncoding(Encoding encoding)
        {
            encoding.If("body encoding").IsNull.ThenThrow();
            _mail.Settings.BodyEncoding = encoding;
            return this;
        }

        public IMailConfiguration Charset(string charset)
        {
            charset.If("charset").IsNull.ThenThrow();
            _mail.Settings.CharSet = charset;
            return this;
        }

        public IMailConfiguration SmtpDeliveryMethod(SmtpDeliveryMethod deliveryMethod)
        {
            _mail.Settings.DeliveryMethod = deliveryMethod;
            return this;
        }



        public IPartialMailI WithConfiguration(EmailSettings settings)
        {
            settings.If("settings").IsNull.ThenThrow();
            _mail.Settings = settings;
            return this;
        }

        public EmailSettings GetSettings()
        {
            return _mail.Settings;
        }

        public IPartialMailI Done
        {
            get
            {
                return this;
            }
        }

        public IPartialMailII From(Contact from)
        {
            from.If("from").IsInvalid.ThenThrow();
            _mail.From = from;
            return this;
        }

        public IPartialMailII From(string email, string name = null)
        {
            email.If("email").IsNull.IsNotValidEmail.ThenThrow();

            if (String.IsNullOrEmpty(name))
            {
                _mail.From = new Contact(email);
            }
            else
            {
                _mail.From = new Contact(email, name);
            }

            return this;
        }

        public IPartialMailII FromDefault
        {
            get
            {
                _mail.From = null;
                return this;
            }
        }

        public IPartialMailIII WithSubject(string subject)
        {
            subject.If("subject").IsNull.IsEmptyOrWhitespace.ThenThrow();
            _mail.Subject = subject;
            return this;
        }

        public IPartialMailIII WithoutSubject
        {
            get
            {
                _mail.Subject = String.Empty;
                return this;
            }
        }

        public IPartialMailIIII AndTemplate<T>(string templateFileName, T templateData)
        {
            _templateService.If("_templateService").IsNull.ThenThrow();
            var body = _templateService.Parse(File.ReadAllText(Path.Combine(_mail.Settings.TemplateDirectory, templateFileName)), templateData, null, templateFileName);
            _mail.Body = body;
            return this;
        }

        public IPartialMailIIII AndBody(string body)
        {
            body.If("body").IsNull.IsEmptyOrWhitespace.ThenThrow();
            _mail.Body = body;
            return this;
        }

        public IPartialMailIIII AddAttachments(params Attachment[] attachments)
        {
            attachments.If("attachments").IsNull.IsEmpty.ThenThrow();

            foreach (var attachment in attachments)
            {
                _mail.Attachments.Add(attachment);
            }

            return this;
        }

        public IPartialMailIIII AddMeeting(Meeting meeting)
        {
            meeting.If("meeting").IsNull.ThenThrow();

            _mail.Meeting = meeting;
            return this;
        }

        public IPartialMailIIII ReplyTo(Contact contact)
        {
            contact.If("contact").IsInvalid.ThenThrow();
            _mail.ReplyTo = contact;
            return this;
        }

        public IMailReadyToSend To(params Contact[] recipients)
        {
            recipients.If("recipients").IsNull.IsEmpty.ThenThrow();
            _mail.To = recipients;
            return this;
        }

        public TypeLessMail GetMessage()
        {
            return _mail;
        }

        public IPartialMailI Reset(bool keepSettings = true)
        {
            if (keepSettings)
            {
                var settings = _mail.Settings;
                _mail = new TypeLessMail() { Settings = settings };
                return this;
            }
            else
            {
                _mail = new TypeLessMail();
                return this;
            }
        }

        private MailMessage PrepareMailMessage()
        {

            Contact Organizer = null;
            MailMessage message = new MailMessage();


            if (_mail.From != null)
            {
                message.From = new MailAddress(_mail.From.MailAddress, _mail.From.Name);
                Organizer = _mail.From;
            }
            else
            {
                message.From = new MailAddress(_mail.Settings.SMTPUserEmail);
                Organizer = new Contact(message.From.Address, message.From.DisplayName);
            }

            if (_mail.ReplyTo != null)
            {
                message.ReplyToList.Add(new MailAddress(_mail.ReplyTo.MailAddress, _mail.ReplyTo.Name));
                Organizer = _mail.ReplyTo;
            }

            foreach (var contact in _mail.To)
            {
                if (contact.Type == ContactType.To || contact.Type == ContactType.Required)
                {
                    message.To.Add(new MailAddress(contact.MailAddress, contact.Name));
                }
                if (contact.Type == ContactType.Cc || contact.Type == ContactType.Optional)
                {
                    message.CC.Add(new MailAddress(contact.MailAddress, contact.Name));
                }
                if (contact.Type == ContactType.Bcc || contact.Type == ContactType.Resource)
                {
                    message.Bcc.Add(new MailAddress(contact.MailAddress, contact.Name));
                }
            }

            if (_mail.Settings.SubjectEncoding != null)
            {
                message.SubjectEncoding = _mail.Settings.SubjectEncoding;
            }
            else
            {
                message.SubjectEncoding = System.Text.Encoding.UTF8;
            }

            if (_mail.Settings.BodyEncoding != null)
            {
                message.BodyEncoding = _mail.Settings.BodyEncoding;
            }
            else
            {
                message.BodyEncoding = System.Text.Encoding.UTF8;
            }

            message.Subject = _mail.Subject;

            foreach (var attachment in _mail.Attachments)
            {
                message.Attachments.Add(new System.Net.Mail.Attachment(attachment.ContentStream, attachment.Name, attachment.MediaType));
            }

            string bodyText = _mail.Body;

            //Set up the different mime types contained in the message    
            System.Net.Mime.ContentType textType = new System.Net.Mime.ContentType(MediaTypeNames.Text.Plain);
            System.Net.Mime.ContentType htmlType = new System.Net.Mime.ContentType(MediaTypeNames.Text.Html);

            if (_mail.Settings.CharSet != null)
            {
                textType.CharSet = _mail.Settings.CharSet;
                htmlType.CharSet = _mail.Settings.CharSet;
            }
            else
            {
                textType.CharSet = "UTF-8";
                htmlType.CharSet = "UTF-8";
            }

            var plainText = ConvertToPlainText(_mail.Body);
            if (plainText != null)
            {
                var textView = AlternateView.CreateAlternateViewFromString(plainText, textType);
                message.AlternateViews.Add(textView);
            }

            var htmlView = AlternateView.CreateAlternateViewFromString(_mail.Body, htmlType);
            message.AlternateViews.Add(htmlView);

            if (_mail.Meeting != null)
            {
                _mail.Meeting.Summary = _mail.Subject;
                _mail.Meeting.Description = bodyText;
                message.Headers.Add("Content-class", "urn:content-classes:calendarmessage");
                System.Net.Mime.ContentType calendarType = new System.Net.Mime.ContentType("text/calendar");
                calendarType.CharSet = htmlType.CharSet;
                //  Add parameters to the calendar header    
                calendarType.Parameters.Add("method", "REQUEST");
                calendarType.Parameters.Add("name", "meeting.ics");

                AlternateView calendarView = AlternateView.CreateAlternateViewFromString(_mail.Meeting.Generate(_mail.To, Organizer), calendarType);
                calendarView.TransferEncoding = TransferEncoding.Base64;
                message.AlternateViews.Add(calendarView);
            }

            return message;

        }

        private SmtpClient PrepareSmtpClient()
        {
            var host = _mail.Settings.SMTPServer;
            int port = _mail.Settings.SMTPort;

            if (host.IndexOf(":") > -1)
            {
                host = host.Split(':')[0];
                port = Convert.ToInt32(host.Split(':')[1]);
            }

            SmtpClient mailClient = new SmtpClient(host, port);

            mailClient.DeliveryMethod = _mail.Settings.DeliveryMethod;

            if (_mail.Settings.SMTPAuthentication)
            {
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = new NetworkCredential(_mail.Settings.SMTPUsername, _mail.Settings.SMTPPassword);
            }

            mailClient.EnableSsl = _mail.Settings.SMTPEnableSSL;
            if (_mail.Settings.SMTPEnableSSL)
            {
                mailClient.Port = _mail.Settings.SMTPSSLPort;
            }
            return mailClient;
        }

        public async Task<SendMailResult> SendAsync()
        {
            var result = new SendMailResult();

            try
            {

                using (var message = PrepareMailMessage())
                {
                    using (SmtpClient mailClient = PrepareSmtpClient())
                    {
                        await mailClient.SendMailAsync(message);
                    }
                    result.State = SendMailState.Ok;
                }

            }
            catch (FormatException ex)
            {
                result.Exception = ex;
                result.State = SendMailState.FormatError;
            }
            catch (SmtpException ex)
            {
                result.Exception = ex;
                result.State = SendMailState.SmtpError;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.State = SendMailState.Error;
            }


            return result;

        }

        public SendMailResult Send()
        {
            var result = new SendMailResult();

            try
            {

                using (var message = PrepareMailMessage())
                {
                    using (SmtpClient mailClient = PrepareSmtpClient())
                    {
                        mailClient.Send(message);
                    }
                    result.State = SendMailState.Ok;
                }

            }
            catch (FormatException ex)
            {
                result.Exception = ex;
                result.State = SendMailState.FormatError;
            }
            catch (SmtpException ex)
            {
                result.Exception = ex;
                result.State = SendMailState.SmtpError;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.State = SendMailState.Error;
            }


            return result;

        }

        private string ConvertToPlainText(string html)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                StringWriter sw = new StringWriter();
                ConvertTo(doc.DocumentNode, sw);
                sw.Flush();
                return sw.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public void ConvertTo(HtmlNode node, TextWriter outText)
        {
            string html;
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    // don't output comments
                    break;

                case HtmlNodeType.Document:
                    ConvertContentTo(node, outText);
                    break;

                case HtmlNodeType.Text:
                    // script and style must not be output
                    string parentName = node.ParentNode.Name;
                    if ((parentName == "script") || (parentName == "style"))
                        break;

                    // get text
                    html = ((HtmlTextNode)node).Text;

                    // is it in fact a special closing node output as text?
                    if (HtmlNode.IsOverlappedClosingElement(html))
                        break;

                    // check the text is meaningful and not a bunch of whitespaces
                    if (html.Trim().Length > 0)
                    {
                        outText.Write(HtmlEntity.DeEntitize(html));
                    }
                    break;

                case HtmlNodeType.Element:
                    switch (node.Name)
                    {
                        case "p":
                            // treat paragraphs as crlf
                            outText.Write("\r\n");
                            break;
                    }

                    if (node.HasChildNodes)
                    {
                        ConvertContentTo(node, outText);
                    }
                    break;
            }
        }

        private void ConvertContentTo(HtmlNode node, TextWriter outText)
        {
            foreach (HtmlNode subnode in node.ChildNodes)
            {
                ConvertTo(subnode, outText);
            }
        }

    }

}
