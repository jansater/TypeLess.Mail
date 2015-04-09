using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Mail
{

    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentInterface
    {
        /// <summary>
        /// Redeclaration that hides the <see cref="object.GetType()"/> method from IntelliSense.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        /// <summary>
        /// Redeclaration that hides the <see cref="object.GetHashCode()"/> method from IntelliSense.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        /// <summary>
        /// Redeclaration that hides the <see cref="object.Equals(object)"/> method from IntelliSense.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
    }

    public interface IMailBuilder : IFluentInterface
    {
        IMailConfiguration Configure { get; }
        IPartialMailI WithConfiguration(EmailSettings settings);
    }

    public interface IMailConfiguration : IFluentInterface
    {
        IMailConfiguration SMTPServer(string server);
        /// <summary>
        /// Defaults to false
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IMailConfiguration RequiresSMTPAuthentication(bool enable, string username, string password);
        /// <summary>
        /// Defaults to false
        /// </summary>
        IMailConfiguration EnableSSL { get; }
        
        IMailConfiguration SmtpDefaultFromEmail(string email);
        /// <summary>
        /// Defaults to 25
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        IMailConfiguration SmtpPort(int port);

        /// <summary>
        /// Defaults to UTF-8
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        IMailConfiguration SubjectEncoding(Encoding encoding);

        /// <summary>
        /// Defaults to UTF-8
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        IMailConfiguration BodyEncoding(Encoding encoding);

        /// <summary>
        /// Defaults to UTF-8
        /// </summary>
        /// <returns></returns>
        IMailConfiguration Charset(string charset);

        /// <summary>
        /// Defaults to network
        /// </summary>
        /// <param name="deliveryMethod"></param>
        /// <returns></returns>
        IMailConfiguration SmtpDeliveryMethod(SmtpDeliveryMethod deliveryMethod);

        /// <summary>
        /// Defaults to 587
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        IMailConfiguration SmtpSSLPort(int port);

        /// <summary>
        /// Defaults to [AppDomain.CurrentDomain.BaseDirectory]/EmailTemplates
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        IMailConfiguration TemplateDirectory(string directory);

        EmailSettings GetSettings();

        IPartialMailI Done { get; }

    }

    public interface IPartialMailI : IFluentInterface
    {
        IPartialMailII From(Contact from);
        IPartialMailII From(string email, string name = null);
        IPartialMailII FromDefault { get; }
    }

    public interface IPartialMailII : IFluentInterface
    {
        IPartialMailIII WithSubject(string header);
        IPartialMailIII WithoutSubject { get; }
    }

    public interface IPartialMailIII : IFluentInterface
    {
        IPartialMailIIII AndTemplate<T>(string templateFileName, T templateData);
        IPartialMailIIII AndBody(string body);
    }

    public interface IPartialMailIIII : IFluentInterface
    {
        IPartialMailIIII AddAttachments(params Attachment[] attachments);
        IPartialMailIIII AddMeeting(Meeting meeting);
        IMailReadyToSend To(params Contact[] recipients);
        IPartialMailIIII ReplyTo(Contact contact);
    }

    public interface IMailReadyToSend : IFluentInterface
    {
        TypeLessMail GetMessage();
        Task<SendMailResult> SendAsync();
        SendMailResult Send();
        IPartialMailI Reset(bool keepSettings = true);
    } 
}
