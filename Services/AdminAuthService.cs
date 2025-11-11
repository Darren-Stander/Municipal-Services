namespace MunicipalServicesApp.Services
{
    public class AdminAuthService : IAdminAuthService
    {
        // Simple hardcoded credentials for admin
        // TODO: In production should use proper authentication with database
        private const string ADMIN_USERNAME = "admin";
        private const string ADMIN_PASSWORD = "admin123";
        private const string SESSION_KEY = "IsAdmin";

        // Check if username and password are correct
        public bool ValidateAdmin(string username, string password)
        {
            // Basic check - compare username and password
            if(username == ADMIN_USERNAME && password == ADMIN_PASSWORD)
            {
                return true;
            }
            return false;
        }

        // Set admin session when logged in
        public void SetAdminSession(HttpContext context)
        {
            context.Session.SetString(SESSION_KEY, "true");
        }

        // Clear admin session when logged out
        public void ClearAdminSession(HttpContext context)
        {
            context.Session.Remove(SESSION_KEY);
        }

        // Check if current user is admin
        public bool IsAdmin(HttpContext context)
        {
            var sessionValue = context.Session.GetString(SESSION_KEY);
            
            if(sessionValue == "true")
            {
                return true;
            }
            
            return false;
        }
    }
}// end of file
