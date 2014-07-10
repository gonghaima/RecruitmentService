using MvcApplication1.Models.ViewModels;
using System.Web.Mvc;
namespace MvcApplication1.Controllers
{
    public class GraduateController : Controller
    {
        //
        // GET: /Gradudate/

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        //[MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Employer")]
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
            if (ModelState.IsValid && acc.register("Graduate"))
            {
                return RedirectToAction("Index", acc);
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
                return RedirectToAction("Index");
            }
            return View();
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult GraduateProfile()
        {
            string UserName = User.Identity.Name;
            GraduateModel gm = new GraduateModel(UserName);
            ViewBag.jt = new SelectList(gm.IenumJobType,"Id","Name");
            return View(gm);
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        [HttpPost]
        public ActionResult GraduateProfile(GraduateModel gradModel)
        {
            return View(gradModel);
        }
    }
}
