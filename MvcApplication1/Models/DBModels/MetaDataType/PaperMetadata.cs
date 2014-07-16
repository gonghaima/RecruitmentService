using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace MvcApplication1.Models.DBModels.MetaDataType
{
    [MetadataType(typeof(PaperMetadata))]
    public partial class Paper
    {
        // No field here
    }

    [Bind(Exclude = "Id")] 
    public class PaperMetadata
    {
        [Display(Name = "Paper Name")]
        [Required(ErrorMessage = "Paper Name is required.")]
        public string Name;

        [Display(Name = "Grade")]
        [Required(ErrorMessage = "Grade is required.")]
        public int GId { get; set; }
        //public int AcQId { get; set; }

    }
}