using CarRent.Models;
using CarRent.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Controllers
{
    [Route("adminpanel")]
    public class AdminController : Controller
    {
        private readonly ICarService _carService;

        public AdminController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet("")] 
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("cars/add")] 
        public IActionResult AddCar()
        {
            return View();
        }

        [HttpPost("cars/add")] 
        public async Task<IActionResult> AddCar(Car car)
        {
            if (ModelState.IsValid)
            {
                await _carService.AddCarAsync(car);
                return RedirectToAction("Index");
            }
            return View(car);
        }
    }
}
