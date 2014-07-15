//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcApplication1.Models.DBModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class Graduate
    {
        public Graduate()
        {
            this.CoverLetters = new HashSet<CoverLetter>();
            this.CVs = new HashSet<CV>();
            this.SoftSkills = new HashSet<SoftSkill>();
            this.JobTitles = new HashSet<JobTitle>();
        }
    
        public string StudentId { get; set; }
        public string KnownAs { get; set; }
        public string VisaStatus { get; set; }
        public string FirstLanguage { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public int JobTypeId { get; set; }
        public Nullable<int> UId { get; set; }
        public string CVId { get; set; }
    
        public virtual ICollection<CoverLetter> CoverLetters { get; set; }
        public virtual ICollection<CV> CVs { get; set; }
        public virtual JobType JobType { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<SoftSkill> SoftSkills { get; set; }
        public virtual ICollection<JobTitle> JobTitles { get; set; }
    }
}
