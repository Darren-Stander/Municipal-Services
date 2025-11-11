using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Services;
using MunicipalServicesApp.Models;
using MunicipalServicesApp.Models.ViewModels;

namespace MunicipalServicesApp.Controllers
{
    public class RequestStatusController : Controller
    {
        private readonly IServiceRequestStatusService _statusService;
        private readonly ILogger<RequestStatusController> _logger;

        public RequestStatusController(
            IServiceRequestStatusService statusService,
            ILogger<RequestStatusController> logger)
        {
            _statusService = statusService;
            _logger = logger;
        }

        /// <summary>
        /// Main index page showing all service requests with filtering and advanced data structure features
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchQuery,
            string? category,
            string? status,
            string? priority,
            string? department,
            string sortBy = "Date")
        {
            try
            {
                var viewModel = new RequestStatusViewModel
                {
                    SearchQuery = searchQuery,
                    SelectedCategory = category,
                    SelectedStatus = status,
                    SelectedPriority = priority,
                    SelectedDepartment = department,
                    SortBy = sortBy
                };

                // Get all requests
                viewModel.AllRequests = await _statusService.GetAllRequestsAsync();

                // Apply search and filters
                if (!string.IsNullOrWhiteSpace(searchQuery) ||
                    !string.IsNullOrWhiteSpace(category) ||
                    !string.IsNullOrWhiteSpace(status) ||
                    !string.IsNullOrWhiteSpace(priority) ||
                    !string.IsNullOrWhiteSpace(department))
                {
                    viewModel.FilteredRequests = await _statusService.SearchRequestsAsync(
                        searchQuery, category, status, priority, department);
                }
                else
                {
                    viewModel.FilteredRequests = viewModel.AllRequests;
                }

                // Apply sorting using data structures
                viewModel.FilteredRequests = sortBy switch
                {
                    "Priority" => _statusService.GetRequestsByPriorityOrder()
                        .Where(r => viewModel.FilteredRequests.Any(f => f.Id == r.Id)).ToList(),
                    "Id" => _statusService.GetRequestsSortedById()
                        .Where(r => viewModel.FilteredRequests.Any(f => f.Id == r.Id)).ToList(),
                    _ => viewModel.FilteredRequests.OrderByDescending(r => r.SubmittedDate).ToList()
                };

                // Get top priority requests using heap
                viewModel.TopPriorityRequests = _statusService.GetTopPriorityRequests(5);

                // Get oldest unresolved requests using heap
                viewModel.OldestRequests = _statusService.GetOldestRequests(5);

                // Calculate statistics
                viewModel.TotalRequests = viewModel.AllRequests.Count;
                viewModel.OpenRequests = viewModel.AllRequests.Count(r =>
                    r.Status == RequestStatus.Submitted ||
                    r.Status == RequestStatus.UnderReview ||
                    r.Status == RequestStatus.Assigned);
                viewModel.InProgressRequests = viewModel.AllRequests.Count(r =>
                    r.Status == RequestStatus.InProgress);
                viewModel.ResolvedRequests = viewModel.AllRequests.Count(r =>
                    r.Status == RequestStatus.Resolved ||
                    r.Status == RequestStatus.Closed);
                viewModel.AverageResolutionDays = await _statusService.GetAverageResolutionTimeAsync();

                // Get analytics using graph
                viewModel.CategoryDistribution = _statusService.GetCategoryDistribution();
                viewModel.LocationClusters = _statusService.GetLocationClusters();
                viewModel.StatusDistribution = await _statusService.GetStatusDistributionAsync();
                viewModel.PriorityDistribution = await _statusService.GetPriorityDistributionAsync();

                // Get dropdown options
                viewModel.Categories = await _statusService.GetCategoriesAsync();
                viewModel.Departments = await _statusService.GetDepartmentsAsync();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading request status index");
                return View("Error");
            }
        }

