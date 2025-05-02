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
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
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
                bool isAdded = await _carService.AddCarAsync(car);
                if (isAdded)
                {
                    TempData["SuccessMessage"] = "Mașina a fost adăugată cu succes!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "A apărut o eroare la adăugarea mașinii.";
                }
            }
            return View(car);
        }
    }
}
