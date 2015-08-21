using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace MvcApplication1.Models.ViewModels
{
    public class Email
    {
        private string _from = "RayTonyIunisiwhitireia@gmail.com";
        private string _user = "RayTonyIunisiwhitireia@gmail.com";
        private string _password = "Passw0rd123";
        private string _host = "smtp.gmail.com";
        private int _port = 587;
        private bool _enableSsl = true;

        public string From
        {
            set
            {
                _from = value;
            }
            get
            {
                return _from;
            }
        }

        public string User
        {
            set
            {
                _user = value;
            }
            get
            {
                return _user;
            }
        }

        public string Password
        {
            set
            {
                _password = value;
            }
            get
            {
                return _password;
            }
        }

        public string Host
        {
            set
            {
                _host = value;
            }
            get
            {
                return _host;
            }
        }

        public int Port
        {
            set
            {
                _port = value;
            }
            get
            {
                return _port;
            }
        }

        public bool EnableSsl
        {
            set
            {
                _enableSsl = value;
            }
            get
            {
                return _enableSsl;
            }
        }


        public void sendMail()
        {
            sendMail("gonghaima@gmail.com", "Test Sub", "HTML code");
        }

        public void sendMail(string mailTo, string mailSub, string htmlBody)
        {
            sendMail(new List<string>() { mailTo }, mailSub, htmlBody);
        }

        public void sendMail(List<string> mailTo, string mailSub, string htmlBody)
        {
            using (SmtpClient SmtpServer = new SmtpClient
            {
                Host = _host,
                Port = _port,
                EnableSsl = _enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(_user, _password)
            })
            {

                var mail = new MailMessage();
                mail.From = new MailAddress(_from);
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
}