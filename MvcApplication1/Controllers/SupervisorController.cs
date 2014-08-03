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
                dbContext.Users.Attach(us);
                var entry = dbContext.Entry(us);
                entry.Property(e => e.Activated).IsModified = true;
                dbContext.SaveChanges();
            }
            ViewBag.updated = true;
            return View(dbContext.Users);
        }
    }
}
