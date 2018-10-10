using System;
using System.DirectoryServices;
using NotificationTaskScheduler.Models;

namespace NotificationTaskScheduler.Services
{
    public class LdapService : ILdapService
    {
        public UserInfo GetUserInfoAndManagers(string msId)
        {
            DirectoryEntry dirEntry = new DirectoryEntry("LDAP://domain.com");
            var userInfo = new UserInfo();

            try
            {
                using (var search = new DirectorySearcher(dirEntry))
                {
                    search.PropertiesToLoad.Add("displayName");
                    search.PropertiesToLoad.Add("manager");
                    search.PropertiesToLoad.Add("mail");
                    search.PropertiesToLoad.Add("userAccountControl");

                    search.Filter = "(sAMAccountName=" + msId + ")";

                    //const int accountDisable = 0x0002;
                    var manager1Cn = string.Empty;
                    SearchResult result = search.FindOne();
                    if (result != null)
                    {   
                        userInfo.UserName = result.Properties["displayName"][0]?.ToString();
                        //var flags = int.Parse(result.Properties["userAccountControl"][0].ToString());
                        //userInfo.IsDisabled = Convert.ToBoolean(flags & accountDisable);
                        //if (userInfo.IsDisabled) return userInfo; //exit immediately

                        userInfo.UserEmail = result.Properties["mail"][0]?.ToString();
                        manager1Cn = result.Properties["manager"][0]?.ToString();
                    }
               
                    var manager1MsId = manager1Cn?.Split(',')[0].Replace("CN=", "");
                    search.Filter = search.Filter = "(sAMAccountName=" + manager1MsId + ")";

                    var manager2Cn = string.Empty;
                    result = search.FindOne();
                    if (result != null)
                    {
                        userInfo.OneLevelUpManagerEmail = result.Properties["mail"][0]?.ToString();
                        userInfo.OneLevelUpManagerName = result.Properties["displayName"][0]?.ToString();

                        manager2Cn = result.Properties["manager"][0]?.ToString();
                    }

                    var manager2MsId = manager2Cn?.Split(',')[0].Replace("CN=", "");
                    search.Filter = search.Filter = "(sAMAccountName=" + manager2MsId + ")";
                    result = search.FindOne();
                    if (result != null)
                    {
                        userInfo.TwoLevelUpManagerEmail = result.Properties["mail"][0]?.ToString();
                        userInfo.TwoLevelUpManagerName = result.Properties["displayName"][0]?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                userInfo.HasIssue = true;
                userInfo.IssueDescription = ex.Message;
            }

            return userInfo;
        }
    }
}
