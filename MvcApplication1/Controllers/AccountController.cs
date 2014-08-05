using MvcApplication1.Models.ViewModels;
using System.Web.Mvc;
namespace MvcApplication1.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(PasswordRetrieval pr)
        {
            if (ModelState.IsValid)
            {
                string[] retrievedResult = new string[2];
                retrievedResult = Account.retrievePassword(pr.UserName);

                if (string.IsNullOrEmpty(retrievedResult[0]))
                {
                    ViewBag.message = "The user name does not exist, please try again.";
                }
                else
                {
                    ViewBag.message = "The password has been sent to you, please check on your email";
                }
                return View();
            }
            else
            {
                return View(pr);
            }
        }

        [HttpPost]
        public ActionResult Logout()
        {
            Account.logout();
            return RedirectToAction("Index","Home");
        }
    }
}
