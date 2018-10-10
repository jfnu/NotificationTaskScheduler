using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NotificationTaskScheduler.Models
{
    public class Recipient
    {
        public string AzurePackId { get; set; }
        public string OwnerEmail { get; set; }
        public bool HasIssue { get; set; } = false;
        public string IssueDescription { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime? FirstSendDate { get; set; } = null;

        public RecipientManager OneLevelUpManager { get; set; }
        public RecipientManager TwoLevelUpManager { get; set; }
        public ICollection<RecipientAsset> Assets { get; set; } = new List<RecipientAsset>();
    }

    public class RecipientAsset
    {
       public string AssetName { get; set; }
       public string SubscriptionId { get; set; }
    }

    public class RecipientManager
    {
        public string ManagerEmail { get; set; }
        public string ManagerName  { get; set; }
        public DateTime DateToNotify { get; set; }
    }
}
