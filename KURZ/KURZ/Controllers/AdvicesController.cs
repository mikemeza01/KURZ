using KURZ.Entities;
using KURZ.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KURZ.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdvicesController : Controller
    {
        private readonly IAdvicesModel _advicesModel;
        public AdvicesController(IAdvicesModel advicesModel)
        { 
            _advicesModel = advicesModel;
        }

        public IActionResult GetAdvices()
        {
            try
            {
                var advices = _advicesModel.GetAdvices();
                return View(advices);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult DetailsAdvice(int id)
        {
            try
            {
                var advices = _advicesModel.GetAdvicesById(id);

                return Json(advices);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
