using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace MvcApplication1.Models.DBModels
{
    [MetadataType(typeof(WorkingExprienceMetadata))]
    public partial class WorkingExprience
    {
        // No field here
    }

    public class WorkingExprienceMetadata
    {
        [Required(ErrorMessage = "Job Start Date is required.")]
        public System.DateTime Start;

        [Required(ErrorMessage = "Job Title is required.")]
        public string JobTitle;

        [Required(ErrorMessage = "Company Name is required.")]
        public string Company;

        [Required(ErrorMessage = "Job Title is required.")]
        public string RoleDescription;

    }
}
