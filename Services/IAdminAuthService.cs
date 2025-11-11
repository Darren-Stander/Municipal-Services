namespace MunicipalServicesApp.Services
{
    public interface IAdminAuthService
    {
        bool ValidateAdmin(string username, string password);
        bool IsAdmin(HttpContext context);
        void SetAdminSession(HttpContext context);
        void ClearAdminSession(HttpContext context);
    }
}// end of file
