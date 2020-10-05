using System;//
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.CommonHelper
{
    public static class Email
    {
        public static void SendMail(string bodymessage, string mailaddress, string subject)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                System.Net.Mail.SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("hosteller@hosteller-001-site1.ftempurl1.com");
                message.To.Add(new MailAddress(mailaddress));
                message.Subject = subject;
               // message.IsBodyHtml = true; //to make message body as html  
                message.Body = bodymessage;
                smtp.Port = 8889;
                smtp.Host = "mail5011.smarterasp.net"; //for smpt host  
                smtp.EnableSsl = false;
                //smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("hosteller@hosteller-001-site1.ftempurl1.com", "hosteller@123");
               // smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex) { }
        }
    }
}