        /// <summary>
        /// Track a specific request by request number or ID
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Track(string? requestNumber, int? id)
        {
            try
            {
                var viewModel = new TrackRequestViewModel();

                ServiceRequest? request = null;

                if (!string.IsNullOrWhiteSpace(requestNumber))
                {
                    request = await _statusService.GetRequestByNumberAsync(requestNumber);
                }
                else if (id.HasValue)
                {
                    request = await _statusService.GetRequestByIdAsync(id.Value);
                }

                if (request == null)
                {
                    viewModel.ErrorMessage = "Request not found. Please check the request number and try again.";
                    return View(viewModel);
                }

                viewModel.Request = request;

                // Get related requests using graph traversal
                viewModel.RelatedRequests = _statusService.GetRelatedRequests(request.Id);

                // Get similar requests (same location or category)
                var locationRequests = _statusService.FindRequestsByLocation(request.Location);
                viewModel.SimilarRequests = locationRequests
                    .Where(r => r.Id != request.Id)
                    .Take(5)
                    .ToList();

                // Build timeline
                viewModel.Timeline = BuildTimeline(request);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error tracking request");
                var viewModel = new TrackRequestViewModel
                {
                    ErrorMessage = "An error occurred while tracking your request. Please try again."
                };
                return View(viewModel);
            }
        }

        /// <summary>
        /// Search for a request (used for quick search)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> QuickSearch(string requestNumber)
        {
            if (string.IsNullOrWhiteSpace(requestNumber))
            {
                TempData["ErrorMessage"] = "Please enter a request number.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Track", new { requestNumber });
        }

        /// <summary>
        /// View analytics dashboard with graph visualizations
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Analytics()
        {
            try
            {
                var viewModel = new RequestStatusViewModel();

                viewModel.AllRequests = await _statusService.GetAllRequestsAsync();

                // Statistics
                viewModel.TotalRequests = viewModel.AllRequests.Count;
                viewModel.OpenRequests = viewModel.AllRequests.Count(r =>
                    r.Status == RequestStatus.Submitted ||
                    r.Status == RequestStatus.UnderReview ||
                    r.Status == RequestStatus.Assigned);
                viewModel.InProgressRequests = viewModel.AllRequests.Count(r =>
                    r.Status == RequestStatus.InProgress);
                viewModel.ResolvedRequests = viewModel.AllRequests.Count(r =>
                    r.Status == RequestStatus.Resolved ||
                    r.Status == RequestStatus.Closed);
                viewModel.AverageResolutionDays = await _statusService.GetAverageResolutionTimeAsync();

                // Analytics data
                viewModel.CategoryDistribution = _statusService.GetCategoryDistribution();
                viewModel.LocationClusters = _statusService.GetLocationClusters();
                viewModel.StatusDistribution = await _statusService.GetStatusDistributionAsync();
                viewModel.PriorityDistribution = await _statusService.GetPriorityDistributionAsync();

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading analytics");
                return View("Error");
            }
        }

        /// <summary>
        /// Helper method to build request timeline
        /// </summary>
        private List<RequestTimelineEvent> BuildTimeline(ServiceRequest request)
        {
            var timeline = new List<RequestTimelineEvent>();

            // Submitted
            timeline.Add(new RequestTimelineEvent
            {
                Status = "Submitted",
                Date = request.SubmittedDate,
                Icon = "bi-file-earmark-plus",
                Color = "info",
                Description = $"Request submitted by {request.SubmittedBy}"
            });

            // Under Review / Assigned
            if (request.AssignedDate.HasValue)
            {
                timeline.Add(new RequestTimelineEvent
                {
                    Status = "Assigned",
                    Date = request.AssignedDate.Value,
                    Icon = "bi-person-check",
                    Color = "primary",
                    Description = $"Assigned to {request.AssignedTo ?? "Team"}"
                });
            }

            // In Progress
            if (request.InProgressDate.HasValue)
            {
                timeline.Add(new RequestTimelineEvent
                {
                    Status = "In Progress",
                    Date = request.InProgressDate.Value,
                    Icon = "bi-tools",
                    Color = "warning",
                    Description = "Work in progress"
                });
            }

            // Completed
            if (request.CompletedDate.HasValue)
            {
                timeline.Add(new RequestTimelineEvent
                {
                    Status = request.Status == RequestStatus.Resolved ? "Resolved" : "Closed",
                    Date = request.CompletedDate.Value,
                    Icon = "bi-check-circle",
                    Color = "success",
                    Description = request.Notes ?? "Request completed"
                });
            }
            // Estimated completion (if not yet completed)
            else if (request.EstimatedCompletionDate.HasValue)
            {
                timeline.Add(new RequestTimelineEvent
                {
                    Status = "Estimated Completion",
                    Date = request.EstimatedCompletionDate.Value,
                    Icon = "bi-calendar-check",
                    Color = "secondary",
                    Description = "Expected completion date"
                });
            }

            return timeline.OrderBy(t => t.Date).ToList();
        }
    }
}
