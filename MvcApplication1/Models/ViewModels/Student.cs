using MvcApplication1.Controllers;
using MvcApplication1.Models.DBModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Security;

namespace MvcApplication1.Models.ViewModels
{
    public class Student : IRegisterInterface, IUserInterface
    {
        dataEntities dbContext = new dataEntities();
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        int Id { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        //[Required]
        [Display(Name = "Student Password")]
        public string Password { get; set; }

        //[Required]
        [Display(Name = "Confirm Password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Student First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Student Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        public Role role_student { get; set; }
        User user_student = new User();

        public bool register()
        {
            user_student.UserName = UserName;
            user_student.Password = Password;
            user_student.FirstName = FirstName;
            user_student.LastName = LastName;
            user_student.Email = Email;

            User tempUser = dbContext.Users.FirstOrDefault(c => c.UserName == user_student.UserName);
            
            if (tempUser != null)
            {
                //already a user in database
                return false;
            }
            else
            {
                dbContext.Users.Add(user_student);
                Role r = dbContext.Roles.SingleOrDefault(c => c.Name == "Student");
                dbContext.SaveChanges();
                dbContext.Users.SingleOrDefault(c => c.UserName == user_student.UserName).Roles.Add(r);
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
    }
}