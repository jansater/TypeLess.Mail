using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

            var templateConfig = new TemplateServiceConfiguration();
            var templateFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "EmailTemplates");
            templateConfig.TemplateManager = new ResolvePathTemplateManager(new string[] { templateFolder });
            var templateService = RazorEngineService.Create(templateConfig);
            
            return await MailBuilder.Create(templateService)
                    .Configure.SMTPServer("localhost")
                    .Done
                    .From("tobias.jansater@test.com", "Tobias Jansäter")
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
