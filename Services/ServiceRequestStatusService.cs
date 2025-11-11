using Microsoft.EntityFrameworkCore;
using MunicipalServicesApp.Data;
using MunicipalServicesApp.Models;
using MunicipalServicesApp.DataStructures;

namespace MunicipalServicesApp.Services
{
    /// <summary>
    /// Service implementation for managing service request status with advanced data structures
    /// </summary>
    public class ServiceRequestStatusService : IServiceRequestStatusService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ServiceRequestStatusService> _logger;

     // Data structures
        private BinarySearchTree _bst;
        private AVLTree _avlTree;
    private ServiceRequestMinHeap _minHeap;
 private ServiceRequestMaxHeap _maxHeap;
        private ServiceRequestGraph _graph;

  private readonly List<string> _categories = new()
        {
      "Road Maintenance",
     "Water & Sanitation",
            "Electricity",
   "Waste Management",
         "Parks & Recreation",
     "Street Lighting",
            "Traffic Signals",
          "Public Safety",
         "Noise Complaint",
            "Other"
        };

  private readonly List<string> _departments = new()
        {
            "Public Works",
            "Water Services",
            "Electricity Department",
            "Sanitation Services",
            "Parks and Recreation",
  "Transportation",
      "Safety and Security",
    "General Services"
        };

        public ServiceRequestStatusService(
            ApplicationDbContext context,
      ILogger<ServiceRequestStatusService> logger)
      {
   _context = context;
            _logger = logger;

            // Initialize data structures
          _bst = new BinarySearchTree();
  _avlTree = new AVLTree();
            _minHeap = new ServiceRequestMinHeap();
    _maxHeap = new ServiceRequestMaxHeap();
            _graph = new ServiceRequestGraph();
    }

        #region Basic CRUD Operations

        public async Task<List<ServiceRequest>> GetAllRequestsAsync()
        {
  return await _context.ServiceRequests
             .OrderByDescending(r => r.SubmittedDate)
                .ToListAsync();
        }

  public async Task<ServiceRequest?> GetRequestByIdAsync(int id)
        {
            // Try BST first for O(log n) search
    var bstResult = _bst.Search(id);
     if (bstResult != null)
  return bstResult;

    // Fallback to database
  return await _context.ServiceRequests.FindAsync(id);
    }

        public async Task<ServiceRequest?> GetRequestByNumberAsync(string requestNumber)
{
   // Try BST first
            var bstResult = _bst.SearchByRequestNumber(requestNumber);
    if (bstResult != null)
      return bstResult;

      // Fallback to database
    return await _context.ServiceRequests
 .FirstOrDefaultAsync(r => r.RequestNumber == requestNumber);
        }

  public async Task<int> CreateRequestAsync(ServiceRequest request)
        {
            try
     {
        request.RequestNumber = GenerateRequestNumber();
                request.SubmittedDate = DateTime.Now;
         request.Status = RequestStatus.Submitted;

          // Set estimated completion based on priority
      request.EstimatedCompletionDate = request.Priority switch
                {
       RequestPriority.Critical => DateTime.Now.AddDays(1),
          RequestPriority.High => DateTime.Now.AddDays(3),
RequestPriority.Medium => DateTime.Now.AddDays(7),
                RequestPriority.Low => DateTime.Now.AddDays(14),
         _ => DateTime.Now.AddDays(7)
       };

          _context.ServiceRequests.Add(request);
     await _context.SaveChangesAsync();

                // Rebuild data structures
 await RebuildDataStructuresAsync();

           return request.Id;
            }
            catch (Exception ex)
  {
            _logger.LogError(ex, "Error creating service request");
       throw;
            }
        }

        public async Task<bool> UpdateRequestStatusAsync(int id, RequestStatus status)
   {
     try
       {
    var request = await _context.ServiceRequests.FindAsync(id);
   if (request == null)
 return false;

   request.Status = status;

       // Update dates based on status
     switch (status)
     {
             case RequestStatus.Assigned:
 request.AssignedDate = DateTime.Now;
       break;
   case RequestStatus.InProgress:
               request.InProgressDate = DateTime.Now;
 break;
        case RequestStatus.Resolved:
              case RequestStatus.Closed:
     request.CompletedDate = DateTime.Now;
               break;
 }

          await _context.SaveChangesAsync();

        // Rebuild data structures
      await RebuildDataStructuresAsync();

        return true;
  }
catch (Exception ex)
            {
 _logger.LogError(ex, "Error updating request status");
       return false;
   }
        }

