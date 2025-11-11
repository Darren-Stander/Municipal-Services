using Microsoft.EntityFrameworkCore;
using MunicipalServicesApp.Data;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Services
{
    public class ReportIssueService : IReportIssueService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

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

        public ReportIssueService(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<int> CreateReportAsync(ReportIssue issue)
        {
            issue.ReportedDate = DateTime.Now;
            issue.Status = "Pending";
            _context.ReportIssues.Add(issue);
            await _context.SaveChangesAsync();
            return issue.Id;
        }

        public async Task<ReportIssue?> GetReportByIdAsync(int id)
        {
            return await _context.ReportIssues.FindAsync(id);
        }

        public async Task<List<ReportIssue>> GetAllReportsAsync()
        {
            return await _context.ReportIssues
                .OrderByDescending(r => r.ReportedDate)
                .ToListAsync();
        }

        public Task<List<string>> GetCategoriesAsync()
        {
            return Task.FromResult(_categories);
        }

        public async Task<bool> SaveAttachmentsAsync(int reportId, List<IFormFile> files)
        {
            try
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "issues");
                Directory.CreateDirectory(uploadsFolder);

                var attachmentPaths = new List<string>();

                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var uniqueFileName = $"{reportId}_{Guid.NewGuid()}_{file.FileName}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        attachmentPaths.Add($"/uploads/issues/{uniqueFileName}");
                    }
                }

                var report = await GetReportByIdAsync(reportId);
                if (report != null)
                {
                    report.AttachmentPaths.AddRange(attachmentPaths);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}// end of file
