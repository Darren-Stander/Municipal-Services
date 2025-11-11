using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Services
{
    public interface IEventRsvpService
    {
        Task<int> CreateRsvpAsync(EventRsvp rsvp);
        Task<List<EventRsvp>> GetRsvpsForEventAsync(int eventId);
        Task<bool> HasUserRsvpedAsync(int eventId, string cellPhoneNumber);
        Task<int> GetRsvpCountForEventAsync(int eventId);
        Task<EventRsvp?> GetRsvpByIdAsync(int rsvpId);
        Task<bool> CancelRsvpAsync(int rsvpId);
    }
}// end of file
