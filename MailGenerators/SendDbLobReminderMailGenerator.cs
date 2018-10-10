using System;
using System.Text;
using NotificationTaskScheduler.Models;

namespace NotificationTaskScheduler.MailGenerators
{
    public class SendDbLobReminderMailGenerator : IMailGenerator
    {
        private const string SubjectEmail = "ACTION REQUIRED – Assign LOB designation to databases in Azure Pack";

        public NotificationMailMessage Generate(Sender sender, Recipient recipient)
        {
            var mailMessage = new NotificationMailMessage
            {
                FromAddress = sender.FromEmail,
                FromName = sender.FromDisplayName,
                ToAddress = recipient.OwnerEmail,
                Body = GetBody(recipient),
                Subject = string.Concat(recipient.FirstSendDate!=null?"REMINDER: ":string.Empty,SubjectEmail)
            };
            if (!string.IsNullOrEmpty(recipient.OneLevelUpManager.ManagerName) &&
                recipient.OneLevelUpManager.DateToNotify <= DateTime.Today)
                mailMessage.CcAddress = recipient.OneLevelUpManager.ManagerEmail;

            if (!string.IsNullOrEmpty(recipient.TwoLevelUpManager.ManagerName) &&
                recipient.TwoLevelUpManager.DateToNotify <= DateTime.Today)
                mailMessage.CcAddress += "," + recipient.TwoLevelUpManager.ManagerEmail;

            if (!sender.Bcc1Email.Equals(string.Empty))
                mailMessage.BccAddress = sender.Bcc1Email;

            if (!sender.Bcc2Email.Equals(string.Empty))
                mailMessage.BccAddress = string.Concat(mailMessage.BccAddress, ",", sender.Bcc2Email);

            return mailMessage;

        }
        private string GetBody(Recipient recipient)
        {
            var sb = new StringBuilder();

            var tmpMsg = "ACTION REQUIRED";
            if (recipient.FirstSendDate != null)
                tmpMsg = string.Concat("REMINDER: ", tmpMsg);

            sb.Append(
                $"<span style='font-size:14.0pt;font-family:\"Calibri\"'><b>{tmpMsg} – Assign LOB designation to databases in Azure Pack</b>.<br/><br/></span>");


            sb.Append(
                $"<span style='font-size:11.0pt;font-family:\"Calibri\"'>One or more SQL databases in Azure Pack assigned to <u>{recipient.Name}</u> do not have a Line of Business (LOB) designation.  We need the missing information updated in the <a href='https://azure.optum.com/'>Azure Pack tenant portal</a> by <b>{recipient.Deadline.ToShortDateString()}</b>.<br/><br/>");

            sb.Append(
                "This information is now required by finance and it will also allow us to automatically route your SQL support incidents submitted through Azure Pack to the correct SQL LOB support group.<br/><br/>");
            sb.Append(
                "To complete this review, please go to 'Chargeback' module in Azure Pack and Fill in LOB information for following database(s): <br/><br/>");

            sb.Append(
                "<table style='font-size:11.0pt;font-family:\"Calibri\"'><tr><td><b><u>Subscription Id</u></b></td><td><b><u>Database Name</u></b></td></tr>");

            foreach (var asset in recipient.Assets)
            {
                sb.Append($"<tr><td>{asset.SubscriptionId}&nbsp;&nbsp;&nbsp;&nbsp;</td><td>{asset.AssetName}</td></tr>");
            }

            sb.Append("</table>");
            sb.Append(
                "<br/><br/>For detailed steps, please reference the <a href='https://www.optumdeveloper.com/content/odv-optumdev/optum-developer/en/developer-centers/net-development-center/platforms/optum-azure-pack/chargeback-user-guide.html'>Chargeback guide</a>. LOB Update information is in the 'How to edit Line of Business information' section.<br/>");

            sb.Append(
                "<br/>This review will automatically inform the following managers if not completed by the dates indicated below:<br/><br/>");

            sb.Append(
                "<table style='font-size:11.0pt;font-family:\"Calibri\"'><tr><td><b><u>Notification Date</u></b>&nbsp;&nbsp;&nbsp;&nbsp;</td><td><b><u>Notification Manager</u></b></td></tr>");
            sb.Append($"<tr><td>{recipient.OneLevelUpManager.DateToNotify.ToShortDateString()}</td><td>{recipient.OneLevelUpManager.ManagerName}</td></tr>");
            sb.Append($"<tr><td>{recipient.TwoLevelUpManager.DateToNotify.ToShortDateString()}</td><td>{recipient.TwoLevelUpManager.ManagerName}</td></tr>");
            sb.Append("</table>");
         
            sb.Append(
                "<br/><br/><b>Frequently Asked Questions:</b></br><ul>");
            sb.Append(
                "<li><b>Q: How do I access chargeback module in Azure Pack?</b><br/>A: Please use this <a href='https://www.optumdeveloper.com/content/odv-optumdev/optum-developer/en/developer-centers/net-development-center/platforms/optum-azure-pack/chargeback-user-guide.html'>guide</a>.</li>");
            sb.Append(
                "<li><b>Q: How I can remove databases I no longer need?</b><br/>A: You can self-service delete development and test environment databases by selecting the database on the 'SQL Server Databases' tab and selecting the 'Delete' button in the <a href='https://azure.optum.com/'>Azure Pack Tenant Portal</a>. Stage and Prod database drop can be requested through <a href='https://servicecatalog.uhc.com/sc/catalog.product.aspx?product_id=microsoft_sql_server_database_management'>SQL Server Database Management</a> form by selecting 'Additional Services' under 'Manual Requests' and using the 'Capacity/Space/Filesystems' Service Catagory. </li>");
            sb.Append(
                "<li><b>Q: How do I transfer ownership of a database to a new owner?</b><br/>A: Please use this <a href='https://www.optumdeveloper.com/content/odv-optumdev/optum-developer/en/developer-centers/net-development-center/platforms/optum-azure-pack/how-to-engage-azure-pack-operations.html'>guide</a> to open a service ticket with Azure Pack operations team for database ownership transfers. You must identify a new owner when contacting operations for an object ownership transfer.</li>");
            sb.Append(
                "<li><b>Q: How do I get assistance if I am having access issues with the 'Chargeback' module, with my databases not showing up, or need additional help navigating to the 'Edit LOB' screen?</b><br/>A: Please use this <a href='https://www.optumdeveloper.com/content/odv-optumdev/optum-developer/en/developer-centers/net-development-center/platforms/optum-azure-pack/how-to-engage-azure-pack-operations.html'>guide</a> to open a service ticket with the Azure Pack operations team for additional assistance.</li>");
            sb.Append(
                "<li><b>Q: I do not know if this database is being used any longer. How can I have a DBA report usage or the security configuration to me?</b><br/>A: Please use this <a href='https://www.optumdeveloper.com/content/odv-optumdev/optum-developer/en/developer-centers/net-development-center/platforms/optum-azure-pack/service-now-integration.html'>guide</a> to open a service ticket with your Line of Business DBA team to investigate database usage or for any other database level questions or issues.</li>");
            sb.Append(
                "<li><b>Q: Why the change to LOB model for SQL Server?</b><br/>A: Read the <a href='http://goto/sqllob'>news article</a> on Optum Developer.</li>");
            sb.Append(
                "<li><b>Q: I’ve completed the update or removed the databases. Is there anything further I need to do?</b><br/>A: No – you will no longer receive notifications once the database is updated or removed.</li></ul><br/>");


           
            sb.Append("<br/>Thank you,<br />");
            sb.Append("Windows   Azure Pack Team <br /><br /></span>");

            return sb.ToString();
        }
    }
}