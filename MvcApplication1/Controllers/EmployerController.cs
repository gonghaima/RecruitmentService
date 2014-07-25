using MvcApplication1.Models.DBModels;
using MvcApplication1.Models.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity;

namespace MvcApplication1.Controllers
{
    public class EmployerController : Controller
    {
        dataEntities dbContext = new dataEntities();
        //
        // GET: /Employer/


        //Generate SelectList for job CRUD
        private void generateJobSelectList()
        {
            ViewBag.JobTitle = new SelectList(dbContext.JobTitles.OrderBy(c => c.Name), "Id", "Name");
            ViewBag.AcademicLevel = new SelectList(dbContext.AcQualificationLevels, "Id", "Name");
            ViewBag.JobType = new SelectList(dbContext.JobTypes, "Id", "Name");
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Employer")]
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
            if (ModelState.IsValid && acc.register("Employer"))
            {
                acc.login();
                return RedirectToAction("Index");
            }
            else
            {
                return View(acc);
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account acc)
        {
            if (acc.login())
            {
                IEnumerable<Employer> Emps = dbContext.Employers.Where(c => c.User.UserName == acc.UserName);
                if (Emps != null)
                {
                    return RedirectToAction("Index", acc);
                }
            }
            return View();
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Employer")]
        public ActionResult EmployerProfile()
        {
            ViewBag.saved = false;
            return View(new EmployerModel(User.Identity.Name));
        }

        [HttpPost]
        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Employer")]
        public ActionResult EmployerProfile(EmployerModel empModel)
        {
            ViewBag.saved = false;
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                ViewBag.saved = empModel.addToDB(User.Identity.Name);
                return View(new EmployerModel(User.Identity.Name));
            }
            else
            {
                return View(new EmployerModel(User.Identity.Name));
            }
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Employer")]
        public ActionResult ShowActivatedJobs()
        {
            IEnumerable<Job> enumJobs= dbContext.Jobs.Where(c => c.Employer.User.UserName == User.Identity.Name);

            if (enumJobs != null)
            {
                return View(enumJobs);
            }
            else
            {
                return View();
            }
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Employer")]
        public ActionResult CreateNewJob()
        {
            generateJobSelectList();
            return View();
        }


        [HttpPost]
        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Employer")]
        public ActionResult CreateNewJob(Job job)
        {
            generateJobSelectList();
            
            
            if (ModelState.IsValid)
            {
                job.EmployerId = dbContext.Employers.SingleOrDefault(c => c.User.UserName == User.Identity.Name).Id;
                dbContext.Jobs.Add(job);
                dbContext.SaveChanges();
                return RedirectToAction("JobCreated");
            }
            else
            {
                return View(job);
            }
        }

        //Confirmation page of job created, but need site supervisor activation.
        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Employer")]
        public ActionResult JobCreated()
        {
            return View();
        }


        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Employer")]
        public ActionResult EditJob(string id)
        {
            generateJobSelectList();
            return View(dbContext.Jobs.FirstOrDefault(c=>c.Id.ToString()==id));
        }

        [HttpPost]
        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Employer")]
        public ActionResult EditJob(Job job)
        {
            if (ModelState.IsValid)
            {
                Job existingJob = dbContext.Jobs.FirstOrDefault(c => c.Id == job.Id);
                existingJob.JobTitleId = job.JobTitleId;
                existingJob.JobTypeId = job.JobTypeId;
                existingJob.JobDescription = job.JobDescription;
                existingJob.RequiredAcqLevelId = job.RequiredAcqLevelId;

                dbContext.SaveChanges();
                return RedirectToAction("ShowActivatedJobs");
            }
            else
            {
                return View(job);
            }
        }


        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Employer")]
        public ActionResult DeleteJob(string id)
        {
            Job job = dbContext.Jobs.FirstOrDefault(c => c.Id.ToString() == id);
            dbContext.Entry(job).State = EntityState.Deleted;
            dbContext.SaveChanges();
            return RedirectToAction("ShowActivatedJobs");
        }

        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Employer")]
        public ActionResult Candidate()
        {
            ViewBag.JobList = new SelectList(
             from c in dbContext.Jobs
             where c.Employer.User.UserName == User.Identity.Name
             select new { c.Id, S_Name = c.JobTitle.Name }, "Id", "S_Name");

            CVInitializer.initAllCV();

            IEnumerable<Graduate> enumGrad = dbContext.Graduates;

            return View(enumGrad);
        }


        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Employer")]
        public ActionResult cv(string id)
        {
            //ViewBag.CoverLetterStatus = new SelectList(dbContext.CoverLetterStatus, "Id", "Name");
            //ViewBag.JobList = new SelectList(dbContext.Jobs.Where(c => c.Employer.User.UserName == User.Identity.Name), "Id","Name");

            ViewBag.JobList = new SelectList(
                from c in dbContext.Jobs
                where c.Employer.User.UserName == User.Identity.Name
                select new { c.Id, S_Name = c.JobTitle.Name }, "Id", "S_Name");

            string candidateUserName = dbContext.CVs.FirstOrDefault(c => c.Id.ToString() == id).Graduate.User.UserName;
            CVModel cvModel = new CVModel(candidateUserName);
            return View(cvModel);
        }

        [HttpPost]
        [MvcApplication1.MvcApplication.OptionalAuthorize(Roles = "Employer")]
        public ActionResult InvitationOfCoverLetter()
        {
            int jobId = int.Parse(Request.Form["jobId"]);
            string studentId = Request.Form["studentId"];
            int statusId=dbContext.CoverLetterStatus.FirstOrDefault(c=>c.Name=="Invited").Id;

            CoverLetter cl = dbContext.CoverLetters.FirstOrDefault(c => c.JobId== jobId && c.StudentId == studentId);
            sendInvitationEmail(studentId);
            if (cl == null)
            {
                CoverLetter tempCl= new CoverLetter() { StudentId = studentId, JobId = jobId, StatusId = statusId };
                dbContext.CoverLetters.Add(tempCl);
                dbContext.SaveChanges();
                
            }
            return View("InvitationSent");
        }

        private void sendInvitationEmail(string sId)
        {
            string mailTo= dbContext.Graduates.FirstOrDefault(c => c.StudentId == sId).User.Email;
            string mailSub = "Job Invitation";
            string mailBody = "A job Invitation has been created by an employer, please check your Invitation List in cover letter section.";
            Email email = new Email();
            email.sendMail(mailTo, mailSub, mailBody);
        }
    }
}
