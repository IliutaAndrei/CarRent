using CarRent.Models;
using CarRent.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            var cars = await _carService.GetAllCarsAsync(); 
            return View(cars); 
            //return View();
        }
        public IActionResult AddCar()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car); 
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

        // GET: Cars/Rent/5
        public async Task<IActionResult> Rent(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            if (!car.IsAvailable)
            {
                TempData["ErrorMessage"] = "Mașina selectată nu este disponibilă pentru închiriere.";
                return RedirectToAction(nameof(Details), new { id = id }); // Redirecționează către Details
            }
            return View(car);
        }

        // POST: Cars/Rent/5
        [HttpPost, ActionName("Rent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RentConfirmed(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                TempData["ErrorMessage"] = "Mașina nu a fost găsită.";
                return RedirectToAction(nameof(Index));
            }

            if (!car.IsAvailable)
            {
                TempData["ErrorMessage"] = "Mașina selectată nu mai este disponibilă.";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            bool rented = await _carService.RentCarAsync(id);
            if (rented)
            {
                TempData["SuccessMessage"] = "Mașina a fost închiriată cu succes!";
                return RedirectToAction(nameof(Details), new { id = id }); // Redirecționăm către Details
            }
            else
            {
                TempData["ErrorMessage"] = "A apărut o eroare la închirierea mașinii.";
                return RedirectToAction(nameof(Index));
            }
        }

        //public IActionResult Details(int id)
        //{
        //    var car = _carService.GetCarByIdAsync(id).Result;
        //    return View(car);
        //}
    }
}
