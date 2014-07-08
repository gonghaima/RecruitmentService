using MvcApplication1.Models.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Security;
namespace MvcApplication1.Models.ViewModels
{
    public class Account:IRegisterInterface, IUserInterface
    {
        static dataEntities dbContext = new dataEntities();

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        int Id { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        //[Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[Required]
        [Display(Name = "Confirm Password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        public Role role{ get; set; }
        User user = new User();

        public bool register(string roleName)
        {
            user.UserName = UserName;
            user.Password = Password;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Email = Email;

            User tempUser = dbContext.Users.FirstOrDefault(c => c.UserName == user.UserName);

            if (tempUser != null)
            {
                //already a user in database
                return false;
            }
            else
            {
                dbContext.Users.Add(user);
                Role r = dbContext.Roles.SingleOrDefault(c => c.Name == roleName);
                dbContext.SaveChanges();
                dbContext.Users.SingleOrDefault(c => c.UserName == user.UserName).Roles.Add(r);
                dbContext.SaveChanges();
                return true;
            }
        }

        public bool login()
        {
            User ur = dbContext.Users.SingleOrDefault(c => c.UserName == UserName);
            if (ur != null && ur.Password == Password)
            {
                FormsAuthentication.SetAuthCookie(UserName, false);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void logout()
        {
            FormsAuthentication.SignOut();
        }

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