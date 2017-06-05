using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dubno.Models
{
    public class MessageServices
    {
        public IConfiguration Configuration { get; set; }
        public MessageServices()
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        public async Task SendEmailAsync(string userName, string email, string subject, string message, ICollection<IFormFile> attachments)
        {
            try
            {
                var _email = "humansofdubno@hotmail.com";
                var _epass = Configuration["AppSettings:EmailPassword"];
                var _dispName = "Humans of Dubno";
                var myMessage = new MimeMessage();
                var builder = new BodyBuilder();
                if (attachments != null && attachments.Count > 0)
                {
                    foreach (var attachment in attachments)
                    {
                        builder.Attachments.Add(attachment.FileName);
                    }
                }
                myMessage.To.Add(new MailboxAddress(userName, email));
                myMessage.From.Add(new MailboxAddress(_dispName, _email));
                myMessage.Subject = subject;
                builder.HtmlBody = message;
                myMessage.Body = builder.ToMessageBody();

                //using (SmtpClient smtp = new SmtpClient())
                //{
                //    bool UseSSL = false;
                //    string Host = "smtp.live.com";
                //    int Port = 587;
                //    await smtp.ConnectAsync(Host, Port, UseSSL).ConfigureAwait(false);
                //    smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                //    smtp.Authenticate(_email, _epass); // Note: only needed if the SMTP server requires authentication
                //    await smtp.SendAsync(myMessage).ConfigureAwait(false);
                //    await smtp.DisconnectAsync(true).ConfigureAwait(false);
                //}

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtp.PickupDirectoryLocation = "C:\\Mails\\";
                    await smtp.SendMailAsync(myMessage);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendBulkEmailAsync(string userName, string[] emails, string subject, string message, ICollection<IFormFile> attachments)
        {
            try
            {
                var _email = "yourEmail";
                var _epass = Configuration["AppSettings:EmailPassword"];
                var _dispName = "DisplayName";
                var myMessage = new MimeMessage();
                var builder = new BodyBuilder();
                if (attachments != null && attachments.Count > 0)
                {
                    foreach (var attachment in attachments)
                    {
                        builder.Attachments.Add(attachment.FileName);
                    }
                }
                foreach (var email in emails)
                {
                    myMessage.To.Add(new MailboxAddress(userName, email));
                }
                myMessage.From.Add(new MailboxAddress(_dispName, _email));
                myMessage.Subject = subject;
                builder.HtmlBody = message;
                myMessage.Body = builder.ToMessageBody();

                using (SmtpClient smtp = new SmtpClient())
                {
                    bool UseSSL = false;
                    string Host = "smtp.live.com";
                    int Port = 587;
                    await smtp.ConnectAsync(Host, Port, UseSSL).ConfigureAwait(false);
                    smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                    smtp.Authenticate(_email, _epass); // Note: only needed if the SMTP server requires authentication
                    await smtp.SendAsync(myMessage).ConfigureAwait(false);
                    await smtp.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}