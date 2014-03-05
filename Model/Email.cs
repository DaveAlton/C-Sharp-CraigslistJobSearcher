using CraigsListSearcher.Hidden;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace CraigsListSearcher.Models
{
    public class Email
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string CoverLetter { get; set; }
        public string Resume { get; set; }

        public Email(string recipient, string subject)
        {
            Recipient = recipient;
            Subject = subject;
            CoverLetter = "Cover Letter.pdf";
            Resume = "Resume.pdf";
        }
        public Email() { }
        public void Send()
        {
            Attachment coverLetter = new Attachment(this.CoverLetter, MediaTypeNames.Application.Octet);
            try
            {
                if (this.Recipient == "")
                {
                    Console.WriteLine("Employer did not specify email.");
                    coverLetter.Dispose();
                }
                else
                {
                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                    //THE FOLLOWING LINE IS COMMENTED OUT SO THE EMPLOYER IS NEVER ACTUALLY CONTACTED.
                    //message.To.Add(this.Recipient);
                    message.Bcc.Add("alton.dave@gmail.com");
                    message.Subject = this.Subject;
                    message.From = new System.Net.Mail.MailAddress("alton.dave@gmail.com");
                    message.Body = "";
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential("alton.dave@gmail.com", HiddenVariables.password);
                    Attachment resume = new Attachment(this.Resume, MediaTypeNames.Application.Octet);
                    message.Attachments.Add(coverLetter);
                    message.Attachments.Add(resume);
                    smtp.Send(message);
                    coverLetter.Dispose();
                    Console.WriteLine("Email Sent");
                }
            } catch(Exception e){
                coverLetter.Dispose();
                Console.WriteLine("Email Error");
                Console.WriteLine(e.Message);
            }
        }
    }

}