using NotificationTaskScheduler.Models;

namespace NotificationTaskScheduler.Mail
{
    public interface IMailServer
    {
        void Send(NotificationMailMessage mailMessage);
    }
}
