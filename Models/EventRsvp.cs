using System.ComponentModel.DataAnnotations;

namespace MunicipalServicesApp.Models
{
    public class EventRsvp
    {
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cell phone number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20)]
        [Display(Name = "Cell Phone Number")]
        public string CellPhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(200)]
        [Display(Name = "Email (Optional)")]
        public string? Email { get; set; }

        public DateTime RsvpDate { get; set; } = DateTime.Now;

        
        public LocalEvent? Event { get; set; }
    }
}//end of file
