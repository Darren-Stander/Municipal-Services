using Microsoft.EntityFrameworkCore;
using MunicipalServicesApp.Data;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;


        private readonly HashSet<string> _categories = new();
        private readonly HashSet<DateTime> _uniqueDates = new();
        private readonly Stack<(string? category, DateTime? date)> _searchHistory = new();
        private readonly Dictionary<string, int> _categorySearchCount = new();

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all events from database
        public async Task<List<LocalEvent>> GetAllEventsAsync()
        {
            var events = await _context.Events.ToListAsync();
            events = events.OrderBy(e => e.EventDate).ToList();
            return events;
        }

        // Search for events based on filters
        public async Task<List<LocalEvent>> SearchEventsAsync(string? category, DateTime? date, string? keyword)
        {
            var allEvents = await _context.Events.ToListAsync();
            var results = new List<LocalEvent>();

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                foreach (var evt in allEvents)
                {
                    if (evt.Category == category)
                    {
                        results.Add(evt);
                    }
                }
                allEvents = results;
                results = new List<LocalEvent>();
            }

            // Filter by date if provided
            if (date.HasValue)
            {
                var searchDate = date.Value.Date;
                foreach (var evt in allEvents)
                {
                    if (evt.EventDate.Date == searchDate)
                    {
                        results.Add(evt);
                    }
                }
                allEvents = results;
                results = new List<LocalEvent>();
            }

            // Filter by keyword
            if (!string.IsNullOrEmpty(keyword))
            {
                foreach (var evt in allEvents)
                {
                    if (evt.Title.Contains(keyword) ||
                       evt.Description.Contains(keyword) ||
                       evt.Location.Contains(keyword))
                    {
                        results.Add(evt);
                    }
                }
                allEvents = results;
            }

            // Sort results by date
            allEvents.Sort((a, b) => a.EventDate.CompareTo(b.EventDate));

            return allEvents;
        }

        // This is where the recommendation algorithm method is created
        public async Task<List<LocalEvent>> GetRecommendedEventsAsync(string? category, DateTime? date)
        {
            var recommendations = new List<LocalEvent>();
            var topCategories = _categorySearchCount
                .OrderByDescending(kvp => kvp.Value)
                .Take(3)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var cat in topCategories)
            {
                var events = await _context.Events
                    .Where(e => e.Category == cat && e.EventDate >= DateTime.Now)
                    .Take(2)
                    .ToListAsync();
                recommendations.AddRange(events);
            }

            // Add similar events if category is selected
            if (!string.IsNullOrEmpty(category))
            {
                var categoryEvents = await _context.Events
                    .Where(e => e.Category == category && e.EventDate >= DateTime.Now)
                    .Take(3)
                    .ToListAsync();
                recommendations.AddRange(categoryEvents);
            }

            // Add high priority events
            var priorityEvents = await _context.Events
                .Where(e => e.Priority <= 2 && e.EventDate >= DateTime.Now)
                .Take(3)
                .ToListAsync();
            recommendations.AddRange(priorityEvents);
            var uniqueRecommendations = recommendations
                .GroupBy(e => e.Id)
                .Select(g => g.First())
                .OrderBy(e => e.EventDate)
                .Take(6)
                .ToList();

            return uniqueRecommendations;
        }

        // Get list of unique categories
        public async Task<HashSet<string>> GetCategoriesAsync()
        {
            var events = await _context.Events.ToListAsync();
            var categories = new HashSet<string>();
            foreach (var evt in events)
            {
                if (!string.IsNullOrEmpty(evt.Category))
                {
                    categories.Add(evt.Category);
                }
            }

            return categories;
        }

        // Get unique event dates
        public async Task<HashSet<DateTime>> GetUniqueDatesAsync()
        {
            var events = await _context.Events.ToListAsync();
            var dates = new HashSet<DateTime>();
            foreach (var evt in events)
            {
                dates.Add(evt.EventDate.Date);
            }

            return dates;
        }

        // Creates a new event
        public async Task<int> CreateEventAsync(LocalEvent localEvent)
        {
            localEvent.CreatedDate = DateTime.Now;
            _context.Events.Add(localEvent);
            await _context.SaveChangesAsync();

            return localEvent.Id;
        }

        // Tracks search patterns for recommendations
        public void RecordSearchPattern(string? category, DateTime? date)
        {
            _searchHistory.Push((category, date));

            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                if (_categorySearchCount.ContainsKey(category))
                {
                    _categorySearchCount[category]++;
                }
                else
                {
                    _categorySearchCount[category] = 1;
                }
            }

            // This method is used to trim the stack logic

            if (_searchHistory.Count > 50)
            {
                var tempList = _searchHistory.Take(30).ToList();
                _searchHistory.Clear();
                foreach (var item in tempList.Reverse<(string?, DateTime?)>())
                {
                    _searchHistory.Push(item);
                }
            }
        }
    }
}// end of file
