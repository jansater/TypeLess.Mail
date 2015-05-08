using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Mail.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {

            SendTestMail();


            Console.ReadLine();

        }

        public static async Task<SendMailResult> SendTestMail() {
            return await MailBuilder.Create(new TemplateService())
                    .Configure.SMTPServer("smtp.sendgrid.net")
                    .RequiresSMTPAuthentication(true, "tobias.jansater", "Ramsan2005")
                    .EnableSSL(true, 587)
                    .SmtpPort(587)
                    .Done
                    .From("tobias.jansater@symbio.com", "Tobias Jansäter")
                    .WithSubject("Some subject")
                    .AndTemplate("SampleTemplate.cshtml", new { Name = "Test User" })
                    .AddMeeting(new Meeting(DateTime.Now.AddHours(1), DateTime.Now.AddHours(3))
                    {
                        Description = "Do you know what the next step should be for TypeLess.Mail?",
                        Location = "The usual place",
                        Summary = "We need to go through some new features"
                    })
                    .To(new Contact("tobias.jansater@rapidsolutions.se", ContactType.To))
                    .SendAsync();
        }
    }
}
