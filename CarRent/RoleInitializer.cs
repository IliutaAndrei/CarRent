using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using CarRent.Models; // Asigură-te că ai namespace-ul corect
using Microsoft.Extensions.Logging;

public static class RoleInitializer
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>(); // Folosește ApplicationUser
            var logger = services.GetRequiredService<ILogger<Program>>();

            // Asigură-te că rolul "Admin" există.
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                // Dacă rolul nu există, îl creăm.
                var adminRole = new IdentityRole { Name = "Admin" };
                var result = await roleManager.CreateAsync(adminRole);
                if (!result.Succeeded)
                {
                    // Log eroare: Important să logăm erorile pentru a depana.
                    logger.LogError("Eroare la crearea rolului Admin: {Errors}", string.Join(", ", result.Errors));
                    return; // Oprim execuția dacă nu putem crea rolul.
                }
                else
                {
                    logger.LogInformation("Rolul Admin a fost creat cu succes.");
                }
            }
            else
            {
                logger.LogInformation("Rolul Admin există deja.");
            }

            // Caută utilizatorul "admin" după adresa de email.
            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser != null)
            {
                // Verifică dacă utilizatorul are deja rolul "Admin".
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    // Dacă utilizatorul nu are rolul, îl adăugăm.
                    var result = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (!result.Succeeded)
                    {
                        // Log eroare: Important să logăm erorile pentru a depana.
                        logger.LogError("Eroare la adăugarea rolului 'Admin' utilizatorului admin: {Errors}", string.Join(", ", result.Errors));
                    }
                    else
                    {
                        logger.LogInformation("Rolul Admin a fost adăugat utilizatorului admin cu succes.");
                    }
                }
                else
                {
                    logger.LogInformation("Utilizatorul admin are deja rolul 'Admin'.");
                }
            }
            else
            {
                logger.LogInformation("Nu s-a găsit utilizatorul admin cu email-ul 'admin@example.com'.");
            }
        }
    }
}
