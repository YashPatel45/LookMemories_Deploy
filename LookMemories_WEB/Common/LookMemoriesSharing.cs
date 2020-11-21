using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LookMemories_WEB.Common
{
    public static class LookMemoriesSharing
    {
        public static bool SendEmail(string subject, string body, string to, string from, string password)
        {
            try
            {
                string fromEmail = "lookmemories@gmail.com";
                var message = new MailMessage
                {
                    From = new MailAddress(fromEmail),

                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                    DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                };
                message.To.Add(new MailAddress(to));
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))

                {
                    smtpClient.Credentials = new NetworkCredential("lookmemories@gmail.com", "Look@123");

                    smtpClient.EnableSsl = true;
                    smtpClient.Send(message);
                    return true;
                }
            }
            catch (Exception excep)
            {
                return false;
            }
        }
    }
}
