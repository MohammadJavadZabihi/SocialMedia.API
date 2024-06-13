using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Senders
{
    public class SendEmailServies
    {
        public void SendEmail(string userEmail, string activationLink)
        {
            var fromAddress = new MailAddress("toxix.dota98@gmail.com", "mahan");
            var toAddress = new MailAddress(userEmail);
            const string fromPassword = "mahan.z.road0908";
            const string subject = "Activate Your Account";
            string body = $"Please click the following link to activate your account: {activationLink}";

            var smtp = new SmtpClient
            {
                Host = "smtp.example.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
    }
}
