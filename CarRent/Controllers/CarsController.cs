using CarRent.Models;
using CarRent.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(
            string make,
            string model,
            int? year,
            FuelType? fuelType,
            TransmissionType? transmissionType,
            bool? isAvailable,
            decimal? minPrice,
            decimal? maxPrice)
        {
            var searchResults = await _carService.SearchCarsAsync(
                make,
                model,
                year,
                fuelType?.ToString(),
                transmissionType?.ToString(),
                isAvailable,
                minPrice,
                maxPrice);

            return View("SearchResults", searchResults);
        }

        public IActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View();
        }
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
