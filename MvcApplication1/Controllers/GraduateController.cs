using MvcApplication1.Models.DBModels;
using MvcApplication1.Models.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity;
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

         [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
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

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult EditExperience(string id)
        {
            return View(dbContext.WorkingExpriences.FirstOrDefault(c=>c.Id.ToString()==id));
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult DeleteExperience(string id)
        {
            WorkingExperiencesModel wem = new WorkingExperiencesModel(User.Identity.Name);
            wem.deleteSingleExperience(id);
            return RedirectToAction("ShowEditExperience");
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
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

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult Papers()
        {
            IEnumerable<Paper> paperList = dbContext.Papers.Where(c => c.AcQualification.CV.Graduate.User.UserName == User.Identity.Name);
            if (paperList.Any())
            {
                return View(paperList);
            }
            return View();
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public PartialViewResult CreatePaper()
        {
            ViewBag.grade = new SelectList(dbContext.Grades, "Id", "Name");
            return PartialView("Paper", new Paper());
        }

        [HttpPost]
        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult CreatePaper(Paper pr)
        {
            ViewBag.grade = new SelectList(dbContext.Grades, "Id", "Name");
            if (ModelState.IsValid)
            {
                pr.AcQId = dbContext.AcQualifications.SingleOrDefault(c => c.CV.Graduate.User.UserName == User.Identity.Name).Id;
                dbContext.Papers.Add(pr);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Papers");
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult ShowVendorQualification()
        {
            IEnumerable<VdQualification> exsitingVQ;
            exsitingVQ = dbContext.VdQualifications.Where(c => c.CV.Graduate.User.UserName == User.Identity.Name);
            if (exsitingVQ != null)
            {
                return View(exsitingVQ);
            }
            else
            {
                return View();
            }
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public PartialViewResult CreateVdQualification(int? VdQualificationId)
        {
            generateSelectList();
            if (ModelState.IsValid)
            {
                if (VdQualificationId == null)
                {
                    return PartialView("VdQualification", new VdQualification());
                }
                else
                {
                    VdQualification we = dbContext.VdQualifications.FirstOrDefault(c => c.Id == VdQualificationId);
                    return PartialView("VdQualification", we);
                }
            }
            return PartialView("VdQualification", new VdQualification());
        }

        private void generateSelectList()
        {
            ViewBag.vdExperience = new SelectList(dbContext.Experiences, "Id", "Name");
            ViewBag.ability = new SelectList(dbContext.Abilities, "Id", "Name");
            ViewBag.Competency = new SelectList(dbContext.Competencies, "Id", "Name");
        }

        [HttpPost]
        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult CreateVdQualification(VdQualification vdq)
        {
            generateSelectList();
            vdq.CVId = dbContext.CVs.FirstOrDefault(c => c.Graduate.User.UserName == User.Identity.Name).Id;
            if (ModelState.IsValid)
            {
                //Update existing or create new
                if (dbContext.VdQualifications.Any(c => c.Id == vdq.Id))
                {
                    VdQualification tmpVdq = dbContext.VdQualifications.FirstOrDefault(c => c.Id == vdq.Id);
                    tmpVdq.Name = vdq.Name;
                    tmpVdq.DateCompleted= vdq.DateCompleted;
                    tmpVdq.ExperienceId= vdq.ExperienceId;
                    tmpVdq.AbilityId= vdq.AbilityId;
                    tmpVdq.CompetencyId= vdq.CompetencyId;
                    dbContext.SaveChanges();
                }
                else
                {
                    dbContext.VdQualifications.Add(vdq);
                    dbContext.SaveChanges();
                }
            }
            else
            {
                return View(vdq);
            }
            return RedirectToAction("ShowVendorQualification");
        }

        public ActionResult EditVdQualification(string id)
        {
            return View(dbContext.VdQualifications.FirstOrDefault(c=>c.Id.ToString()==id));
        }

        public ActionResult DeleteVdQualification(string id)
        {
            int vId = int.Parse(id);
            var c = new VdQualification() { Id = vId };
            dbContext.Entry(c).State = EntityState.Deleted;
            dbContext.SaveChanges();
            return RedirectToAction("ShowVendorQualification");
        }

        public ActionResult CV()
        {
            CVModel cvModel = new CVModel(User.Identity.Name);
            return View(cvModel);
        }

        public ActionResult CoverLetter()
        {
            IEnumerable<CoverLetter> ienumCL = dbContext.CoverLetters.Where(c => c.Graduate.User.UserName == User.Identity.Name);
            return View(ienumCL);
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult ShowSoftskill()
        {
            string sId = dbContext.Graduates.FirstOrDefault(c => c.User.UserName == User.Identity.Name).StudentId;
            IEnumerable<StudentSoftskillLevel> softSkills = dbContext.StudentSoftskillLevels.Where(c => c.StudentId == sId);
            return View(softSkills);
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public PartialViewResult CreateSoftskill(int? SoftSkillId)
        {
            return PartialView("SoftSkill", new SoftSkill());
        }

        [HttpPost]
        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult CreateSoftskill(SoftSkill sskill)
        {
            SoftSkill exsitingssk = dbContext.SoftSkills.FirstOrDefault(c => c.Id == sskill.Id);
            SoftSkill exsitingName = dbContext.SoftSkills.FirstOrDefault(c => c.Name == sskill.Name.Trim());
            //If already a record with same id, just update name
            if (exsitingssk != null)
            {
                exsitingssk.Name = sskill.Name;
                dbContext.SaveChanges();
            }//If there is already a same skill name, return
            else if (exsitingName != null)
            {
                return RedirectToAction("ShowSoftskill");
            }//New, add the new softskill
            else
            {
                dbContext.SoftSkills.Add(sskill);
                dbContext.SaveChanges();
            }
            return RedirectToAction("ShowSoftskill");
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public PartialViewResult AddSoftskill(int? SoftSkillId)
        {
            generateSoftSkillSelectList();
            return PartialView("AddSoftskill", new StudentSoftskillLevel());
        }

        private void generateSoftSkillSelectList()
        {
            ViewBag.skillList = new SelectList(dbContext.SoftSkills, "Id", "Name");
            ViewBag.skillLevelList = new SelectList(dbContext.SoftSkillLevels, "Id", "Name");
        }

        [HttpPost]
        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Graduate")]
        public ActionResult SoftskillProcessing(StudentSoftskillLevel ssssl)
        {
            string sId = dbContext.Graduates.FirstOrDefault(c => c.User.UserName.ToString() == User.Identity.Name).StudentId;
            ssssl.StudentId = sId;
            StudentSoftskillLevel exsitsk = dbContext.StudentSoftskillLevels.FirstOrDefault(c => c.StudentId == sId && c.SoftSkillId == ssssl.SoftSkillId);
            StudentSoftskillLevel exsitskl = dbContext.StudentSoftskillLevels.FirstOrDefault(c => c.StudentId == sId && c.SoftSkillId == ssssl.SoftSkillId && c.SoftSkillLevelId == ssssl.SoftSkillLevelId);
            
            //If there is already a graduate with the skill, remove it and add new record
            if (exsitsk != null)
            {
                dbContext.Entry(exsitsk).State = EntityState.Deleted;
                dbContext.StudentSoftskillLevels.Add(ssssl);
                dbContext.SaveChanges();
            } //If no such graduate information at all, insert a new record.
            else if (exsitskl == null)
            {
                dbContext.StudentSoftskillLevels.Add(ssssl);
                dbContext.SaveChanges();
            }

            return RedirectToAction("ShowSoftskill");
        }

        public ActionResult DeleteStudentSoftskill(StudentSoftskillLevel sskl)
        {
            dbContext.Entry(sskl).State = EntityState.Deleted;
            dbContext.SaveChanges();
            return RedirectToAction("ShowSoftskill");
        }

    }
}
