using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KURZ.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize(Roles = "Administrator")]
        public IActionResult DashboardAdmin()
        {
            return View();
        }
    }
}
