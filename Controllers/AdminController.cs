using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;
using MunicipalServicesApp.Services;

namespace MunicipalServicesApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminAuthService _adminAuthService;
        private readonly IEventService _eventService;
        private readonly IEventRsvpService _rsvpService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IAdminAuthService adminAuthService, 
            IEventService eventService,
            IEventRsvpService rsvpService,
            ILogger<AdminController> logger)
        {
            _adminAuthService = adminAuthService;
            _eventService = eventService;
            _rsvpService = rsvpService;
            _logger = logger;
        }

        // Show login page
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // Handles login page submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AdminLogin model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (_adminAuthService.ValidateAdmin(model.Username, model.Password))
                {
                    _adminAuthService.SetAdminSession(HttpContext);
                    
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    
                    return RedirectToAction("Dashboard");
                }

                ModelState.AddModelError("", "Invalid username or password");
            }

            return View(model);
        }

        // admin logout happens here
        public IActionResult Logout()
        {
            _adminAuthService.ClearAdminSession(HttpContext);
            return RedirectToAction("Index", "Home");
        }

        // Admin dashboard page
        public IActionResult Dashboard()
        {
            if (!_adminAuthService.IsAdmin(HttpContext))
            {
                return RedirectToAction("Login", new { returnUrl = "/Admin/Dashboard" });
            }

            return View();
        }

        // Manage events page
        [HttpGet]
        public IActionResult ManageEvents()
        {
            if (!_adminAuthService.IsAdmin(HttpContext))
            {
                return RedirectToAction("Login", new { returnUrl = "/Admin/ManageEvents" });
            }

            return View();
        }

        // Show create event page
        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (!_adminAuthService.IsAdmin(HttpContext))
            {
                return RedirectToAction("Login", new { returnUrl = "/Admin/CreateEvent" });
            }

            return View(new LocalEvent());
        }

        // Takes care of the create event page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(LocalEvent model)
        {
            if (!_adminAuthService.IsAdmin(HttpContext))
            {
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var eventId = await _eventService.CreateEventAsync(model);
                    TempData["SuccessMessage"] = $"Event created successfully! Event ID: {eventId}";
                    return RedirectToAction("ManageEvents");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating event");
                    ModelState.AddModelError("", "An error occurred while creating the event.");
                }
            }

            return View(model);
        }

        // User can view all events
        [HttpGet]
        public async Task<IActionResult> ViewAllEvents()
        {
            if (!_adminAuthService.IsAdmin(HttpContext))
            {
                return RedirectToAction("Login", new { returnUrl = "/Admin/ViewAllEvents" });
            }

            var events = await _eventService.GetAllEventsAsync();
            return View(events);
        }

        // Sort events by different criteria
        [HttpPost]
        public async Task<IActionResult> SortEvents(string sortBy = "date", string order = "asc")
        {
            if (!_adminAuthService.IsAdmin(HttpContext))
            {
                return RedirectToAction("Login");
            }

            var events = await _eventService.GetAllEventsAsync();

            // Based on whatever the user selected, sort the events
            if (sortBy.ToLower() == "title")
            {
                if(order == "asc")
                    events = events.OrderBy(e => e.Title).ToList();
                else
                    events = events.OrderByDescending(e => e.Title).ToList();
            }
            else if(sortBy.ToLower() == "category")
            {
                if(order == "asc")
                    events = events.OrderBy(e => e.Category).ToList();
                else
                    events = events.OrderByDescending(e => e.Category).ToList();
            }
            else if(sortBy.ToLower() == "priority")
            {
                if(order == "asc")
                    events = events.OrderBy(e => e.Priority).ToList();
                else
                    events = events.OrderByDescending(e => e.Priority).ToList();
            }
            else 
            {
                if(order == "asc")
                    events = events.OrderBy(e => e.EventDate).ToList();
                else
                    events = events.OrderByDescending(e => e.EventDate).ToList();
            }

            return PartialView("_EventListPartial", events);
        }

        // View RSVPs for a specific event
        [HttpGet]
        public async Task<IActionResult> ViewEventRsvps(int eventId)
        {
            if (!_adminAuthService.IsAdmin(HttpContext))
            {
                return RedirectToAction("Login", new { returnUrl = $"/Admin/ViewEventRsvps?eventId={eventId}" });
            }

            // Get the event details
            var events = await _eventService.GetAllEventsAsync();
            var evt = events.FirstOrDefault(e => e.Id == eventId);
            
            if (evt == null)
            {
                return NotFound();
            }

            // Get RSVPs for this event
            var rsvps = await _rsvpService.GetRsvpsForEventAsync(eventId);
            
            ViewBag.Event = evt;
            return View(rsvps);
        }
    }
} // end of file
