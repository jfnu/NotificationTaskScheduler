using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NotificationTaskScheduler.DbModels;
using NotificationTaskScheduler.Mail;
using NotificationTaskScheduler.MailGenerators;
using NotificationTaskScheduler.Models;
using NotificationTaskScheduler.RecipientGetterStrategies;

namespace NotificationTaskScheduler.NotificationTasks
{
    public class SendDbLobReminderNotificationTask : INotificationTask
    {
        private readonly IConfiguration _configuration;
        private readonly IMailServer _mailServer;
        private readonly IRecipientGetterStrategy _getStrategy;
        private readonly IMailGenerator _mailGenerator;
        private readonly ILogger _logger;
        private readonly Sender _sender;
        private readonly NotificationDbContext _notificationDbContext;

        public SendDbLobReminderNotificationTask(
            IConfiguration configuration, 
            IMailServer mailServer,
            IRecipientGetterStrategy getStrategy,
            IMailGenerator mailGenerator,
            ILogger logger,
            Sender sender,
            NotificationDbContext notificationDbContext)
        {
            _configuration = configuration;
            _mailServer = mailServer;
            _getStrategy = getStrategy;
            _mailGenerator = mailGenerator;
            _logger = logger;
            _sender = sender;
            _notificationDbContext = notificationDbContext;
        }

        public void Execute()
        {
            var recipients = _getStrategy.Get();
            foreach (var recipient in recipients)
            {
                var manager1SendAfterDays = Convert.ToInt16(_configuration["AppSettings:Manager1Days"]);
                var manager2SendAfterDays = Convert.ToInt16(_configuration["AppSettings:Manager2Days"]);
                var lastSendDate = DateTime.Today;
                if (recipient.FirstSendDate != null && !recipient.FirstSendDate.Equals(DateTime.MinValue))
                    lastSendDate = Convert.ToDateTime(recipient.FirstSendDate);

                recipient.OneLevelUpManager.DateToNotify =
                        lastSendDate.Date.Add(new TimeSpan(manager1SendAfterDays, 0, 0, 0));
                recipient.TwoLevelUpManager.DateToNotify =
                    lastSendDate.Date.Add(new TimeSpan(manager2SendAfterDays, 0, 0, 0));

                recipient.Deadline = Convert.ToDateTime(_configuration["AppSettings:Deadline"]);
                
                if (!recipient.HasIssue)
                {
                    var mailMessage = _mailGenerator.Generate(_sender, recipient);
                    _mailServer.Send(mailMessage);
                    SaveToTrackingTableFirstSendDate(recipient);
                }
                else
                {
                    _logger.Write($"Unable to send email to recipient: {recipient.Name} - {recipient.IssueDescription}");
                }
            }
        }

        private void SaveToTrackingTableFirstSendDate(Models.Recipient recipient)
        {
            var record = _notificationDbContext.LobEmailTrackings.FirstOrDefault(x => x.Owner == recipient.AzurePackId);

            if (record == null)
            {
                _notificationDbContext.LobEmailTrackings.Add(new LobEmailTracking
                {
                    Owner = recipient.AzurePackId,
                    FirstSendDate = DateTime.Today
                });
                _notificationDbContext.SaveChanges();
            }
        }
    }
}