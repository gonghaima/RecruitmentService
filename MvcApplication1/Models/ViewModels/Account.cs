using MvcApplication1.Models.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace MvcApplication1.Models.ViewModels
{
    public static class Account
    {
        static dataEntities dbContext = new dataEntities();
        public static string[] retrievePassword(string UserName)
        {
            Dictionary<string, string> dicResult= new Dictionary<string,string>();
            string strU="";
            string strP="";
            if(dbContext.Users.Any(c=>c.UserName.Contains(UserName)))
            {
                strU=UserName;
                strP=dbContext.Users.FirstOrDefault(c=>c.UserName==UserName).Password;
                sendPasswordToUser(UserName);
            }
            string[] result2Return= new string[2];
            result2Return[0]=strU;
            result2Return[1]=strP;

            return result2Return;
        }

        private static void sendPasswordToUser(String UserName)
        {
            Email email = new Email();
            string mailTo = dbContext.Users.FirstOrDefault(c=>c.UserName==UserName).Email;
            string mailSub = "Please keep your password in a safe place";
            string mailBody = "Your password is     " + dbContext.Users.FirstOrDefault(c => c.UserName == UserName).Password;
            email.sendMail(mailTo, mailSub, mailBody);
        }
    }

    public class PasswordRetrieval
    {
        [Required]
        public string UserName { get; set; }
    }
}