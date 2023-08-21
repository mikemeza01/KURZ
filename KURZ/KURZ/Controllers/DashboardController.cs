using Microsoft.AspNetCore.Mvc;

namespace KURZ.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult DashboardAdmin()
        {
            return View();
        }
    }
}
