using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MunicipalServicesApp.Models
{
    public class ReportIssue
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [NotMapped]
        public List<string> AttachmentPaths { get; set; } = new List<string>();

        public string AttachmentPathsString
        {
            get => string.Join(",", AttachmentPaths);
            set => AttachmentPaths = string.IsNullOrEmpty(value) 
                ? new List<string>() 
                : value.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public DateTime ReportedDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending";
    }
}// end of file
