using System.Web.Mvc;
using MvcApplication1.Models.ViewModels;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public void Test()
        {
            Email em = new Email();
            em.sendMail();
        }
    }
}
