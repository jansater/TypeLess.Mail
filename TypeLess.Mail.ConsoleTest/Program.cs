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
                    .Configure.SMTPServer("localhost").Done
                    .From("tobias.jansater@rapidsolutions.se", "Tobias Jansäter")
                    .WithSubject("Some subject")
                    .AndTemplate("SampleTemplate.cshtml", new { Name = "Test User" })
                    .AddAttachments(new Attachment(File.OpenRead(@"c:/Fresh fish 2014_3.pdf"), "Fresh fish", System.Net.Mime.MediaTypeNames.Application.Pdf))
                    .AddMeeting(new Meeting(DateTime.Now.AddHours(1), DateTime.Now.AddHours(3))
                    {
                        Description = "Do you know what the next step should be for TypeLess.Mail?",
                        Location = "The usual place",
                        Summary = "We need to go through some new features"
                    })
                    .To(new Contact("tobias.jansater@symbio.com", ContactType.To))
                    .Send();
        }
    }
}
