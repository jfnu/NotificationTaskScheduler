using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using NotificationTaskScheduler.Mail;
using NotificationTaskScheduler.NotificationTasks;

namespace NotificationTaskScheduler
{
    public class TaskRunner
    {
        private readonly ILogger _logger;

        public TaskRunner(ILogger logger)
        {
            _logger = logger;
        }
        public void Run(NotificationType notificationType)
        {
            try
            {
                _logger.Write("Process running notification task is starting.....");
                switch (notificationType)
                {
                    case NotificationType.SendDbLobReminder:
                        var task = Program.ContainerServiceProvider.GetService<INotificationTask>();
                        task.Execute();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(notificationType), notificationType, null);
                }
                _logger.Write("Process running notification task is done.....");
            }
            catch (Exception ex)
            {
                _logger.Write($"Exception Thrown: {ex.Message}");
                throw;
            }
            if(_logger is IDisposable logger) logger.Dispose();
        }
    }
}
