using NotificationTaskScheduler.Models;

namespace NotificationTaskScheduler.Services
{
    public interface ILdapService
    {
        UserInfo GetUserInfoAndManagers(string msId);
    }
}