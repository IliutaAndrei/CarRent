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

        [HttpGet("/cars/search")]
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

            return View(searchResults);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
