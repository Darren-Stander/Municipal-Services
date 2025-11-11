using Microsoft.AspNetCore.Mvc;

namespace MunicipalServicesApp.Controllers
{
    public class RequestStatusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
