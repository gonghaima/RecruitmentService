using MvcApplication1.Models.DBModels;
using MvcApplication1.Models.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity;

namespace MvcApplication1.Controllers
{
    public class SupervisorController : Controller
    {
        dataEntities dbContext = new dataEntities();

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Supervisor")]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Account acc)
        {
            if (ModelState.IsValid && acc.register("Supervisor"))
            {
                acc.login();
                return RedirectToAction("Index");
            }
            else
            {
                return View(acc);
            }
        }

        public ActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated == true)
            {
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpPost]
        public ActionResult Login(Account acc)
        {
            if (acc.login())
            {
                IEnumerable<SiteSuperVisor> Emps = dbContext.SiteSuperVisors.Where(c => c.User.UserName == acc.UserName);
                if (Emps != null)
                {
                    return RedirectToAction("Index", acc);
                }
            }
            return View();
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Supervisor")]
        public ActionResult UserActivation()
        {
            return View(dbContext.Users);
        }

        [HttpPost]
        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Supervisor")]
        public ActionResult UserActivation(IEnumerable<MvcApplication1.Models.DBModels.User> users)
        {
            foreach (User us in users)
            {
                User tempUser = dbContext.Users.FirstOrDefault(c => c.Id == us.Id);
                if (tempUser.Activated == false && us.Activated == true)
                {
                    sendingActivationEmailToUser(us.Email);
                }
                tempUser.UserName = us.UserName;
                tempUser.FirstName = us.FirstName;
                tempUser.LastName = us.LastName;
                tempUser.Email = us.Email;
                tempUser.Activated= us.Activated;
                dbContext.Entry(tempUser).State = EntityState.Modified;
                dbContext.SaveChanges();
            }

            //dbContext.SaveChanges();

            ViewBag.updated = true;
            return View(dbContext.Users);
        }

        //Notify user that account has been activated
        private void sendingActivationEmailToUser(string email)
        {
            Email mail = new Email();
            mail.sendMail(email, "Account Activated", "Your account has been activated, now you can login with your credentials.");
        }

        //News
        public ActionResult ShowNews()
        {
            return View(dbContext.News);
        }

        //Create News
        public ActionResult CreateNews()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateNews(News news)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            //ModelState.Remove("Id"); // Remove the key  (same as [Bind(Exclude = "Id")]  )
            if (ModelState.IsValid)
            {
                dbContext.News.Add(news);
                dbContext.SaveChanges();
                return RedirectToAction("ShowNews");
            }
            else
            {
                return View(news);
            }

        }
    }
}
