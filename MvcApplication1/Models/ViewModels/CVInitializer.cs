using MvcApplication1.Models.DBModels;
using System.Collections.Generic;
using System.Linq;
namespace MvcApplication1.Models.ViewModels
{
    public static class CVInitializer
    {
        
        public static CV getCV(string uName)
        {
            dataEntities dbContext = new dataEntities();
            
            CV existingCV = dbContext.CVs.FirstOrDefault(c => c.Graduate.User.UserName == uName);
            if (existingCV == null)
            {
                string SId = dbContext.Graduates.FirstOrDefault(c => c.User.UserName == uName).StudentId;
                CV newCV = new CV() { Name = uName, StudentId = SId };
                dbContext.CVs.Add(newCV);
                dbContext.SaveChanges();
                return dbContext.CVs.FirstOrDefault(c=>c.StudentId==newCV.StudentId);
            }
            return existingCV;
        }

        public static CV initCV(string SId)
        {
            dataEntities dbContext = new dataEntities();

            CV existingCV = dbContext.CVs.FirstOrDefault(c => c.StudentId== SId);
            string userName = dbContext.Graduates.FirstOrDefault(c => c.StudentId == SId).User.UserName;
            if (existingCV == null)
            {
                CV newCV = new CV() { Name = userName, StudentId = SId };
                dbContext.CVs.Add(newCV);
                dbContext.SaveChanges();
                return dbContext.CVs.FirstOrDefault(c => c.StudentId == newCV.StudentId);
            }
            return existingCV;
        }

        public static void initAllCV()
        {
            dataEntities dbContext = new dataEntities();
            var lstSId = dbContext.Graduates.Select(c => c.StudentId);
            foreach (string sid in lstSId)
            {
                initCV(sid);
            }
        }
    }
}