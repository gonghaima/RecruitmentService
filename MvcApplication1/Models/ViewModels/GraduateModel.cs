using System;
using System.ComponentModel.DataAnnotations;
using MvcApplication1.Models.DBModels;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Web.Mvc;
namespace MvcApplication1.Models.ViewModels
{
    public class GraduateModel : Person
    {
        dataEntities dbContext = new dataEntities();

        public string UserName { get; set; }

        [Required]
        public string StudentId { get; set; }

        public string KnownAs { get; set; }

        [Required]
        public string VisaStatus { get; set; }

        [Required]
        public string FirstLanguage { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Preferred Work Type")]
        public int JobTypeId { get; set; }

        public bool profileSaved { set; get; }

        public bool graduateExist { get; set; }

        public IEnumerable<JobType> IenumJobType { get; set; }

        [Required]
        public Nullable<int> UId { get; set; }

        public string CVId { get; set; }

        public Graduate existingGrad = new Graduate();
        User us;

        public SelectList JobTypeList;
        //ViewBag.jt = new SelectList(gradModel.IenumJobType, "Id", "Name");

        public Email mail {get;set;}

        public GraduateModel()
        {
            this.IenumJobType = dbContext.JobTypes;
            mail = new ViewModels.Email();
            JobTypeList = new SelectList(this.IenumJobType, "Id", "Name");

        }

        public GraduateModel(string un)
        {
            mail = new ViewModels.Email();
            existingGrad = dbContext.Graduates.FirstOrDefault(c => c.User.UserName == un);
            this.IenumJobType = dbContext.JobTypes;
            JobTypeList = new SelectList(this.IenumJobType, "Id", "Name");
            us = dbContext.Users.FirstOrDefault(c => c.UserName == un);

            this.UserName = us.UserName;
            this.FirstName = us.FirstName;
            this.LastName = us.LastName;
            this.Email = us.Email;
            this.UId = us.Id;
            if (existingGrad != null)
            {
                graduateExist = true;
                this.StudentId = existingGrad.StudentId;
                this.KnownAs = existingGrad.KnownAs;
                this.VisaStatus = existingGrad.VisaStatus;
                this.PhoneNumber = existingGrad.PhoneNumber;
                this.Location = existingGrad.Location;
                this.JobTypeId = existingGrad.JobTypeId;
                this.UId = existingGrad.UId;
                this.CVId = existingGrad.CVId;
                this.FirstLanguage = existingGrad.FirstLanguage;
                this.IenumJobType = dbContext.JobTypes;
            }
        }


        Graduate convertToGraduate()
        {
            Graduate grad = new Graduate();
            grad.StudentId = StudentId;
            grad.KnownAs = KnownAs;
            grad.VisaStatus = VisaStatus;
            grad.FirstLanguage = FirstLanguage;
            grad.PhoneNumber = PhoneNumber;
            grad.Location = Location;
            grad.JobTypeId = JobTypeId;
            grad.UId = UId;
            grad.CVId = (CVId == null) ? null : CVId;
            updateUser(UId);
            return grad;
        }

        //Update user information
        private void updateUser(int? UId)
        {
            User us = dbContext.Users.FirstOrDefault(c => c.Id == UId);
            if (us != null)
            {
                us.FirstName = FirstName;
                us.LastName = LastName;
                us.Email = Email;
            }
        }

        public void updateExistingGrad()
        {
            existingGrad.KnownAs = KnownAs;
            existingGrad.VisaStatus = VisaStatus;
            existingGrad.FirstLanguage = FirstLanguage;
            existingGrad.PhoneNumber = PhoneNumber;
            existingGrad.Location = Location;
            existingGrad.JobTypeId = JobTypeId;
            updateUser(UId);
        }

        public bool addToDB()
        {
            graduateExist = dbContext.Graduates.Any(c => c.User.UserName == this.UserName);
            if (graduateExist)
            {
                updateExistingGrad();
                dbContext.SaveChanges();

                //Confirmation email to the graduate
                string mailSub = "Profile Updated";
                string mailBody = "The graduate: " + this.FirstName + " " + this.LastName + " has updated his/her profile";
                mail.sendMail(this.Email, mailSub, mailBody);

                profileSaved = true;
                return profileSaved;
            }
            else
            {
                dbContext.Graduates.Add(convertToGraduate());
                dbContext.SaveChanges();

                //Confirmation email to the graduate
                Email mail = new Email();
                string mailSub = "Your graduate profiles has been created";
                string mailBody = "The graduate: " + this.FirstName + " " + this.LastName + " has updated his/her profile";
                mail.sendMail(this.Email, mailSub, mailBody);

                profileSaved = true;
                return profileSaved;
            }

        }
    }
}