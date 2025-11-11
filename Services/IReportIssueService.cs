using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Services
{
    public interface IReportIssueService
    {
        Task<int> CreateReportAsync(ReportIssue issue);
        Task<ReportIssue?> GetReportByIdAsync(int id);
        Task<List<ReportIssue>> GetAllReportsAsync();
        Task<List<string>> GetCategoriesAsync();
        Task<bool> SaveAttachmentsAsync(int reportId, List<IFormFile> files);
    }
}// end of file