      #endregion

 #region Search and Filtering

        public async Task<List<ServiceRequest>> SearchRequestsAsync(
        string? query, 
            string? category, 
     string? status, 
            string? priority, 
          string? department)
      {
            var requests = _context.ServiceRequests.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
      {
    requests = requests.Where(r =>
        r.RequestNumber.Contains(query) ||
  r.Title.Contains(query) ||
          r.Description.Contains(query) ||
  r.Location.Contains(query));
            }

        if (!string.IsNullOrWhiteSpace(category))
        {
 requests = requests.Where(r => r.Category == category);
            }

    if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<RequestStatus>(status, out var statusEnum))
   {
         requests = requests.Where(r => r.Status == statusEnum);
            }

            if (!string.IsNullOrWhiteSpace(priority) && Enum.TryParse<RequestPriority>(priority, out var priorityEnum))
       {
           requests = requests.Where(r => r.Priority == priorityEnum);
      }

   if (!string.IsNullOrWhiteSpace(department))
     {
       requests = requests.Where(r => r.Department == department);
            }

 return await requests.OrderByDescending(r => r.SubmittedDate).ToListAsync();
        }

    public async Task<List<ServiceRequest>> GetRequestsByStatusAsync(RequestStatus status)
        {
     // Use BST for efficient filtering
            var bstRequests = _bst.GetRequestsByStatus(status);
            if (bstRequests.Any())
      return bstRequests;

  // Fallback to database
return await _context.ServiceRequests
    .Where(r => r.Status == status)
              .OrderByDescending(r => r.SubmittedDate)
         .ToListAsync();
  }

    public async Task<List<ServiceRequest>> GetRequestsByPriorityAsync(RequestPriority priority)
        {
 // Use BST for efficient filtering
        var bstRequests = _bst.GetRequestsByPriority(priority);
 if (bstRequests.Any())
              return bstRequests;

          // Fallback to database
         return await _context.ServiceRequests
          .Where(r => r.Priority == priority)
              .OrderByDescending(r => r.SubmittedDate)
      .ToListAsync();
     }

        public async Task<List<ServiceRequest>> GetRequestsByCategoryAsync(string category)
        {
      return await _context.ServiceRequests
                .Where(r => r.Category == category)
       .OrderByDescending(r => r.SubmittedDate)
      .ToListAsync();
  }

     #endregion

        #region Data Structure Operations

        public List<ServiceRequest> GetRequestsSortedById()
    {
       // Use BST in-order traversal for sorted results
            return _bst.InOrderTraversal();
        }

 public List<ServiceRequest> GetRequestsByPriorityOrder()
        {
        // Use AVL tree for priority-ordered results
       return _avlTree.InOrderTraversal();
        }

        public List<ServiceRequest> GetTopPriorityRequests(int count)
    {
          // Use Max Heap for quick access to top priority requests
    return _maxHeap.GetAllSorted().Take(count).ToList();
  }

  public List<ServiceRequest> GetOldestRequests(int count)
    {
// Use Min Heap for oldest unresolved requests
      var allRequests = _minHeap.GetAllSorted();
        return allRequests
       .Where(r => r.Status != RequestStatus.Resolved && r.Status != RequestStatus.Closed)
   .OrderByDescending(r => r.DaysOpen)
       .Take(count)
     .ToList();
        }

        #endregion

        #region Graph Operations

