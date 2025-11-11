using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Services
{
    public interface IEventService
    {
        Task<List<LocalEvent>> GetAllEventsAsync();
        Task<List<LocalEvent>> SearchEventsAsync(string? category, DateTime? date, string? keyword);
        Task<List<LocalEvent>> GetRecommendedEventsAsync(string? category, DateTime? date);
        Task<HashSet<string>> GetCategoriesAsync();
        Task<HashSet<DateTime>> GetUniqueDatesAsync();
        Task<int> CreateEventAsync(LocalEvent localEvent);
        void RecordSearchPattern(string? category, DateTime? date);
    }
}// end of file
