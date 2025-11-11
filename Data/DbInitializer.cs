using Microsoft.EntityFrameworkCore;
using MunicipalServicesApp.Data;
using MunicipalServicesApp.Models;

namespace MunicipalServicesApp.Data
{
    public static class DbInitializer
    {
        // This seeded db with events
        public static void SeedData(ApplicationDbContext context)
        {
           
            if (context.Events.Any())
            {
                return; 
            }

            var events = new List<LocalEvent>
            {
                new LocalEvent
                {
                    Title = "Community Clean-Up Day",
                    Description = "Join us for a community-wide clean-up initiative to make our neighborhood beautiful.",
                    EventDate = DateTime.Now.AddDays(7),
                    Category = "Community",
                    Location = "Company's Garden, Cape Town",
                    Priority = 1,
                    CreatedDate = DateTime.Now
                },
                
                new LocalEvent
                {
                    Title = "Gun Violence Awareness Workshop",
                    Description = "Educational workshop on gun violence prevention, safe storage, and community safety initiatives. Open to all community members.",
                    EventDate = DateTime.Now.AddDays(10),
                    Category = "Community",
                    Location = "Mitchell's Plain Community Centre, Cape Town",
                    Priority = 1,
                    CreatedDate = DateTime.Now
                },

                new LocalEvent
                {
                    Title = "Anti-Gangsterism Youth Program Launch",
                    Description = "Launch of a comprehensive anti-gangsterism program providing mentorship, skills training, and support for at-risk youth in our community.",
                    EventDate = DateTime.Now.AddDays(5),
                    Category = "Community",
                    Location = "Hanover Park Youth Centre, Cape Town",
                    Priority = 1,
                    CreatedDate = DateTime.Now
                },
                
                new LocalEvent
                {
                    Title = "Community Feeding Scheme Registration",
                    Description = "Register for our weekly community feeding scheme providing nutritious meals to families in need. Volunteers also welcome.",
                    EventDate = DateTime.Now.AddDays(3),
                    Category = "Community",
                    Location = "Khayelitsha Community Centre, Cape Town",
                    Priority = 1,
                    CreatedDate = DateTime.Now
                },
                
                new LocalEvent
                {
                    Title = "Town Hall Meeting",
                    Description = "Quarterly town hall meeting to discuss municipal developments and answer questions.",
                    EventDate = DateTime.Now.AddDays(14),
                    Category = "Government",
                    Location = "Cape Town City Hall",
                    Priority = 2,
                    CreatedDate = DateTime.Now
                },
                
                new LocalEvent
                {
                    Title = "Community Safety Forum",
                    Description = "Open forum to discuss community safety concerns with police and fire departments.",
                    EventDate = DateTime.Now.AddDays(17),
                    Category = "Government",
                    Location = "Manenberg Community Hall, Cape Town",
                    Priority = 1,
                    CreatedDate = DateTime.Now
                },
                
                new LocalEvent
                {
                    Title = "Water Conservation Workshop",
                    Description = "Learn practical tips and techniques for water conservation in your home.",
                    EventDate = DateTime.Now.AddDays(15),
                    Category = "Environment",
                    Location = "Green Point Community Centre, Cape Town",
                    Priority = 1,
                    CreatedDate = DateTime.Now
                },
                
                new LocalEvent
                {
                    Title = "Tree Planting Initiative",
                    Description = "Community tree planting event to increase urban forest coverage. Volunteers needed.",
                    EventDate = DateTime.Now.AddDays(13),
                    Category = "Environment",
                    Location = "Table Mountain National Park, Cape Town",
                    Priority = 1,
                    CreatedDate = DateTime.Now
                },
                
                new LocalEvent
                {
                    Title = "Free Health Screening",
                    Description = "Free health screenings for all residents. No appointment necessary.",
                    EventDate = DateTime.Now.AddDays(21),
                    Category = "Health",
                    Location = "Gugulethu Community Health Centre, Cape Town",
                    Priority = 3,
                    CreatedDate = DateTime.Now
                },
                
                new LocalEvent
                {
                    Title = "Mental Health Awareness Seminar",
                    Description = "Free seminar on mental health awareness and available community resources.",
                    EventDate = DateTime.Now.AddDays(22),
                    Category = "Health",
                    Location = "Groote Schuur Hospital Auditorium, Cape Town",
                    Priority = 1,
                    CreatedDate = DateTime.Now
                },
                
                new LocalEvent
                {
                    Title = "Road Maintenance Notice",
                    Description = "Scheduled road maintenance on Main Road. Expect delays.",
                    EventDate = DateTime.Now.AddDays(4),
                    Category = "Infrastructure",
                    Location = "Main Road, Observatory, Cape Town",
                    Priority = 2,
                    CreatedDate = DateTime.Now
                },
                
                new LocalEvent
                {
                    Title = "Youth Sports Festival",
                    Description = "Annual youth sports festival featuring various sports activities and competitions.",
                    EventDate = DateTime.Now.AddDays(30),
                    Category = "Sports",
                    Location = "Athlone Stadium, Cape Town",
                    Priority = 1,
                    CreatedDate = DateTime.Now
                },
                
                new LocalEvent
                {
                    Title = "Public Library Book Fair",
                    Description = "Annual book fair with thousands of books at discounted prices. Proceeds support library programs.",
                    EventDate = DateTime.Now.AddDays(25),
                    Category = "Education",
                    Location = "Cape Town Library",
                    Priority = 3,
                    CreatedDate = DateTime.Now
                },
                
                new LocalEvent
                {
                    Title = "Children's Summer Reading Program",
                    Description = "Free summer reading program for children ages 5-12 with weekly activities and prizes.",
                    EventDate = DateTime.Now.AddDays(45),
                    Category = "Education",
                    Location = "Wynberg Public Library, Cape Town",
                    Priority = 2,
                    CreatedDate = DateTime.Now
                },
                
                new LocalEvent
                {
                    Title = "Cultural Heritage Festival",
                    Description = "Celebrate our diverse community with food, music, and cultural performances.",
                    EventDate = DateTime.Now.AddDays(40),
                    Category = "Culture",
                    Location = "Grand Parade, Cape Town",
                    Priority = 1,
                    CreatedDate = DateTime.Now
                },
                
                new LocalEvent
                {
                    Title = "Summer Concert Series Kickoff",
                    Description = "Free outdoor concert series begins! Bring blankets and chairs for a night of live music.",
                    EventDate = DateTime.Now.AddDays(50),
                    Category = "Culture",
                    Location = "Kirstenbosch National Botanical Garden, Cape Town",
                    Priority = 2,
                    CreatedDate = DateTime.Now
                }
            };

            context.Events.AddRange(events);
            context.SaveChanges();
        }
    }
}// End of file
