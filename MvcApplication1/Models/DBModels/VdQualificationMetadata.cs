using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcApplication1.Models.DBModels.MetaDataType
{
    [MetadataType(typeof(VdQualificationMetadata))]
    public partial class VdQualification
    {
        [Bind(Exclude = "Id")]
        class VdQualificationMetadata
        {
            [Display(Name = "Name of the qualification")]
            [Required(ErrorMessage = "Qualification Name is required.")]
            public string Name { get; set; }

            [Display(Name = "Completion Date")]
            [Required(ErrorMessage = "Date is required.")]
            public Nullable<System.DateTime> DateCompleted{get;set;}

            [Display(Name = "Experience Level")]
            [Required(ErrorMessage = "Experience Level is required")]
            public int ExperienceId{get;set;}

            [Display(Name = "Ability Level")]
            [Required(ErrorMessage = "Ability Level is required.")]
            public int AbilityId { get; set; }

            [Display(Name = "Competency Level")]
            [Required(ErrorMessage = "Competency Level is required.")]
            public int CompetencyId { get; set; }
        }
    }
}