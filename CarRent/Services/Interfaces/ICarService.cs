using CarRent.Models;

namespace CarRent.Services.Interfaces
{
    public interface ICarService
    {
        Task<Car> GetCarByIdAsync(int id);
        Task<List<Car>> GetAllCarsAsync();
        Task AddCarAsync(Car car);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(int id);

        
        Task<List<Car>> SearchCarsAsync(
            string make,
            string model,
            int? year,
            string fuelType,
            string transmissionType,
            bool? isAvailable,
            decimal? minPrice,
            decimal? maxPrice);
    }
}
