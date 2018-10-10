namespace NotificationTaskScheduler.Models
{
    public class UserInfo
    {
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public bool HasIssue { get; set; } = false;
        public string IssueDescription { get; set; }
        public string OneLevelUpManagerName { get; set; }
        public string OneLevelUpManagerEmail { get; set; }
        public string TwoLevelUpManagerName { get; set; }
        public string TwoLevelUpManagerEmail { get; set; }
    }
}