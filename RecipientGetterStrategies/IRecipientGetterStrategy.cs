using System.Collections.Generic;
using NotificationTaskScheduler.Models;

namespace NotificationTaskScheduler.RecipientGetterStrategies
{
    public interface IRecipientGetterStrategy
    {
        ICollection<Recipient> Get();
    }
}
