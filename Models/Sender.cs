using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationTaskScheduler.Models
{
    public class Sender
    {
        public string FromEmail { get; set; } = "from@domain.com";
        public string FromDisplayName { get; set; } = "from display name";
        public string Bcc1Email { get; set; } = "bcc@domain.com";
        public string Bcc2Email { get; set; } = "bcc1@domain.com";

    }
}
