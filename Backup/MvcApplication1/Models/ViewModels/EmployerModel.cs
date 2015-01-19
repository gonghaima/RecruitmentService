using System;
using System.ComponentModel.DataAnnotations;
using MvcApplication1.Models.DBModels;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;
using System.Web.Mvc;
namespace MvcApplication1.Models.ViewModels
{
    public class EmployerModel : Person
    {
        dataEntities dbContext = new dataEntities();

        [Required]
        public string Address { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        public string Phone { get; set; }

        public bool profileSaved { set; get; }

        public bool employerExist { get; set; }

        [Required]
        public int UId { get; set; }

        public Employer existingEmp;
        User us;

        public EmployerModel()
        {
        }

        public EmployerModel(string un)
        {
            
            existingEmp = dbContext.Employers.FirstOrDefault(c => c.User.UserName == un);
            us = dbContext.Users.FirstOrDefault(c => c.UserName == un);
            
            this.FirstName = us.FirstName;
            this.LastName = us.LastName;
            this.Email = us.Email;
            this.UId = us.Id;
            if (existingEmp != null)
            {
                employerExist = true;

                this.UId = existingEmp.UId;
                this.Address = existingEmp.Address;
                this.Company = existingEmp.Company;
                this.Phone = existingEmp.Phone;
            }
        }


        Employer convertToEmp()
        {
            Employer emp = new Employer();

            emp.UId = UId;
            emp.Address = Address;
            emp.Company = Company;
            emp.Phone = Phone;
            
            updateUser(UId);
            return emp;
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

        public void updateExistingEmp()
        {
            existingEmp.Address = Address;
            existingEmp.Company = Company;
            existingEmp.Phone = Phone;
            updateUser(UId);
        }

        public bool addToDB(string uName)
        {
            if (employerExist)
            {
                existingEmp = dbContext.Employers.FirstOrDefault(c => c.User.UserName == uName);
                updateExistingEmp();
                dbContext.SaveChanges();
                profileSaved = true;
                return profileSaved;
            }
            else
            {
                dbContext.Employers.Add(convertToEmp());
                dbContext.SaveChanges();
                profileSaved = true;
                return profileSaved;
            }

        }
    }
}