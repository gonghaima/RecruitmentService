using MvcApplication1.Models.DBModels;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
namespace MvcApplication1.Models.ViewModels
{
    public class CVModel
    {
        dataEntities dbContext = new dataEntities();
        public string userName { get; set; }
        public User user { get; set; }
        public Graduate graduate { get; set; }
        public AcQualification acQualification { get; set; }
        public IEnumerable<VdQualification> vdQualifications { get; set; }
        public IEnumerable<WorkingExprience> wkExperiences { get; set; }
        public CV CVInfo { get; set; }

        public CVModel(string uName)
        {
            CVInfo = CVInitializer.getCV(uName);
            userName = uName;
            user = dbContext.Users.FirstOrDefault(c => c.UserName == userName);
            graduate = dbContext.Graduates.FirstOrDefault(c => c.User.UserName == userName);
            acQualification = dbContext.AcQualifications.FirstOrDefault(c => c.CV.Graduate.User.UserName == userName);
            vdQualifications = dbContext.VdQualifications.Where(c => c.CV.Graduate.User.UserName == userName);
            wkExperiences = dbContext.WorkingExpriences.Where(c => c.CV.Graduate.User.UserName == userName);
        }
    }
}