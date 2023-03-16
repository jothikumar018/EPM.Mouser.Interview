using EPM.Mouser.Interview.Models;
using EPM.Mouser.Interview.Web.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EPM.Mouser.Interview.Web.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly IWarehouseService _warehouseService;

        public HomeController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _warehouseService.GetAvailableStockProducts();
            return View(products);
        }
    }
}
