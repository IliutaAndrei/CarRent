using CarRent.Models;
using CarRent.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICarService _carService;

        public AdminController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet] 
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet] 
        public IActionResult AddCar()
        {
            return View();
        }

        [HttpPost] 
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
