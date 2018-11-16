using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Mail.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                // RazorEngine cannot clean up from the default appdomain...
                Console.WriteLine("Switching to second AppDomain, for RazorEngine...");
                AppDomainSetup adSetup = new AppDomainSetup();
                adSetup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                var current = AppDomain.CurrentDomain;
                // You only need to add strongnames when your appdomain is not a full trust environment.
                var strongNames = new StrongName[0];

                var domain = AppDomain.CreateDomain(
                    "ConsoleTest", null,
                    current.SetupInformation, new PermissionSet(PermissionState.Unrestricted),
                    strongNames);

                var exitCode = domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location);
                
                // RazorEngine will cleanup. 
                AppDomain.Unload(domain);
            }

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
                    .WithHeader("test", "sdfsdf")
                    .WithSubject("Some subject")
                    .AndTemplate("SampleTemplate.cshtml", new { Name = "Test User" })
                    //.AddMeeting(new Meeting(DateTime.Now.AddHours(1), DateTime.Now.AddHours(3))
                    //{
                    //    Description = "Do you know what the next step should be for TypeLess.Mail?",
                    //    Location = "The usual place",
                    //    Summary = "We need to go through some new features"
                    //})
                    .To(new Contact("tobias.jansater@rapidsolutions.se", ContactType.To))
                    .SendAsync();
        }
    }
}
