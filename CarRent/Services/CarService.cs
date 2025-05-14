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

        public async Task<int> CountAllCarsAsync()
        {
            return await _context.Cars.CountAsync();
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<bool> AddCarAsync(Car car)
        {
            try
            {
                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task UpdateCarAsync(Car car)
        {
            _context.Entry(car).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {

                return false; 
            }

            _context.Cars.Remove(car);
            try
            {
                await _context.SaveChangesAsync();

                return true; 
            }
            catch (DbUpdateException ex)
            {

                return false; 
            }
        }

        public async Task<List<Car>> SearchCarsAsync(
            string make,
            string model,
            int? year,
            string fuelType,
            string transmissionType,
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
                query = query.Where(c => c.YearOfFabrication == year);
            }

            if (!string.IsNullOrEmpty(fuelType) && fuelType.ToLower() != "toate")
            {
                if (Enum.TryParse<FuelType>(fuelType, out var parsedFuelType))
                {
                    query = query.Where(c => c.FuelType == parsedFuelType);
                }
            }

            if (!string.IsNullOrEmpty(transmissionType) && transmissionType.ToLower() != "toate")
            {
                if (Enum.TryParse<TransmissionType>(transmissionType, out var parsedTransmissionType))
                {
                    query = query.Where(c => c.TransmissionType == parsedTransmissionType);
                }
            }

            if (isAvailable.HasValue)
            {
                query = query.Where(c => c.IsAvailable == isAvailable);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(c => c.PricePerDay >= minPrice);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(c => c.PricePerDay <= maxPrice);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> RentCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return false;
            }

            if (!car.IsAvailable)
            {
                return false;
            }

            car.IsAvailable = false;
            _context.Cars.Update(car);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}
