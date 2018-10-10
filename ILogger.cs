using System.Collections.Generic;
using System.Text;

namespace NotificationTaskScheduler
{
    public interface ILogger
    {
        void Write(string message);
    }
}
