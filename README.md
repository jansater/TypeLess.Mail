TypeLess.Mail
=============

##A fluent async send mail lib based on Razor templates##

**Sample usage**
``` c#
public static async Task<SendMailResult> SendTestMail() {
            return await MailBuilder.Create(new TemplateService())
                    .Configure.SMTPServer("localhost").Done
                    .From("someuser@somedomain.se", "Sender name")
                    .WithSubject("Some subject")
                    .AndTemplate("SampleTemplate.cshtml", new { Name = "Test User" })
                    .AddAttachments(new Attachment(File.OpenRead(@"c:/Somefile.pdf"), "Some file", System.Net.Mime.MediaTypeNames.Application.Pdf))
                    .AddMeeting(new Meeting(DateTime.Now.AddHours(1), DateTime.Now.AddHours(3))
                    {
                        Description = "Do you know what the next step should be for TypeLess.Mail?",
                        Location = "The usual place",
                        Summary = "We need to go through some new features"
                    })
                    .To(new Contact("recipient@somedomain.com", ContactType.To))
                    .Send();
        }
```
