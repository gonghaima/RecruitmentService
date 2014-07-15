using System.Collections.Generic;
using MvcApplication1.Models.DBModels;
using System.Linq;
using System.Data.Entity;
namespace MvcApplication1.Models.ViewModels
{
    public class WorkingExperiencesModel
    {
        public IEnumerable<WorkingExprience> WorkingExperiences { get; set; }
        public CV CVInfo { get; set; }
        dataEntities dbContext = new dataEntities();
        //string gradStudentID;
        string userName;

        public WorkingExperiencesModel(string uName)
        {
            userName= uName;
            //CVInfo = dbContext.CVs.FirstOrDefault(c => c.Graduate.User.UserName == uName);
            //if (CVInfo == null)
            //{
            //    string SId=dbContext.Graduates.FirstOrDefault(c=>c.User.UserName==uName).StudentId ;
            //    CVInfo = new CV() { Name = uName, StudentId = SId };
            //    dbContext.CVs.Add(CVInfo);
            //    dbContext.SaveChanges();
            //}
            CVInfo = CVInitializer.getCV(uName);
            WorkingExperiences = findExperienceByUserName();
        }

        public bool addOrUpdate(WorkingExprience we)
        {
            bool updateStatus;
            WorkingExprience exsitingWE= dbContext.WorkingExpriences.FirstOrDefault(c => c.Id == we.Id) ;
            if (exsitingWE == null)
            {
                updateStatus=addSingleExperience(we);
            }
            else
            {
                updateStatus=updateSingleExperience(exsitingWE, we);
            }
            return updateStatus;
        }

        private bool updateSingleExperience(WorkingExprience existingW, WorkingExprience we)
        {
            existingW.Start = we.Start;
            existingW.Finish= we.Finish;
            existingW.JobTitle= we.JobTitle;
            existingW.Company= we.Company;
            existingW.RoleDescription= we.RoleDescription;
            dbContext.SaveChanges();
            return true;
        }


        public bool addSingleExperience(WorkingExprience we)
        {
            we.CId = CVInfo.Id;
            dbContext.WorkingExpriences.Add(we);
            dbContext.SaveChanges();
            return true;
        }

        public bool deleteSingleExperience(string weId)
        {
            int wId = int.Parse(weId);
            var c = new WorkingExprience() { Id = wId };
            dbContext.Entry(c).State = EntityState.Deleted;
            dbContext.SaveChanges();
            return true;
        }

        public IEnumerable<WorkingExprience> findExperienceByUserName()
        {
            IEnumerable<WorkingExprience> exs = dbContext.WorkingExpriences.Where(c => c.CV.Graduate.User.UserName == userName);
            return exs;
        }
    }
}