using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace NotificationTaskScheduler.DbModels
{
    public class NotificationDbContext:DbContext
    {
        public NotificationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ObjectChargeback> ObjectChargebacks { get; set; }
        public DbSet<LobEmailTracking> LobEmailTrackings { get; set; }
    }
}
