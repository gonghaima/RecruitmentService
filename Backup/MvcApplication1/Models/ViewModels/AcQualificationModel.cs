
using MvcApplication1.Models.DBModels;
using System.Web.Mvc;
using System.Linq;
namespace MvcApplication1.Models.ViewModels
{
    public class AcQualificationModel
    {
        dataEntities dbContext = new dataEntities();
        public AcQualification acq { get; set; }
        public SelectList acqLevel{get;set;}
        public CV CVInfo { get; set; }


        public AcQualificationModel(string userName)
        {
            acqLevel = new SelectList(dbContext.AcQualificationLevels, "Id", "Name");
            CVInitializer.initAllCV();
            CVInfo = CVInitializer.getCV(userName);
            try
            {
                acq = dbContext.AcQualifications.FirstOrDefault(c => c.CV.Graduate.User.UserName == userName);
            }
            catch
            {

            }
        }
        public AcQualificationModel(AcQualification ac, string userName)
        {
            acq = ac;
            acqLevel = new SelectList(dbContext.AcQualificationLevels, "Id", "Name");
            CVInfo = CVInitializer.getCV(userName);
        }

        public bool addOrUpdate()
        {
            AcQualification existingAcq = dbContext.AcQualifications.FirstOrDefault(c => c.Id == acq.Id);
            if (existingAcq != null)
            {
                existingAcq.Name = acq.Name;
                existingAcq.AcqLevelId = acq.AcqLevelId;
                dbContext.SaveChanges();
                return true;
            }
            else
            {
                dbContext.AcQualifications.Add(acq);
                dbContext.SaveChanges();
                return true;
            }
        }
    }
}