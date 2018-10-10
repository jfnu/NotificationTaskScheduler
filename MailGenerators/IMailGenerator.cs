using System;
using System.Collections.Generic;
using NotificationTaskScheduler.Models;

namespace NotificationTaskScheduler.MailGenerators
{
    public interface IMailGenerator
    {
        NotificationMailMessage Generate(Sender sender, Recipient recipient);
    }
}
