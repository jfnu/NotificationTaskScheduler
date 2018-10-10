using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NotificationTaskScheduler.DbModels
{
    [Table("ObjectChargeback")]
    public class ObjectChargeback
    {
        public int Id { get; set; }
        public Guid SubscriptionId { get; set; }
        public string ObjectName { get; set; }
        public string ObjectType { get; set; }
        public string Owner { get; set; }
        public int? DbLobId { get; set; }
    }
}
