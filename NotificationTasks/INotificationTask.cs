using System.Collections.Generic;
using System.Text;

namespace NotificationTaskScheduler.NotificationTasks
{
    public interface INotificationTask
    {
        void Execute();
    }
}
