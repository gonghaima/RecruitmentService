using System;
using System.ComponentModel.DataAnnotations;
using MvcApplication1.Models.DBModels;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Web.Mvc;
namespace MvcApplication1.Models.ViewModels
{
    public class GraduateModel : Account
    {
        dataEntities dbContext = new dataEntities();

        [Required]
        public string StudentId { get; set; }

        public string KnownAs { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public System.DateTime DoB { get; set; }

        [Required]
        public string VisaStatus { get; set; }

        [Required]
        public string FirstLanguage { get; set; }

        [Required]
        public decimal IELTS { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public int JobTypeId { get; set; }

        public IEnumerable<JobType> IenumJobType { get; set; }

        [Required]
        public Nullable<int> UId { get; set; }

        public string CVId { get; set; }

        public GraduateModel() { }

        public GraduateModel(string un) 
        {
            Graduate grad = new Graduate();
            grad = dbContext.Graduates.FirstOrDefault(c => c.User.UserName == un);
            if (grad != null)
            {
                this.StudentId = grad.StudentId;
                this.KnownAs = grad.KnownAs;
                this.DoB = grad.DoB;
                this.VisaStatus = grad.VisaStatus;
                this.IELTS = grad.IELTS;
                this.PhoneNumber = grad.PhoneNumber;
                this.Location = grad.Location;
                this.JobTypeId = grad.JobTypeId;
                this.UId = grad.UId;
                this.CVId = grad.CVId;
                this.IenumJobType = dbContext.JobTypes;
            }
            else
            {
                this.IenumJobType = dbContext.JobTypes;
            }
        }



        Graduate convertToGraduate()
        {
            Graduate grad = new Graduate();
            grad.StudentId = StudentId;
            grad.KnownAs = KnownAs;
            grad.DoB = DoB;
            grad.VisaStatus =VisaStatus;
            grad.FirstLanguage= FirstLanguage;
            grad.IELTS= IELTS;
            grad.PhoneNumber= PhoneNumber;
            grad.Location = Location;
            grad.JobTypeId = JobTypeId;
            grad.UId = UId;
            grad.CVId= (CVId==null)?null:CVId;
            return grad;
        }

        void addToDB()
        {
            dbContext.Graduates.Add(convertToGraduate());
            dbContext.SaveChanges();
        }
    }
}