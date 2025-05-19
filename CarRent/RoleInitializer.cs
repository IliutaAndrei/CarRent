using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using CarRent.Models; 
using Microsoft.Extensions.Logging;

public static class RoleInitializer
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                
                var adminRole = new IdentityRole { Name = "Admin" };
                var result = await roleManager.CreateAsync(adminRole);
                if (!result.Succeeded)
                {
                    
                    logger.LogError("Eroare la crearea rolului Admin: {Errors}", string.Join(", ", result.Errors));
                    return;
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

            
            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser != null)
            {
                
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    
                    var result = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (!result.Succeeded)
                    {
                        
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
