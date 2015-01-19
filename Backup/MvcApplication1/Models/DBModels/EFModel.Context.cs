﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dataEntities : DbContext
    {
        public dataEntities()
            : base("name=dataEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<AcQualification> AcQualifications { get; set; }
        public DbSet<AcQualificationLevel> AcQualificationLevels { get; set; }
        public DbSet<Competency> Competencies { get; set; }
        public DbSet<CoverLetter> CoverLetters { get; set; }
        public DbSet<CV> CVs { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Graduate> Graduates { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
        public DbSet<Paper> Papers { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<SiteSuperVisor> SiteSuperVisors { get; set; }
        public DbSet<VdQualification> VdQualifications { get; set; }
        public DbSet<WorkingExprience> WorkingExpriences { get; set; }
        public DbSet<SoftSkill> SoftSkills { get; set; }
        public DbSet<SoftSkillLevel> SoftSkillLevels { get; set; }
        public DbSet<StudentSoftskillLevel> StudentSoftskillLevels { get; set; }
        public DbSet<CoverLetterStatu> CoverLetterStatus { get; set; }
        public DbSet<News> News { get; set; }
    }
}
