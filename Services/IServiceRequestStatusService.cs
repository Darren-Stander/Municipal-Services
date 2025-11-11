using MunicipalServicesApp.Models;
using MunicipalServicesApp.Models.ViewModels;

namespace MunicipalServicesApp.Services
{
    /// <summary>
    /// Service interface for managing service request status and tracking
    /// </summary>
    public interface IServiceRequestStatusService
    {
        // Basic CRUD operations
        Task<List<ServiceRequest>> GetAllRequestsAsync();
        Task<ServiceRequest?> GetRequestByIdAsync(int id);
        Task<ServiceRequest?> GetRequestByNumberAsync(string requestNumber);
        Task<int> CreateRequestAsync(ServiceRequest request);
        Task<bool> UpdateRequestStatusAsync(int id, RequestStatus status);

        // Advanced search and filtering
        Task<List<ServiceRequest>> SearchRequestsAsync(string? query, string? category, string? status, string? priority, string? department);
        Task<List<ServiceRequest>> GetRequestsByStatusAsync(RequestStatus status);
        Task<List<ServiceRequest>> GetRequestsByPriorityAsync(RequestPriority priority);
        Task<List<ServiceRequest>> GetRequestsByCategoryAsync(string category);

        // Data structure operations
        List<ServiceRequest> GetRequestsSortedById();
        List<ServiceRequest> GetRequestsByPriorityOrder();
        List<ServiceRequest> GetTopPriorityRequests(int count);
        List<ServiceRequest> GetOldestRequests(int count);

        // Graph operations
        List<ServiceRequest> GetRelatedRequests(int requestId);
 List<ServiceRequest> FindRequestsByLocation(string location);
      Dictionary<string, int> GetLocationClusters();
        Dictionary<string, int> GetCategoryDistribution();

        // Statistics and analytics
        Task<Dictionary<string, int>> GetStatusDistributionAsync();
        Task<Dictionary<string, int>> GetPriorityDistributionAsync();
        Task<double> GetAverageResolutionTimeAsync();

     // Helper methods
      Task<List<string>> GetCategoriesAsync();
        Task<List<string>> GetDepartmentsAsync();
        string GenerateRequestNumber();

  // Rebuild data structures (called when data changes)
        Task RebuildDataStructuresAsync();
    }
}
