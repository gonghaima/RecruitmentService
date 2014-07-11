using System.Collections.Generic;
using MvcApplication1.Models.DBModels;
using System.Linq;
namespace MvcApplication1.Models.ViewModels
{
    public class WorkingExperiencesModel
    {
        public IEnumerable<WorkingExprience> WorkingExperiences { get; set; }

        dataEntities dbContext = new dataEntities();
        public void addToDb()
        {
            foreach (WorkingExprience we in WorkingExperiences)
            {
                WorkingExprience w= dbContext.WorkingExpriences.FirstOrDefault(c => c.Id == we.Id);
                if (w != null)
                {
                    w.Start = we.Start;
                    w.Finish = we.Finish;
                    w.JobTitle = we.JobTitle;
                    w.Company = we.Company;
                    w.RoleDescription = we.RoleDescription;
                    w.CId = we.CId;
                }
                else
                {
                    dbContext.WorkingExpriences.Add(we);
                }
                dbContext.SaveChanges();
            }
        }
    }
}