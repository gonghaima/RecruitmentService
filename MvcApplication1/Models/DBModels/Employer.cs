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
    
    public partial class Employer
    {
        public Employer()
        {
            this.Jobs = new HashSet<Job>();
        }
    
        public int Id { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public int UId { get; set; }
    
        public virtual User User { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}