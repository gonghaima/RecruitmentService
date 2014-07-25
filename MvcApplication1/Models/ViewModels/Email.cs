using System.Collections.Generic;
using System.Net.Mail;

namespace MvcApplication1.Models.ViewModels
{
    public class Email
    {
        //static string adminEmail = "RayTonyIunisiwhitireia@hotmail.com";
        //static string adminEmailPassword = "Passw0rd123";

        static string adminEmail = "RayTonyIunisiwhitireia@gmail.com";
        static string adminEmailPassword = "Passw0rd123";

        System.Net.NetworkCredential mailCredential = new System.Net.NetworkCredential(adminEmail, adminEmailPassword);

        //public Email(string)
        //{

        //}

        private SmtpClient createMailServer()
        {
            //SmtpClient SmtpServ = new SmtpClient("smtp.live.com");
            //SmtpServ.Port = 587;
            //SmtpServ.UseDefaultCredentials = false;
            //SmtpServ.Credentials = mailCredential;
            //SmtpServ.EnableSsl = true;
            //return SmtpServ;

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(adminEmail, adminEmailPassword)
            };
            return smtp;
        }

        public void sendMail()
        {
            SmtpClient SmtpServer = createMailServer();
            var mail = new MailMessage();
            mail.From = new MailAddress(adminEmail);
            mail.To.Add("gonghaima@gmail.com");
            mail.Subject = "Test Sub";
            mail.IsBodyHtml = true;
            string htmlBody;
            htmlBody = "HTML code";
            mail.Body = htmlBody;

            SmtpServer.Send(mail);
        }

        public void sendMail(string mailTo, string mailSub, string htmlBody)
        {
            SmtpClient SmtpServer = createMailServer();
            var mail = new MailMessage();
            mail.From = new MailAddress(adminEmail);
            mail.To.Add(mailTo);
            mail.Subject = mailSub;
            mail.IsBodyHtml = true;
            mail.Body = htmlBody;
            SmtpServer.Send(mail);
        }

        public void sendMail(List<string> mailTo, string mailSub, string htmlBody)
        {
            SmtpClient SmtpServer = createMailServer();
            var mail = new MailMessage();
            mail.From = new MailAddress(adminEmail);
            foreach (string strMail in mailTo)
            {
                mail.To.Add(strMail);
            }
            mail.Subject = mailSub;
            mail.IsBodyHtml = true;
            mail.Body = htmlBody;
            SmtpServer.Send(mail);
        }

    }
}