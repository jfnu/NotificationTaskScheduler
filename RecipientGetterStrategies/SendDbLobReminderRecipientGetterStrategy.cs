using System.Collections.Generic;
using System.Linq;
using NotificationTaskScheduler.DbModels;
using NotificationTaskScheduler.Models;
using NotificationTaskScheduler.Services;

namespace NotificationTaskScheduler.RecipientGetterStrategies
{
    public class SendDbLobReminderRecipientGetterStrategy : IRecipientGetterStrategy
    {
        private readonly NotificationDbContext _notificationDbContext;
        private readonly ILdapService _ldapService;

        public SendDbLobReminderRecipientGetterStrategy(
            NotificationDbContext notificationDbContext, 
            ILdapService ldapService)
        {
            _notificationDbContext = notificationDbContext;
            _ldapService = ldapService;
        }
       
        public ICollection<Recipient> Get()
        {
            //need to work on getting record from DB - DONE
            //need to work on getting the managers from ldap
            //need to get first send email date - DONE
            var recipients = GetRecipientsFromDbWithoutManagerInfo();
            foreach (var recipient in recipients)
            {
                var msId = recipient.AzurePackId.Replace("@domain.com", "").Trim();  
                var userInfo = _ldapService.GetUserInfoAndManagers(msId);
                recipient.OneLevelUpManager = new RecipientManager
                {
                    ManagerEmail = userInfo.OneLevelUpManagerEmail,
                    ManagerName = userInfo.OneLevelUpManagerName
                };
                recipient.TwoLevelUpManager = new RecipientManager
                {
                    ManagerEmail = userInfo.TwoLevelUpManagerEmail,
                    ManagerName = userInfo.TwoLevelUpManagerName
                };
                recipient.HasIssue = userInfo.HasIssue;
                recipient.IssueDescription = userInfo.IssueDescription;
                recipient.Name = userInfo.UserName;
                recipient.OwnerEmail = userInfo.UserEmail;
            }
            return recipients;
        }

        private List<Recipient> GetRecipientsFromDbWithoutManagerInfo()
        {
            var dbRecords =
                (from objcb in _notificationDbContext.ObjectChargebacks
                    join lobTrack in _notificationDbContext.LobEmailTrackings on objcb.Owner equals lobTrack.Owner into
                        tmp
                    from lobTrack in tmp.DefaultIfEmpty()
                    where (objcb.DbLobId == null) && objcb.ObjectType.Equals("Database")
                    select new
                    {
                        objcb.Owner,
                        FirstSendDate = lobTrack == null ? null : lobTrack.FirstSendDate,
                        SubscriptionId = objcb.SubscriptionId.ToString(),
                        DbName = objcb.ObjectName
                    }).ToList();

            var uniqueOwners = _notificationDbContext.ObjectChargebacks
                .Where(x=> (x.DbLobId == null) && x.ObjectType.Equals("Database"))
                .GroupBy(x => x.Owner)
                .Select(g => g.Key)
                .ToList();

            var recipients = new List<Recipient>();
            foreach (var owner in uniqueOwners)
            {
                var recipient = new Recipient
                {
                    AzurePackId = owner,
                    FirstSendDate = dbRecords.First(x => x.Owner.Equals(owner)).FirstSendDate,
                    Assets = dbRecords.Where(x => x.Owner.Equals(owner)).Select(x => new RecipientAsset
                    {
                        AssetName = x.DbName,
                        SubscriptionId = x.SubscriptionId
                    }).ToList()
                };
                recipients.Add(recipient);
            }

            return recipients;
        }
    }
}
