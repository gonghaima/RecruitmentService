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
    
    public partial class SoftSkillLevel
    {
        public SoftSkillLevel()
        {
            this.StudentSoftskillLevels = new HashSet<StudentSoftskillLevel>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int Ranking { get; set; }
    
        public virtual ICollection<StudentSoftskillLevel> StudentSoftskillLevels { get; set; }
    }
}