using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Models.ViewModels
{
    /// <summary>
    /// View model for Service Request Status page
    /// </summary>
public class RequestStatusViewModel
    {
        // Search/Filter properties
        public string? SearchQuery { get; set; }
        public string? SelectedCategory { get; set; }
        public string? SelectedStatus { get; set; }
        public string? SelectedPriority { get; set; }
        public string? SelectedDepartment { get; set; }
      public string SortBy { get; set; } = "Date";

        // Data properties
        public List<ServiceRequest> AllRequests { get; set; } = new();
        public List<ServiceRequest> FilteredRequests { get; set; } = new();
        public List<ServiceRequest> TopPriorityRequests { get; set; } = new();
        public List<ServiceRequest> OldestRequests { get; set; } = new();
 public List<ServiceRequest> RelatedRequests { get; set; } = new();

   // Statistics
        public int TotalRequests { get; set; }
        public int OpenRequests { get; set; }
        public int InProgressRequests { get; set; }
     public int ResolvedRequests { get; set; }
        public double AverageResolutionDays { get; set; }

        // Analytics data
        public Dictionary<string, int> CategoryDistribution { get; set; } = new();
        public Dictionary<string, int> LocationClusters { get; set; } = new();
      public Dictionary<string, int> StatusDistribution { get; set; } = new();
        public Dictionary<string, int> PriorityDistribution { get; set; } = new();

        // Dropdown options
        public List<string> Categories { get; set; } = new();
        public List<string> Departments { get; set; } = new();
    }

    /// <summary>
    /// View model for tracking a specific service request
    /// </summary>
    public class TrackRequestViewModel
    {
    public ServiceRequest? Request { get; set; }
   public List<ServiceRequest> RelatedRequests { get; set; } = new();
        public List<ServiceRequest> SimilarRequests { get; set; } = new();
        public string? ErrorMessage { get; set; }

        // Timeline data
        public List<RequestTimelineEvent> Timeline { get; set; } = new();
    }

    /// <summary>
    /// Represents an event in the request timeline
    /// </summary>
    public class RequestTimelineEvent
    {
  public string Status { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
