namespace MunicipalServicesApp.Models.ViewModels
{
    public class ReportIssueViewModel
    {
        public ReportIssue Issue { get; set; } = new ReportIssue();
        public List<string> Categories { get; set; } = new List<string>();
        public List<IFormFile>? Attachments { get; set; }
    }
} // end of file
