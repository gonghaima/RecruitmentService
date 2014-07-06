using MvcApplication1.Models.ViewModels;
using System.Web.Mvc;
namespace MvcApplication1.Controllers
{
    public class StudentController : Controller
    {
        //
        // GET: /Student/

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
        public ActionResult Register(Student student)
        {
            if (ModelState.IsValid&&student.register())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(student);
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Student student)
        {
            if (student.login())
            {
                return RedirectToAction("Index");
            }

            return View();
        }
    }
}
