using System.Net.Mail;
using NotificationTaskScheduler.Models;

namespace NotificationTaskScheduler.Mail
{
    public class SmtpMailServer: IMailServer
    {
        public void Send(NotificationMailMessage mailMessage)
        {
            var client = new SmtpClient("mail.com") {UseDefaultCredentials = true};
            MailMessage message = new MailMessage
            {
                From = new MailAddress(mailMessage.FromAddress,mailMessage.FromName),
                To = {mailMessage.ToAddress},
                Body = mailMessage.Body,
                Subject = mailMessage.Subject,
                IsBodyHtml = true
            };
            if (!string.IsNullOrEmpty(mailMessage.CcAddress))
                message.CC.Add(mailMessage.CcAddress);

            if (!string.IsNullOrEmpty(mailMessage.BccAddress))
                message.Bcc.Add(mailMessage.BccAddress);

            client.Send(message);
        }
    }
}