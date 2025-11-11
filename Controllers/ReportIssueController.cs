using Microsoft.AspNetCore.Mvc;
using MunicipalServicesApp.Models;
using MunicipalServicesApp.Models.ViewModels;
using MunicipalServicesApp.Services;

namespace MunicipalServicesApp.Controllers
{
    public class ReportIssueController : Controller
    {
        private readonly IReportIssueService _reportIssueService;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ReportIssueController> _logger;

        public ReportIssueController(
            IReportIssueService reportIssueService,
            IWebHostEnvironment environment,
            ILogger<ReportIssueController> logger)
        {
            _reportIssueService = reportIssueService;
            _environment = environment;
            _logger = logger;
        }

        // Shows report issue page
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = new ReportIssueViewModel
            {
                Issue = new ReportIssue(),
                Categories = await _reportIssueService.GetCategoriesAsync()
            };
            return View(viewModel);
        }

        // Handles the report submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ReportIssueViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Reload categories if validation fails
                viewModel.Categories = await _reportIssueService.GetCategoriesAsync();
                return View(viewModel);
            }

            try
            {
                // If the user uploaded files this is where it's delt with
                if (viewModel.Attachments != null && viewModel.Attachments.Count > 0)
                {
                    var attachmentPaths = new List<string>();

                    foreach (var file in viewModel.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                            if (!Directory.Exists(uploadsFolder))
                            {
                                Directory.CreateDirectory(uploadsFolder);
                            }


                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var filePath = Path.Combine(uploadsFolder, fileName);

                            // Saves the file
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            attachmentPaths.Add("/uploads/" + fileName);
                        }
                    }

                    viewModel.Issue.AttachmentPaths = attachmentPaths;
                }

                // Saves derictly to the database
                var issueId = await _reportIssueService.CreateReportAsync(viewModel.Issue);
                
                TempData["SuccessMessage"] = $"Issue reported successfully! Reference number: {issueId}";
                return RedirectToAction("Success", new { id = issueId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting issue report");
                ModelState.AddModelError("", "An error occurred. Please try again.");
                viewModel.Categories = await _reportIssueService.GetCategoriesAsync();
                return View(viewModel);
            }
        }

        // when the user submits a report it shows the success page
        [HttpGet]
        public async Task<IActionResult> Success(int id)
        {
            var issue = await _reportIssueService.GetReportByIdAsync(id);
            
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // View all reports
        [HttpGet]
        public async Task<IActionResult> ViewReports()
        {
            var reports = await _reportIssueService.GetAllReportsAsync();
            return View(reports);
        }
    }
} // end of file