public List<ServiceRequest> GetRelatedRequests(int requestId)
        {
            return _graph.GetRelatedRequests(requestId);
        }

        public List<ServiceRequest> FindRequestsByLocation(string location)
        {
   return _graph.GetRequestsByLocation(location);
        }

   public Dictionary<string, int> GetLocationClusters()
        {
       return _graph.GetLocationClusters();
        }

        public Dictionary<string, int> GetCategoryDistribution()
    {
      return _graph.GetCategoryClusters();
        }

      #endregion

        #region Statistics and Analytics

  public async Task<Dictionary<string, int>> GetStatusDistributionAsync()
  {
    var distribution = new Dictionary<string, int>();

 foreach (RequestStatus status in Enum.GetValues(typeof(RequestStatus)))
     {
     var count = await _context.ServiceRequests.CountAsync(r => r.Status == status);
           distribution[status.ToString()] = count;
      }

            return distribution;
        }

  public async Task<Dictionary<string, int>> GetPriorityDistributionAsync()
        {
  var distribution = new Dictionary<string, int>();

 foreach (RequestPriority priority in Enum.GetValues(typeof(RequestPriority)))
         {
                var count = await _context.ServiceRequests.CountAsync(r => r.Priority == priority);
 distribution[priority.ToString()] = count;
       }

     return distribution;
        }

        public async Task<double> GetAverageResolutionTimeAsync()
        {
         var resolvedRequests = await _context.ServiceRequests
              .Where(r => r.Status == RequestStatus.Resolved || r.Status == RequestStatus.Closed)
      .Where(r => r.CompletedDate != null)
      .ToListAsync();

 if (!resolvedRequests.Any())
 return 0;

      var averageDays = resolvedRequests
        .Average(r => (r.CompletedDate!.Value - r.SubmittedDate).TotalDays);

            return Math.Round(averageDays, 1);
        }

        #endregion

        #region Helper Methods

        public Task<List<string>> GetCategoriesAsync()
        {
    return Task.FromResult(_categories);
     }

        public Task<List<string>> GetDepartmentsAsync()
        {
            return Task.FromResult(_departments);
      }

   public string GenerateRequestNumber()
        {
    var year = DateTime.Now.Year;
      var count = _context.ServiceRequests.Count(r => r.SubmittedDate.Year == year);
            return $"REQ-{year}-{(count + 1):D5}";
        }

        public async Task RebuildDataStructuresAsync()
        {
   try
            {
    var allRequests = await _context.ServiceRequests.ToListAsync();

            // Clear existing structures
           _bst = new BinarySearchTree();
       _avlTree = new AVLTree();
                _minHeap = new ServiceRequestMinHeap();
         _maxHeap = new ServiceRequestMaxHeap();
          _graph = new ServiceRequestGraph();

    // Rebuild all structures
        foreach (var request in allRequests)
     {
      _bst.Insert(request);
          _avlTree.Insert(request);
        _minHeap.Insert(request);
              _maxHeap.Insert(request);
 _graph.AddRequest(request);
                }

         // Build graph relationships
              BuildGraphRelationships(allRequests);

          _logger.LogInformation($"Data structures rebuilt with {allRequests.Count} requests");
         }
  catch (Exception ex)
     {
                _logger.LogError(ex, "Error rebuilding data structures");
}
        }

        private void BuildGraphRelationships(List<ServiceRequest> requests)
        {
   // Create relationships based on:
            // 1. Same location
        // 2. Same category
 // 3. Similar time period

       for (int i = 0; i < requests.Count; i++)
            {
 for (int j = i + 1; j < requests.Count; j++)
          {
    var req1 = requests[i];
      var req2 = requests[j];

    bool isRelated = false;

  // Same location and category
        if (req1.Location == req2.Location && req1.Category == req2.Category)
        {
   isRelated = true;
              }
          // Same location within 7 days
      else if (req1.Location == req2.Location &&
           Math.Abs((req1.SubmittedDate - req2.SubmittedDate).TotalDays) <= 7)
   {
         isRelated = true;
           }
              // Same category and high priority
        else if (req1.Category == req2.Category &&
  req1.Priority >= RequestPriority.High &&
          req2.Priority >= RequestPriority.High)
              {
       isRelated = true;
  }

          if (isRelated)
      {
             _graph.AddRelationship(req1.Id, req2.Id);
    }
        }
            }
        }

        #endregion
    }
}
