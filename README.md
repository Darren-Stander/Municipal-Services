# Municipal Services Application
## YouTube link:
https://www.youtube.com/watch?v=1dKDfV78A9U
## Features

### 1. **Report Issues**
- Citizens can report municipal issues (potholes, water leaks, streetlights, etc.)
- Support for multiple categories
- File attachment support (images, documents)
- Issue tracking with status updates

### 2. **Local Events and Announcements**
- View upcoming local events
- **Event RSVP System**:
  - Citizens can RSVP for events using name, surname, and cell phone
  - Real-time RSVP count display
  - Confirmation page with event details
  - Duplicate RSVP prevention
- Advanced search functionality:
  - Search by category
  - Search by date
  - Keyword search
- Advanced data structures implementation:
  - **SortedDictionary**: Events organized by date
  - **Dictionary**: Fast category-based lookup
  - **HashSet**: Unique categories and dates
  - **Stack**: Search history tracking
  - **Queue**: Priority event management
- **Smart Recommendations**:
  - Based on user search patterns
  - Category-based suggestions
  - Priority event highlighting

### 3. **Service Request Status**
- Feature placeholder (to be implemented)

### 4. **Admin Panel**
- Secure admin authentication
- Create and manage events
- **View Event RSVPs**:
  - See who has RSVP'd for each event
  - Export RSVP lists to CSV
  - Print attendee lists
  - Real-time RSVP tracking
- Advanced sorting capabilities:
  - Sort by date (ascending/descending)
  - Sort by Name
  - Sort by category
  - Sort by priority
- View all events with detailed information

## Tech Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQLite with Entity Framework Core
- **Frontend**: Bootstrap 5 with Bootstrap Icons
- **Session Management**: ASP.NET Core Session for admin authentication

## Database Structure

### Events Table
- Id (Primary Key)
- Title
- Description
- EventDate
- Category
- Location
- ImageUrl
- Priority (1-3)
- CreatedDate

### EventRsvps Table
- Id (Primary Key)
- EventId (Foreign Key)
- FirstName
- LastName
- CellPhoneNumber
- Email (Optional)
- RsvpDate

### ReportIssues Table
- Id (Primary Key)
- Location
- Category
- Description
- AttachmentPathsString (comma-separated)
- ReportedDate
- Status

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
2. Navigate to the project directory
3. Restore packages:
4. Run database migrations:
5. Run the application:

## Admin Access

**Admin Credentials:**
- Username: admin
- Password: admin123

## Admin Features

### Access Admin Panel
1. Click "Admin Login" on the home page
2. Enter credentials
3. Access the Admin Dashboard

### Create Events
1. Navigate to Dashboard ? Create Event
2. Fill in event details:
   - Title
   - Description
   - Date & Time
   - Category
   - Location
   - Priority (1-3, where 1 is highest)
   - Optional: Image URL

### Manage Events
1. Navigate to Dashboard ? Manage Events
2. Use sorting buttons to organize events:
   - Sort by Date (Asc/Desc)
   - Sort by Title (A-Z/Z-A)
   - Sort by Category
   - Sort by Priority

### View All Events
- See complete list of all events in card format
- View event details, priority, and creation date

## User Engagement Strategy

The Municipality application implements several engagement strategies:

1. **Easy Issue Reporting**: Simple form with category selection and file uploads
2. **Event RSVP System**: Simple registration with name and phone number
3. **Visual Event Display**: Aesthetically pleasing event cards with color coding
4. **Event Details Pages**: Comprehensive event information with RSVP counts
5. **Smart Search**: Multiple search options for finding relevant events
6. **Personalized Recommendations**: AI-powered event suggestions based on search patterns
7. **Priority Highlighting**: Important events are prominently displayed
8. **Community Impact Messaging**: Encourages active participation
9. **RSVP Confirmations**: Immediate confirmation after registration

## Data Structures Implementation

### Advanced Data Structures Used:

1. **SortedDictionary**: 
   - Organizes events by date for efficient chronological access
   - Automatically maintains sorted order

2. **Dictionary**:
   - Fast category-based event lookup
   - Optimizes search performance

3. **HashSet** (Categories & Dates):
   - Stores unique categories
   - Provides lookup for filtering

4. **Stack**:
   - Tracks search history
   - Enables recommendation algorithm

5. **Queue**:
   - Manages priority events
   - First-in-first-out processing

6. **Dictionary**:
   - Tracks category search frequency
   - Powers recommendation engine

## File Upload Support

### Supported File Types:
- Images: .jpeg, .png
- Documents: .pdf, .doc,

## Search & Filter Features

### Event Search:
- **Category Filter**: Filter by event category
- **Date Filter**: Find events on specific dates
- **Keyword Search**: Search in title, description, and location
- **Combined Filters**: Use multiple filters simultaneously

### Recommendation Algorithm:
1. Tracks user search patterns using Stack
2. Counts category searches using Dictionary
3. Suggests events from most-searched categories
4. Includes high-priority upcoming events
5. Removes duplicates and limits to top 6 recommendations

## Database Management

### Connection String
Located in appsettings.json

### Seed Data
- Initial events are automatically seeded on first run
- See Data/DbInitializer.cs for seed data

### Migrations
To create new migrations:
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```
## References 
- https://www.youtube.com/watch?v=mxgyZmQ-Krc&list=PL2Q8rFbm-4ruplp2SRUTQjZaFfxh-knS0
- https://www.youtube.com/watch?v=xuFdrXqpPB0&t=22s
- https://www.youtube.com/watch?v=_5X3DkhQlG4
- https://www.youtube.com/watch?v=zsjE2F6p9ig
- https://www.youtube.com/watch?v=HXt-q8bMeP4
- https://www.youtube.com/watch?v=O9v10jQkm5c
- https://www.youtube.com/watch?v=IPpEefuFiVM
- https://www.youtube.com/watch?v=2sp4gWCq3o4


