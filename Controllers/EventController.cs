using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;
using MunicipalServicesApp.Models.ViewModels;
using MunicipalServicesApp.Services;

namespace MunicipalServicesApp.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IEventRsvpService _rsvpService;
        private readonly ILogger<EventController> _logger;

        public EventController(
            IEventService eventService, 
            IEventRsvpService rsvpService,
            ILogger<EventController> logger)
        {
            _eventService = eventService;
            _rsvpService = rsvpService;
            _logger = logger;
        }

        // Main events page
        public async Task<IActionResult> Index()
        {
            // Create view model with event data
            var viewModel = new EventSearchViewModel();
            
            viewModel.Events = await _eventService.GetAllEventsAsync();
            viewModel.Categories = await _eventService.GetCategoriesAsync();
            viewModel.UniqueDates = await _eventService.GetUniqueDatesAsync();
            viewModel.RecommendedEvents = await _eventService.GetRecommendedEventsAsync(null, null);

            return View(viewModel);
        }

        // Handle search page submission
        [HttpPost]
        public async Task<IActionResult> Search(string? category, DateTime? date, string? keyword)
        {
            _eventService.RecordSearchPattern(category, date);

            // Fetches search results
            var searchResults = await _eventService.SearchEventsAsync(category, date, keyword);
            var viewModel = new EventSearchViewModel();
            viewModel.SearchCategory = category;
            viewModel.SearchDate = date;
            viewModel.SearchKeyword = keyword;
            viewModel.Events = searchResults;
            viewModel.Categories = await _eventService.GetCategoriesAsync();
            viewModel.UniqueDates = await _eventService.GetUniqueDatesAsync();
            viewModel.RecommendedEvents = await _eventService.GetRecommendedEventsAsync(category, date);

            return View("Index", viewModel);
        }

        // This gets reccommendations based on category
        [HttpGet]
        public async Task<IActionResult> GetRecommendations(string? category)
        {
            var recommendations = await _eventService.GetRecommendedEventsAsync(category, null);
            return PartialView("_RecommendationsPartial", recommendations);
        }

        // Displays event details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var events = await _eventService.GetAllEventsAsync();
            var evt = events.FirstOrDefault(e => e.Id == id);
            
            if (evt == null)
            {
                return NotFound();
            }

            // This get the amont of RSVP there are
            var rsvpCount = await _rsvpService.GetRsvpCountForEventAsync(id);
            ViewBag.RsvpCount = rsvpCount;
            
            return View(evt);
        }

        // Dispays the RSVP page
        [HttpGet]
        public async Task<IActionResult> Rsvp(int eventId)
        {
            var events = await _eventService.GetAllEventsAsync();
            var evt = events.FirstOrDefault(e => e.Id == eventId);
            
            if (evt == null)
            {
                return NotFound();
            }
            // Creates a new RSVP with event ID for everyone
            var rsvp = new EventRsvp();
            rsvp.EventId = eventId;

            ViewBag.Event = evt;
            return View(rsvp);
        }

        // Handle RSVP page submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rsvp(EventRsvp model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var rsvpId = await _rsvpService.CreateRsvpAsync(model);
                    TempData["SuccessMessage"] = "Your RSVP has been confirmed! We look forward to seeing you at the event.";
                    return RedirectToAction("RsvpConfirmation", new { id = rsvpId });
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating RSVP");
                    ModelState.AddModelError("", "An error occurred while processing your RSVP. Please try again.");
                }
            }

            // If any errors occur then we come back here
            var events = await _eventService.GetAllEventsAsync();
            ViewBag.Event = events.FirstOrDefault(e => e.Id == model.EventId);
            return View(model);
        }

        // This shows the RSVP confirmation page
        [HttpGet]
        public async Task<IActionResult> RsvpConfirmation(int id)
        {
            var rsvp = await _rsvpService.GetRsvpByIdAsync(id);
            
            if (rsvp == null)
            {
                return NotFound();
            }

            return View(rsvp);
        }
    }
}// End of file
