using MvcApplication1.Models.DBModels;
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
    }
}