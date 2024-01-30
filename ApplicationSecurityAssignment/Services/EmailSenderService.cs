using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace ApplicationSecurityAssignment.Services
{
    public class EmailSenderService
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "plcehlder2@gmail.com";
            var password = "anfx bbtb hjeb wsjo";

            var client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            return client.SendMailAsync(from: mail, recipients: email, subject, message);
        }
    }
}
