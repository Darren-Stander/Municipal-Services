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
     // Seed Events
      if (!context.Events.Any())
         {
       SeedEvents(context);
   }

            // Seed Service Requests
   if (!context.ServiceRequests.Any())
    {
      SeedServiceRequests(context);
    }
        }

        private static void SeedEvents(ApplicationDbContext context)
{
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

        private static void SeedServiceRequests(ApplicationDbContext context)
        {
            var baseDate = DateTime.Now;
            var serviceRequests = new List<ServiceRequest>
  {
   new ServiceRequest
 {
          RequestNumber = $"REQ-{DateTime.Now.Year}-00001",
     Title = "Pothole on Main Street",
  Description = "Large pothole causing traffic issues and potential vehicle damage near the intersection of Main Street and 5th Avenue.",
     Category = "Road Maintenance",
Location = "Main Street & 5th Avenue, Cape Town",
Status = RequestStatus.InProgress,
    Priority = RequestPriority.High,
  Department = "Public Works",
  SubmittedDate = baseDate.AddDays(-15),
     AssignedDate = baseDate.AddDays(-14),
     InProgressDate = baseDate.AddDays(-12),
   EstimatedCompletionDate = baseDate.AddDays(2),
        AssignedTo = "Road Crew Team A",
  SubmittedBy = "John Smith"
      },
   new ServiceRequest
   {
       RequestNumber = $"REQ-{DateTime.Now.Year}-00002",
   Title = "Street Light Out",
  Description = "Street light pole #4521 has been out for three days, creating safety concerns in the neighborhood.",
         Category = "Street Lighting",
     Location = "Oak Avenue, Mitchell's Plain",
       Status = RequestStatus.Assigned,
 Priority = RequestPriority.Medium,
 Department = "Electricity Department",
 SubmittedDate = baseDate.AddDays(-3),
 AssignedDate = baseDate.AddDays(-2),
 EstimatedCompletionDate = baseDate.AddDays(5),
      AssignedTo = "Electrician Team B",
      SubmittedBy = "Sarah Johnson"
      },
          new ServiceRequest
 {
  RequestNumber = $"REQ-{DateTime.Now.Year}-00003",
   Title = "Water Main Break",
  Description = "Emergency: Water main break flooding the street and affecting multiple properties.",
     Category = "Water & Sanitation",
      Location = "Hanover Park, 3rd Street",
 Status = RequestStatus.Resolved,
 Priority = RequestPriority.Critical,
    Department = "Water Services",
    SubmittedDate = baseDate.AddDays(-5),
  AssignedDate = baseDate.AddDays(-5),
       InProgressDate = baseDate.AddDays(-5),
  CompletedDate = baseDate.AddDays(-4),
   EstimatedCompletionDate = baseDate.AddDays(-5),
  AssignedTo = "Emergency Water Team",
     SubmittedBy = "City Operations Center",
       Notes = "Emergency repair completed successfully"
       },
      new ServiceRequest
     {
 RequestNumber = $"REQ-{DateTime.Now.Year}-00004",
Title = "Missed Garbage Collection",
        Description = "Garbage has not been collected for two weeks in our area. Bins are overflowing.",
 Category = "Waste Management",
      Location = "Khayelitsha Community, Block C",
     Status = RequestStatus.UnderReview,
Priority = RequestPriority.High,
Department = "Sanitation Services",
  SubmittedDate = baseDate.AddDays(-2),
EstimatedCompletionDate = baseDate.AddDays(3),
SubmittedBy = "Community Representative"
     },
    new ServiceRequest
    {
      RequestNumber = $"REQ-{DateTime.Now.Year}-00005",
Title = "Park Equipment Damaged",
Description = "Playground equipment in the park is damaged and unsafe for children.",
       Category = "Parks & Recreation",
     Location = "Green Point Community Park",
 Status = RequestStatus.Submitted,
              Priority = RequestPriority.Medium,
  Department = "Parks and Recreation",
  SubmittedDate = baseDate.AddDays(-1),
EstimatedCompletionDate = baseDate.AddDays(7),
  SubmittedBy = "Parent Group"
  },
new ServiceRequest
{
 RequestNumber = $"REQ-{DateTime.Now.Year}-00006",
    Title = "Traffic Signal Malfunction",
    Description = "Traffic light at busy intersection is stuck on red in all directions causing major delays.",
Category = "Traffic Signals",
    Location = "Main Road & Victoria Street, Observatory",
Status = RequestStatus.InProgress,
  Priority = RequestPriority.Critical,
       Department = "Transportation",
      SubmittedDate = baseDate.AddDays(-1),
   AssignedDate = baseDate.AddDays(-1),
      InProgressDate = baseDate.AddDays(-1),
      EstimatedCompletionDate = baseDate,
 AssignedTo = "Signal Maintenance Team",
SubmittedBy = "Traffic Control"
     },
    new ServiceRequest
{
  RequestNumber = $"REQ-{DateTime.Now.Year}-00007",
Title = "Illegal Dumping",
Description = "Large amount of construction debris dumped illegally in empty lot.",
    Category = "Waste Management",
    Location = "Manenberg, Vacant Lot on 2nd Avenue",
     Status = RequestStatus.Assigned,
     Priority = RequestPriority.Medium,
      Department = "Sanitation Services",
   SubmittedDate = baseDate.AddDays(-4),
  AssignedDate = baseDate.AddDays(-3),
    EstimatedCompletionDate = baseDate.AddDays(4),
      AssignedTo = "Waste Removal Team C",
           SubmittedBy = "Neighborhood Watch"
       },
new ServiceRequest
{
      RequestNumber = $"REQ-{DateTime.Now.Year}-00008",
  Title = "Broken Water Pipe",
  Description = "Water pipe leaking on property causing water wastage and potential foundation damage.",
          Category = "Water & Sanitation",
Location = "Gugulethu, 15 Rainbow Street",
 Status = RequestStatus.Submitted,
Priority = RequestPriority.High,
   Department = "Water Services",
      SubmittedDate = baseDate.AddDays(-1),
EstimatedCompletionDate = baseDate.AddDays(3),
            SubmittedBy = "Homeowner"
},
    new ServiceRequest
    {
 RequestNumber = $"REQ-{DateTime.Now.Year}-00009",
       Title = "Graffiti Removal Request",
      Description = "Offensive graffiti on public building needs removal.",
Category = "Public Safety",
    Location = "Wynberg Library, Cape Town",
Status = RequestStatus.Closed,
       Priority = RequestPriority.Low,
      Department = "General Services",
     SubmittedDate = baseDate.AddDays(-10),
           AssignedDate = baseDate.AddDays(-9),
InProgressDate = baseDate.AddDays(-8),
      CompletedDate = baseDate.AddDays(-7),
 EstimatedCompletionDate = baseDate.AddDays(-8),
   AssignedTo = "Maintenance Crew",
 SubmittedBy = "Library Staff",
       Notes = "Graffiti removed and wall repainted"
  },
   new ServiceRequest
      {
   RequestNumber = $"REQ-{DateTime.Now.Year}-00010",
      Title = "Pothole Cluster on Oak Street",
Description = "Multiple potholes on Oak Street causing vehicle damage.",
   Category = "Road Maintenance",
  Location = "Oak Street, Mitchell's Plain",
    Status = RequestStatus.UnderReview,
             Priority = RequestPriority.High,
 Department = "Public Works",
      SubmittedDate = baseDate.AddDays(-6),
  EstimatedCompletionDate = baseDate.AddDays(5),
     SubmittedBy = "Local Resident"
   },
  new ServiceRequest
   {
 RequestNumber = $"REQ-{DateTime.Now.Year}-00011",
    Title = "Power Outage",
Description = "Multiple homes without electricity for 6 hours.",
  Category = "Electricity",
Location = "Kirstenbosch Area, Cape Town",
      Status = RequestStatus.Resolved,
Priority = RequestPriority.Critical,
Department = "Electricity Department",
     SubmittedDate = baseDate.AddDays(-2),
   AssignedDate = baseDate.AddDays(-2),
  InProgressDate = baseDate.AddDays(-2),
CompletedDate = baseDate.AddDays(-2),
      EstimatedCompletionDate = baseDate.AddDays(-2),
AssignedTo = "Power Grid Team",
 SubmittedBy = "Multiple Residents",
  Notes = "Transformer repaired and power restored"
   },
      new ServiceRequest
      {
     RequestNumber = $"REQ-{DateTime.Now.Year}-00012",
Title = "Noise Complaint - Construction",
Description = "Construction site operating outside permitted hours, disturbing residents.",
Category = "Noise Complaint",
     Location = "Grand Parade Area, Cape Town",
Status = RequestStatus.Assigned,
     Priority = RequestPriority.Low,
 Department = "Safety and Security",
 SubmittedDate = baseDate.AddDays(-3),
     AssignedDate = baseDate.AddDays(-2),
      EstimatedCompletionDate = baseDate.AddDays(2),
        AssignedTo = "Code Enforcement Officer",
SubmittedBy = "Apartment Residents"
      }
            };

            context.ServiceRequests.AddRange(serviceRequests);
      context.SaveChanges();
        }
    }
}// End of file
