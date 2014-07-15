using MvcApplication1.Models.DBModels;
using MvcApplication1.Models.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
namespace MvcApplication1.Controllers
{
    public class GraduateController : Controller
    {
        dataEntities dbContext = new dataEntities();

        //
        // GET: /Gradudate/

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult Index(Account acc)
        {
            return View(acc);
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
                return RedirectToAction("GraduateProfile");
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
                IEnumerable<Graduate> Grads = dbContext.Graduates.Where(c => c.User.UserName == acc.UserName);
                if (Grads != null)
                {
                    return RedirectToAction("Index", acc);
                }
            }
            return View();
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult GraduateProfile()
        {
            ViewBag.saved = false;
            return View(new GraduateModel(User.Identity.Name));
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        [HttpPost]
        public ActionResult GraduateProfile(GraduateModel gradModel)
        {
            ViewBag.saved = false;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                ViewBag.saved=gradModel.addToDB();
                return View(new GraduateModel(User.Identity.Name));
            }
            else
            {
                return View(new GraduateModel(User.Identity.Name));
            }
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult ShowEditExperience()
        {

            WorkingExperiencesModel wem = new WorkingExperiencesModel(User.Identity.Name);
            if (wem.WorkingExperiences != null)
            {
                return View(wem.WorkingExperiences);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult ShowEditExperience(IEnumerable<WorkingExprience> exs)
        {

            return View(exs);
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public PartialViewResult CreateExperience(int? workingExperienceId)
        {
            if (ModelState.IsValid)
            {
                if (workingExperienceId == null)
                {
                    return PartialView("WorkingExperience", new WorkingExprience());
                }
                else
                {
                    WorkingExprience we = dbContext.WorkingExpriences.FirstOrDefault(c => c.Id == workingExperienceId);
                    return PartialView("WorkingExperience", we);
                }
            }
            return PartialView("WorkingExperience", new WorkingExprience());
        }

        [HttpPost]
        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult CreateExperience(WorkingExprience we)
        {
            if (ModelState.IsValid)
            {
                WorkingExperiencesModel wem = new WorkingExperiencesModel(User.Identity.Name);
                wem.addOrUpdate(we);
                return RedirectToAction("ShowEditExperience");
            }
            else
            {
                return RedirectToAction("ShowEditExperience");
            }
            
        }

        public ActionResult EditExperience(string id)
        {
            return View(dbContext.WorkingExpriences.FirstOrDefault(c=>c.Id.ToString()==id));
        }

        public ActionResult DeleteExperience(string id)
        {
            WorkingExperiencesModel wem = new WorkingExperiencesModel(User.Identity.Name);
            wem.deleteSingleExperience(id);
            return RedirectToAction("ShowEditExperience");
        }

        public ActionResult AcademicQualification()
        {
            ViewBag.jobTypeList = new SelectList(dbContext.AcQualificationLevels, "Id", "Name");
            AcQualificationModel acqModel = new AcQualificationModel(User.Identity.Name);
            if (acqModel.acq != null)
            {
                return View(acqModel.acq);
            }
            return View();
        }

        [HttpPost]
        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult AcademicQualification(AcQualification acq)
        {
            ViewBag.jobTypeList = new SelectList(dbContext.AcQualificationLevels, "Id", "Name");
            AcQualificationModel acqModel = new AcQualificationModel(acq, User.Identity.Name);
            acq.CVId = acqModel.CVInfo.Id;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                acqModel.addOrUpdate();
                return View();
            }
            else
            {
                return View(acq);
            }
            
        }

        public ActionResult Papers()
        {

            return View();
        }
    }
}
