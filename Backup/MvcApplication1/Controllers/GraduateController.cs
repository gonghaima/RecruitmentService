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
        public ActionResult Index(string userName)
        {
            ViewBag.userName = userName;
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
            bool valid=ModelState.IsValid ;
            bool regSucceed = false;

            //return if validation failed
            if (valid)
            {
                regSucceed = acc.register("Graduate");
            }
            else
            {
                return View(acc);
            }

            //redirect to Index page if registration succeed.
            if (regSucceed)
            {
                return RedirectToAction("GraduateProfile", new { userName = acc.UserName, submitTxt = "Next Step", forRegistration=true });
            }
            else
            {
                ViewBag.userExist = "There is already a user with the same user name in database";
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
                    return RedirectToAction("Index", new { userName = acc.UserName });
                }
            }
            return View();
        }

        public ActionResult GraduateProfile(string submitTxt, bool? forRegistration)
        {
            ViewBag.saved = false;
            ViewBag.subTxt = submitTxt;
            ViewBag.forRegistration = forRegistration;
            
            return View(new GraduateModel(HttpContext.User.Identity.Name));
        }

        [HttpPost]
        public ActionResult GraduateProfile(GraduateModel gradModel)
        {
            ViewBag.saved = false;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            string msg = Request.Form["forRegistration"];
            bool forRegistration = (msg=="True")?true:false;
            //if valid, process and move to working experience page
            if (ModelState.IsValid && !forRegistration)
            {
                ViewBag.saved = gradModel.addToDB();
                //Account acc = new Account(gra);

                return RedirectToAction("Index", new { userName = gradModel.UserName });
            }
            else if (ModelState.IsValid)
            {
                ViewBag.saved=gradModel.addToDB();

                //return View(new GraduateModel(gradModel.UserName));
                return RedirectToAction("ShowEditExperience", new { userName = gradModel.UserName });
            }
                //otherwise, show validation message
            else
            {
                return View(gradModel);
            }
        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult ShowEditExperience(string userName)
        {
            //WorkingExperiencesModel wem = new WorkingExperiencesModel(User.Identity.Name);
            WorkingExperiencesModel wem = new WorkingExperiencesModel(userName);
            ViewBag.userName = userName;
            if (wem.WorkingExperiences.Any())
            {
                return View(wem.WorkingExperiences);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult ShowEditExperience()
        {
            string userName=Request.Form["userName"];
            return RedirectToAction("AcademicQualification", new { userName=userName});
            //return View(exs);
        }

        public PartialViewResult CreateExperience(int? workingExperienceId, string userName)
        {
            ViewBag.userName = userName;
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
        public ActionResult CreateExperience(WorkingExprience we)
        {
            string userName = Request.Form["userName"];
            if (ModelState.IsValid)
            {
                WorkingExperiencesModel wem = new WorkingExperiencesModel(userName);
                wem.addOrUpdate(we);
                return RedirectToAction("ShowEditExperience", new { userName = userName });
            }
            else
            {
                return RedirectToAction("ShowEditExperience", new { userName = userName });
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

        public ActionResult AcademicQualification(string userName)
        {
            ViewBag.userName = userName;
            ViewBag.jobTypeList = new SelectList(dbContext.AcQualificationLevels, "Id", "Name");
            //AcQualificationModel acqModel = new AcQualificationModel(User.Identity.Name);
            AcQualificationModel acqModel = new AcQualificationModel(userName);
            if (acqModel.acq != null)
            {
                return View(acqModel.acq);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AcademicQualification(AcQualification acq)
        {
            string AddPaperOrNextStep = Request.Form["submitButton"];
            string userName = Request.Form["userName"];

            //If next step, save ACqualification and move to VDqulification
            if (AddPaperOrNextStep == "Next Step")
            {
                
                ViewBag.jobTypeList = new SelectList(dbContext.AcQualificationLevels, "Id", "Name");
                //AcQualificationModel acqModel = new AcQualificationModel(acq, User.Identity.Name);
                AcQualificationModel acqModel = new AcQualificationModel(acq, userName);
                acq.CVId = acqModel.CVInfo.Id;
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {
                    acqModel.addOrUpdate();
                    //return View();
                    return RedirectToAction("ShowVendorQualification", new { userName = userName });
                }
                else
                {
                    return View(acq);
                }
            }
            //If Add paper, save ACqualification and move to papers page.
            else if (AddPaperOrNextStep == "Add Papers")
            {
                ViewBag.jobTypeList = new SelectList(dbContext.AcQualificationLevels, "Id", "Name");
                //AcQualificationModel acqModel = new AcQualificationModel(acq, User.Identity.Name);
                AcQualificationModel acqModel = new AcQualificationModel(acq, userName);
                acq.CVId = acqModel.CVInfo.Id;
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {
                    acqModel.addOrUpdate();
                    //return View();
                    return RedirectToAction("Papers", new { userName = userName });
                }
                else
                {
                    return View(acq);
                }
            }
            //other scenarios, return with posted back value.
            else
            {
                return View(acq);
            }
        }

        public ActionResult Papers(string userName)
        {
            ViewBag.userName=userName;
            //IEnumerable<Paper> paperList = dbContext.Papers.Where(c => c.AcQualification.CV.Graduate.User.UserName == User.Identity.Name);
            IEnumerable<Paper> paperList = dbContext.Papers.Where(c => c.AcQualification.CV.Graduate.User.UserName == userName);
            if (paperList.Any())
            {
                return View(paperList);
            }
            return View();
        }

        public PartialViewResult CreatePaper(string userName)
        {
            ViewBag.userName = userName;
            ViewBag.grade = new SelectList(dbContext.Grades, "Id", "Name");
            return PartialView("Paper", new Paper());
        }

        [HttpPost]
        public ActionResult CreatePaper(Paper pr)
        {
            string userName = Request.Form["userName"];
            ViewBag.grade = new SelectList(dbContext.Grades, "Id", "Name");
            if (ModelState.IsValid)
            {
                pr.AcQId = dbContext.AcQualifications.FirstOrDefault(c => c.CV.Graduate.User.UserName == userName).Id;
                dbContext.Papers.Add(pr);
                dbContext.SaveChanges();
            }
            return RedirectToAction("Papers", new { userName=userName});
        }

        public ActionResult ShowVendorQualification(string userName)
        {
            ViewBag.userName = userName;
            IEnumerable<VdQualification> exsitingVQ;
            //exsitingVQ = dbContext.VdQualifications.Where(c => c.CV.Graduate.User.UserName == User.Identity.Name);
            exsitingVQ = dbContext.VdQualifications.Where(c => c.CV.Graduate.User.UserName == userName);
            if (exsitingVQ != null)
            {
                return View(exsitingVQ);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult ShowVendorQualification()
        {
            string userName = Request.Form["userName"];
            return RedirectToAction("ShowSoftskill", new { userName = userName });
        }

        public PartialViewResult CreateVdQualification(int? VdQualificationId, string userName)
        {
            ViewBag.userName = userName;
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
        public ActionResult CreateVdQualification(VdQualification vdq)
        {
            string userName=Request.Form["userName"];
            generateSelectList();
            //vdq.CVId = dbContext.CVs.FirstOrDefault(c => c.Graduate.User.UserName == User.Identity.Name).Id;
            vdq.CVId = dbContext.CVs.FirstOrDefault(c => c.Graduate.User.UserName == userName).Id;
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
            return RedirectToAction("ShowVendorQualification", new { userName=userName});
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

        public ActionResult CV(string userName)
        {
            CVModel cvModel = new CVModel(userName);
            return View(cvModel);
        }

        public ActionResult CoverLetter()
        {
            IEnumerable<CoverLetter> ienumCL = dbContext.CoverLetters.Where(c => c.Graduate.User.UserName == User.Identity.Name);
            return View(ienumCL);
        }

        [HttpGet]
        public ActionResult ShowSoftskill(string userName)
        {
            ViewBag.userName = userName;
            string sId = dbContext.Graduates.FirstOrDefault(c => c.User.UserName == userName).StudentId;
            IEnumerable<StudentSoftskillLevel> softSkills = dbContext.StudentSoftskillLevels.Where(c => c.StudentId == sId);
            return View(softSkills);
        }

        //[HttpPost]
        //public ActionResult ShowSoftskill()
        //{
        //    string userName=Request.Form["userName"];
        //    return RedirectToAction("RegistrationCompleted");
        //}

        public ActionResult RegistrationCompleted(string userName)
        {
            //(No need, null by default) update user status

            //sending notification email to site supervisor
            Email email = new Email();
            //Role adminRole=dbContext.Roles.SingleOrDefault(c=>c.Name=="Supervisor");
            //IEnumerable<string> adminMailList= dbContext.Users.Where(c => c.Roles.Contains(adminRole)).Select(c=>c.Email);

            IEnumerable<string> adminMailList=dbContext.SiteSuperVisors.Select(c => c.User.Email);
            List<string> emailList = adminMailList.ToList();
            //email.sendMail(emailList, "Registration Approval", "A graduate has registered, please login and activate his or her account");
            //var supers = dbContext.SiteSuperVisors;

            Role r = dbContext.Roles.FirstOrDefault(c => c.Name == "Supervisor");
            IEnumerable<User> superUsers = dbContext.Users.Where(c => c.Roles.Any(g=>g.Id==r.Id));

            List<string> emails= new List<string>();
            foreach (User sup in superUsers)
            {
                emails.Add(sup.Email);
            }

            email.sendMail(emails, "Registration Approval Request", "A graduate has registered, please login and activate his or her account");

            //foreach (string strSupEmail in emailList)
            //{
            //    email.sendMail(strSupEmail, "Registration Approval", "A graduate has registered, please login and activate his or her account");
            //}


            return View();
        }

        public PartialViewResult CreateSoftskill(int? SoftSkillId, string userName)
        {
            ViewBag.userName = userName;
            return PartialView("SoftSkill", new SoftSkill());
        }

        [HttpPost]
        public ActionResult CreateSoftskill(SoftSkill sskill)
        {
            string userName = Request.Form["userName"];
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
                return RedirectToAction("ShowSoftskill", new {userName=userName });
            }//New, add the new softskill
            else
            {
                dbContext.SoftSkills.Add(sskill);
                dbContext.SaveChanges();
            }
            return RedirectToAction("ShowSoftskill", new { userName = userName });
        }

        public PartialViewResult AddSoftskill(int? SoftSkillId, string userName)
        {
            ViewBag.userName = userName;
            generateSoftSkillSelectList();
            return PartialView("AddSoftskill", new StudentSoftskillLevel());
        }

        private void generateSoftSkillSelectList()
        {
            ViewBag.skillList = new SelectList(dbContext.SoftSkills, "Id", "Name");
            ViewBag.skillLevelList = new SelectList(dbContext.SoftSkillLevels, "Id", "Name");
        }

        [HttpPost]
        public ActionResult SoftskillProcessing(StudentSoftskillLevel ssssl)
        {
            string userName=Request.Form["userName"];
            string sId = dbContext.Graduates.FirstOrDefault(c => c.User.UserName.ToString() == userName).StudentId;
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

            return RedirectToAction("ShowSoftskill", new { userName=userName});
        }

        public ActionResult DeleteStudentSoftskill(StudentSoftskillLevel sskl)
        {
            dbContext.Entry(sskl).State = EntityState.Deleted;
            dbContext.SaveChanges();
            return RedirectToAction("ShowSoftskill");
        }

        public PartialViewResult showNews(string userName)
        {
            return PartialView("News", dbContext.News);
        }

        public ActionResult NewsDetails(string userName, string newsId)
        {
            ViewBag.userName = userName;
            News news= dbContext.News.FirstOrDefault(c => c.Id.ToString() == newsId);
            if (news != null)
            {
                return View(news);
            }
            else
            {
                return View();
            }
        }
    }
}
