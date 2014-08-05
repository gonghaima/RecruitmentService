using MvcApplication1.Models.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Security;
namespace MvcApplication1.Models.ViewModels
{
    public class Account : Person, IRegisterInterface, IUserInterface
    {
        static dataEntities dbContext = new dataEntities();

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        int Id { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        public Role role { get; set; }
        User user = new User();

        public Account()
        {
        }
        public Account(string userName)
        {
            this.UserName = UserName;
        }

        public bool register(string roleName)
        {
            user.UserName = UserName;
            user.Password = Password;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Email = Email;

            Role role4Register = dbContext.Roles.SingleOrDefault(c => c.Name == roleName);
            User tempUser = dbContext.Users.FirstOrDefault(c => c.UserName == user.UserName);
            Role tempRole = new Role();
            if (tempUser != null)
            {
                tempRole = tempUser.Roles.FirstOrDefault(c => c.Name == roleName);
            }

            //already a same user with the same role
            if (tempUser != null && tempRole != null)
            {
                return false;
            }
            //already a user in database, but does not have the role.
            else if (tempUser != null && tempRole == null)
            {
                tempUser.Roles.Add(role4Register);
                dbContext.SaveChanges();
                return true;
            }
            //new user with new role
            else
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                dbContext.Users.SingleOrDefault(c => c.UserName == user.UserName).Roles.Add(role4Register);
                dbContext.SaveChanges();
                return true;
            }
        }

        public void sendRegConfirmation()
        {
            //Confirmation email of register
            Email mail = new Email();
            string mailSub = "Account Created";
            string mailBody = "Your Name: " + this.FirstName + " " + this.LastName + " User Name: " + user.UserName + " has registered as a new user.";
            mail.sendMail(this.Email, mailSub, mailBody);
        }

        public bool login()
        {
            try
            {
                User ur = dbContext.Users.SingleOrDefault(c => c.UserName == UserName);
                bool activated = false;
                if (ur.Activated != null && ur.Activated == true) { activated = true; }
                if (ur != null && activated && ur.Password == Password)
                {
                    FormsAuthentication.SetAuthCookie(UserName, false);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void logout()
        {
            FormsAuthentication.SignOut();
        }


        public static string[] retrievePassword(string UserName)
        {
            Dictionary<string, string> dicResult = new Dictionary<string, string>();
            string strU = "";
            string strP = "";
            if (dbContext.Users.Any(c => c.UserName.Contains(UserName)))
            {
                strU = UserName;
                strP = dbContext.Users.FirstOrDefault(c => c.UserName == UserName).Password;
                sendPasswordToUser(UserName);
            }
            string[] result2Return = new string[2];
            result2Return[0] = strU;
            result2Return[1] = strP;

            return result2Return;
        }

        private static void sendPasswordToUser(String UserName)
        {
            Email email = new Email();
            string mailTo = dbContext.Users.FirstOrDefault(c => c.UserName == UserName).Email;
            string mailSub = "Please keep your password in a safe place";
            string mailBody = "Your password is     " + dbContext.Users.FirstOrDefault(c => c.UserName == UserName).Password;
            email.sendMail(mailTo, mailSub, mailBody);
        }

        public bool hasGraduateInfo()
        {
            return (dbContext.Graduates.FirstOrDefault(c => c.User.UserName == UserName) != null);
        }
    }


    public class PasswordRetrieval
    {
        [Required]
        public string UserName { get; set; }
    }
}