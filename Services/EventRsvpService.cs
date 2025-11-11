using Microsoft.EntityFrameworkCore;
using MunicipalServicesApp.Data;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Services
{
    public class EventRsvpService : IEventRsvpService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EventRsvpService> _logger;

        public EventRsvpService(ApplicationDbContext context, ILogger<EventRsvpService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Create a new RSVP
        public async Task<int> CreateRsvpAsync(EventRsvp rsvp)
        {
            var existingRsvp = await _context.EventRsvps
                .FirstOrDefaultAsync(r => r.EventId == rsvp.EventId && r.CellPhoneNumber == rsvp.CellPhoneNumber);

            if (existingRsvp != null)
            {
                throw new InvalidOperationException("You have already RSVP'd to this event with this phone number.");
            }

            rsvp.RsvpDate = DateTime.Now;

            _context.EventRsvps.Add(rsvp);
            await _context.SaveChangesAsync();

            return rsvp.Id;
        }

        public async Task<List<EventRsvp>> GetRsvpsForEventAsync(int eventId)
        {
            var rsvps = await _context.EventRsvps
                .Where(r => r.EventId == eventId)
                .Include(r => r.Event)
                .ToListAsync();

            // Sort by RSVP date (newest first)
            rsvps = rsvps.OrderByDescending(r => r.RsvpDate).ToList();

            return rsvps;
        }
        public async Task<bool> HasUserRsvpedAsync(int eventId, string cellPhoneNumber)
        {
            var rsvp = await _context.EventRsvps
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.CellPhoneNumber == cellPhoneNumber);

            if (rsvp != null)
                return true;
            else
                return false;
        }

        public async Task<int> GetRsvpCountForEventAsync(int eventId)
        {
            var count = await _context.EventRsvps
                .Where(r => r.EventId == eventId)
                .CountAsync();

            return count;
        }

        // Get a specific RSVP by ID
        public async Task<EventRsvp?> GetRsvpByIdAsync(int rsvpId)
        {
            var rsvp = await _context.EventRsvps
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.Id == rsvpId);

            return rsvp;
        }

        public async Task<bool> CancelRsvpAsync(int rsvpId)
        {
            var rsvp = await _context.EventRsvps.FindAsync(rsvpId);

            if (rsvp == null)
            {
                return false;
            }

            _context.EventRsvps.Remove(rsvp);
            await _context.SaveChangesAsync();

            return true;
        }

        // This is where the CSV export functionality
        public async Task<string> ExportRsvpsToCsvAsync(int eventId)
        {
            var rsvps = await GetRsvpsForEventAsync(eventId);

            var csv = new System.Text.StringBuilder();
            csv.AppendLine("First Name,Last Name,Cell Phone,Email,RSVP Date");

            foreach (var rsvp in rsvps)
            {
                var email = string.IsNullOrEmpty(rsvp.Email) ? "N/A" : rsvp.Email;
                var line = $"{rsvp.FirstName},{rsvp.LastName},{rsvp.CellPhoneNumber},{email},{rsvp.RsvpDate:yyyy-MM-dd HH:mm}";
                csv.AppendLine(line);
            }

            return csv.ToString();
        }
    }
}// end of file
