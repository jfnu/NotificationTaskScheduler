using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationTaskScheduler.DbModels
{
    [Table("LobEmailTracking")]
    public class LobEmailTracking
    {
        [Key]
        public string  Owner { get; set; }
        public DateTime? FirstSendDate { get; set; }
    }
}