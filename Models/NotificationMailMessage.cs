using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationTaskScheduler.Models
{
    public class NotificationMailMessage
    {
        public string FromAddress { get; set; }

        public string FromName { get; set; }

        public string ToAddress { get; set; }
        public string BccAddress { get; set; }
        public string CcAddress { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
