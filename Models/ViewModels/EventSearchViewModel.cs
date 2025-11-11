namespace MunicipalServicesApp.Models.ViewModels
{
    public class EventSearchViewModel
    {
        public string? SearchCategory { get; set; }
        public DateTime? SearchDate { get; set; }
        public string? SearchKeyword { get; set; }
        public List<LocalEvent> Events { get; set; } = new List<LocalEvent>();
        public List<LocalEvent> RecommendedEvents { get; set; } = new List<LocalEvent>();
        public HashSet<string> Categories { get; set; } = new HashSet<string>();
        public HashSet<DateTime> UniqueDates { get; set; } = new HashSet<DateTime>();
    }
}// end of file
