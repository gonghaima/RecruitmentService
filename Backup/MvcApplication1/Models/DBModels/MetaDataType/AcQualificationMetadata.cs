using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MvcApplication1.Models.DBModels
{
    [MetadataType(typeof(AcQualificationMetadata))]
    public partial class AcQualification
    {
        // No field here
    }

    [Bind(Exclude = "Id")] 
    public class AcQualificationMetadata
    {

        [Display(Name="Name of the qualification")]
        [Required(ErrorMessage = "The Name of the qualification is required.")]
        public string Name;

        [Display(Name="Level of the qualification")]
        [Required(ErrorMessage = "The level of the qualification is required.")]
        public int AcqLevelId;
        
    }
}