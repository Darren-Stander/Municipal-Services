using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesApp.Models
{
    public class LocalEvent
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public int Priority { get; set; } = 0;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}// end of file
