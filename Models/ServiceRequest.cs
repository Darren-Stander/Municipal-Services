using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MunicipalServicesApp.Models
{
    /// <summary>
    /// Represents a service request in the municipal system
    /// </summary>
    public class ServiceRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string RequestNumber { get; set; } = string.Empty; // Unique identifier like REQ-2024-00001

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
   [StringLength(2000)]
  public string Description { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Submitted;

        [Required]
        public RequestPriority Priority { get; set; } = RequestPriority.Medium;

 public DateTime SubmittedDate { get; set; } = DateTime.Now;

  public DateTime? AssignedDate { get; set; }

        public DateTime? InProgressDate { get; set; }

        public DateTime? CompletedDate { get; set; }

  public DateTime? EstimatedCompletionDate { get; set; }

        [StringLength(100)]
      public string? AssignedTo { get; set; }

  [StringLength(100)]
        public string SubmittedBy { get; set; } = "Citizen";

        [StringLength(500)]
        public string? Notes { get; set; }

        // Related service requests (for graph structure)
        [NotMapped]
        public List<int> RelatedRequestIds { get; set; } = new List<int>();

      public string RelatedRequestIdsString
        {
          get => string.Join(",", RelatedRequestIds);
         set => RelatedRequestIds = string.IsNullOrEmpty(value)
    ? new List<int>()
     : value.Split(',', StringSplitOptions.RemoveEmptyEntries)
      .Select(int.Parse)
 .ToList();
      }

        // Department responsible
        [StringLength(100)]
    public string Department { get; set; } = "General";

 // Calculated properties
      [NotMapped]
        public int DaysOpen => (CompletedDate ?? DateTime.Now).Subtract(SubmittedDate).Days;

     [NotMapped]
      public string StatusColor => Status switch
 {
     RequestStatus.Submitted => "info",
        RequestStatus.UnderReview => "warning",
         RequestStatus.Assigned => "primary",
   RequestStatus.InProgress => "primary",
       RequestStatus.Resolved => "success",
            RequestStatus.Closed => "secondary",
   RequestStatus.Rejected => "danger",
   _ => "secondary"
        };

        [NotMapped]
  public string PriorityColor => Priority switch
    {
            RequestPriority.Critical => "danger",
     RequestPriority.High => "warning",
      RequestPriority.Medium => "info",
            RequestPriority.Low => "secondary",
  _ => "secondary"
   };
    }

    /// <summary>
    /// Enumeration of possible request statuses
  /// </summary>
    public enum RequestStatus
    {
     Submitted = 0,
        UnderReview = 1,
        Assigned = 2,
        InProgress = 3,
        Resolved = 4,
        Closed = 5,
        Rejected = 6
    }

    /// <summary>
    /// Enumeration of priority levels
    /// </summary>
public enum RequestPriority
    {
    Low = 0,
        Medium = 1,
        High = 2,
      Critical = 3
    }
}
