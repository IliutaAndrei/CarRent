using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CarRent.Models;
using CarRent.Services; 
using CarRent.Data;    

public class CarServiceTests
{
    [Fact]
    public async Task CountAllCarsAsync_ReturnsCorrectCount()
    {

        var options = new DbContextOptionsBuilder<CarRentContext>()
            .UseInMemoryDatabase(databaseName: "CarTestDb")
            .Options;


        using (var context = new CarRentContext(options))
        {
            context.Cars.Add(new Car
            {
                Make = "Toyota",
                Model = "Corolla",
                YearOfFabrication = 2020,
                FuelType = FuelType.Benzina,
                TransmissionType = TransmissionType.Automata,
                IsAvailable = true,
                PricePerDay = 50
            });

            context.Cars.Add(new Car
            {
                Make = "Honda",
                Model = "Civic",
                YearOfFabrication = 2021,
                FuelType = FuelType.Diesel,
                TransmissionType = TransmissionType.Manuala,
                IsAvailable = false,
                PricePerDay = 45
            });

            await context.SaveChangesAsync();
        }


        using (var context = new CarRentContext(options))
        {
            var service = new CarService(context);
            var result = await service.CountAllCarsAsync();


            Assert.Equal(2, result);
        }
    }
}
