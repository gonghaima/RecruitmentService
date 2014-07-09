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
    
    public partial class SoftSkill
    {
        public SoftSkill()
        {
            this.Graduates = new HashSet<Graduate>();
            this.Jobs = new HashSet<Job>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int SSLevelId { get; set; }
    
        public virtual SoftSkillLevel SoftSkillLevel { get; set; }
        public virtual ICollection<Graduate> Graduates { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}