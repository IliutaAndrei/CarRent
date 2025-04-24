using CarRent.Data;
using CarRent.Models;
using CarRent.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Services
{
    public class CarService : ICarService
    {
        private readonly CarRentContext _context;

        public CarService(CarRentContext context)
        {
            _context = context;
        }



        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task AddCarAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCarAsync(Car car)
        {
            _context.Entry(car).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Car>> SearchCarsAsync(
    string make,
    string model,
    int? year,
    string fuelType, // Acum primim string din formular
    string transmissionType, // Acum primim string din formular
    bool? isAvailable,
    decimal? minPrice,
    decimal? maxPrice)
        {
            IQueryable<Car> query = _context.Cars;

            if (!string.IsNullOrEmpty(make))
            {
                query = query.Where(c => c.Make.ToLower().Contains(make.ToLower()));
            }

            if (!string.IsNullOrEmpty(model))
            {
                query = query.Where(c => c.Model.ToLower().Contains(model.ToLower()));
            }

            if (year.HasValue)
            {
                query = query.Where(c => c.YearOfFabrication == year.Value);
            }

            if (!string.IsNullOrEmpty(fuelType))
            {
                if (Enum.TryParse<FuelType>(fuelType, true, out var parsedFuelType))
                {
                    query = query.Where(c => c.FuelType == parsedFuelType);
                }
                // Dacă nu se poate parsa, ignorăm filtrul pentru FuelType
            }

            if (!string.IsNullOrEmpty(transmissionType))
            {
                if (Enum.TryParse<TransmissionType>(transmissionType, true, out var parsedTransmissionType))
                {
                    query = query.Where(c => c.TransmissionType == parsedTransmissionType);
                }
                // Dacă nu se poate parsa, ignorăm filtrul pentru TransmissionType
            }

            if (isAvailable.HasValue)
            {
                query = query.Where(c => c.IsAvailable == isAvailable.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(c => c.PricePerDay >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(c => c.PricePerDay <= maxPrice.Value);
            }

            return await query.ToListAsync();
        }
    }
}
