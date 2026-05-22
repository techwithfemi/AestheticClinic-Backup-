// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Models;
using AestheticEMR.Core.Models.Account;
using AestheticEMR.Core.Models.Aesthetic;
using AestheticEMR.Core.Models.Shop;
using AestheticEMR.Core.Services.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace AestheticEMR.Core.Infrastructure
{
    public class DatabaseSeeder(ApplicationDbContext dbContext, ILogger<DatabaseSeeder> logger,
        IUserAccountService userAccountService, IUserRoleService userRoleService) : IDatabaseSeeder
    {
        public async Task SeedAsync()
        {
            await dbContext.Database.MigrateAsync();
            await SeedDefaultUsersAsync();
            await SeedConsentTemplatesAsync();
            await SeedDemoDataAsync();
        }

        private async Task SeedConsentTemplatesAsync()
        {
            if (!await TableExistsAsync("AppAestheticConsentTemplates"))
            {
                logger.LogWarning("Skipping consent template seeding because table {TableName} does not exist", "AppAestheticConsentTemplates");
                return;
            }

            logger.LogInformation("Seeding aesthetics consent templates");

            var templates = new[]
            {
                new AestheticConsentTemplate
                {
                    Name = "Botox Consent",
                    Title = "Botox Treatment Consent",
                    ProcedureType = "Botox",
                    Content = "I confirm that the Botox procedure has been explained to me, including expected benefits, possible risks, side effects, alternatives and after-care instructions. I consent to proceed with treatment.",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                },
                new AestheticConsentTemplate
                {
                    Name = "Dental Consent",
                    Title = "Dental Treatment Consent",
                    ProcedureType = "Dental",
                    Content = "DENTAL TREATMENT CONSENT FORM\r\n\r\nTREATMENT DETAILS\r\nPROPOSED TREATMENT: _________________________________________________\r\nDENTIST PERFORMING THE TREATMENT: ____________________________________\r\nDATE: ____________________\r\nDESCRIPTION OF PROPOSED TREATMENTS\r\nTHE DENTAL PROCEDURE(S) MAY INCLUDE BUT NOT LIMITED TO:\r\n\r\nI understand that the nature of the treatment, expected benefits, potential risks, and alternatives to the procedure(s) have been explained to me.\r\nI understand that during the course of treatment, unforeseen conditions may require different procedures or additional treatments.\r\nI acknowledge that the following risks are associated with the proposed treatment(s):\r\n• Pain, discomfort, or swelling\r\n• Prolonged numbness or altered sensation\r\n• Need for further treatments, adjustments, or procedures\r\n• Others: _________________________________________________\r\n\r\nNAME & SIGNATURE: _________________________________________________",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                },
                new AestheticConsentTemplate
                {
                    Name = "Laser Consent",
                    Title = "Laser Treatment Consent",
                    ProcedureType = "Laser",
                    Content = "I confirm that the laser procedure has been explained to me, including expected results, possible discomfort, burns, pigment changes, required eye protection and after-care instructions. I consent to proceed with treatment.",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                },
                new AestheticConsentTemplate
                {
                    Name = "Spa Consent",
                    Title = "Spa Treatment Consent",
                    ProcedureType = "Spa",
                    Content = "I confirm that the spa procedure has been explained to me, including expected benefits, possible irritation, contraindications and after-care instructions. I consent to proceed with treatment.",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                }
            };

            foreach (var template in templates)
            {
                var existing = await dbContext.AestheticConsentTemplates
                    .FirstOrDefaultAsync(x => x.ProcedureType == template.ProcedureType && x.Title == template.Title);

                if (existing == null)
                {
                    dbContext.AestheticConsentTemplates.Add(template);
                    continue;
                }

                existing.Name = template.Name;
                existing.Title = template.Title;
                existing.ProcedureType = template.ProcedureType;
                existing.Content = template.Content;
                existing.IsActive = template.IsActive;
                existing.UpdatedDate = DateTime.UtcNow;
            }

            await dbContext.SaveChangesAsync();
            logger.LogInformation("Aesthetics consent templates seeded");
        }

        private async Task<bool> TableExistsAsync(string tableName)
        {
            var connection = dbContext.Database.GetDbConnection();
            var wasOpen = connection.State == ConnectionState.Open;

            if (!wasOpen)
            {
                await connection.OpenAsync();
            }

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @tableName";

            var parameter = command.CreateParameter();
            parameter.ParameterName = "@tableName";
            parameter.Value = tableName;
            command.Parameters.Add(parameter);

            var exists = await command.ExecuteScalarAsync() is not null;

            if (!wasOpen)
            {
                await connection.CloseAsync();
            }

            return exists;
        }

        /************ DEFAULT USERS **************/

        private async Task SeedDefaultUsersAsync()
        {
            if (!await dbContext.Users.AnyAsync())
            {
                logger.LogInformation("Generating inbuilt accounts");

                const string adminRoleName = "administrator";
                const string userRoleName = "user";

                await EnsureRoleAsync(adminRoleName, "Default administrator",
                    ApplicationPermissions.GetAllPermissionValues());

                await EnsureRoleAsync(userRoleName, "Default user", []);

                await CreateUserAsync("admin",
                                      "tempP@ss123",
                                      "Inbuilt Administrator",
                                      "admin@ebenmonney.com",
                                      "+1 (123) 000-0000",
                                      [adminRoleName]);

                await CreateUserAsync("user",
                                      "tempP@ss123",
                                      "Inbuilt Standard User",
                                      "user@ebenmonney.com",
                                      "+1 (123) 000-0001",
                                      [userRoleName]);

                logger.LogInformation("Inbuilt account generation completed");
            }
        }

        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if (await userRoleService.GetRoleByNameAsync(roleName) == null)
            {
                logger.LogInformation("Generating default role: {roleName}", roleName);

                var applicationRole = new ApplicationRole(roleName, description);

                var result = await userRoleService.CreateRoleAsync(applicationRole, claims);

                if (!result.Succeeded)
                {
                    throw new UserRoleException($"Seeding \"{description}\" role failed. Errors: " +
                        $"{string.Join(Environment.NewLine, result.Errors)}");
                }
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(
            string userName, string password, string fullName, string email, string phoneNumber, string[] roles)
        {
            logger.LogInformation("Generating default user: {userName}", userName);

            var applicationUser = new ApplicationUser
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                IsEnabled = true
            };

            var result = await userAccountService.CreateUserAsync(applicationUser, roles, password);

            if (!result.Succeeded)
            {
                throw new UserAccountException($"Seeding \"{userName}\" user failed. Errors: " +
                    $"{string.Join(Environment.NewLine, result.Errors)}");
            }

            return applicationUser;
        }

        /************ DEMO DATA **************/

        private async Task SeedDemoDataAsync()
        {
            if (!await dbContext.Customers.AnyAsync() && !await dbContext.ProductCategories.AnyAsync())
            {
                logger.LogInformation("Seeding demo data");

                var cust_1 = new Customer
                {
                    Name = "Ebenezer Monney",
                    Email = "contact@ebenmonney.com",
                    Gender = Gender.Male
                };

                var cust_2 = new Customer
                {
                    Name = "Itachi Uchiha",
                    Email = "uchiha@narutoverse.com",
                    PhoneNumber = "+81123456789",
                    Address = "Some fictional Address, Street 123, Konoha",
                    City = "Konoha",
                    Gender = Gender.Male
                };

                var cust_3 = new Customer
                {
                    Name = "John Doe",
                    Email = "johndoe@anonymous.com",
                    PhoneNumber = "+18585858",
                    Address = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio.
                    Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at elementum imperdiet",
                    City = "Lorem Ipsum",
                    Gender = Gender.Male
                };

                var cust_4 = new Customer
                {
                    Name = "Jane Doe",
                    Email = "Janedoe@anonymous.com",
                    PhoneNumber = "+18585858",
                    Address = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio.
                    Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at elementum imperdiet",
                    City = "Lorem Ipsum",
                    Gender = Gender.Male
                };

                var prodCat_1 = new ProductCategory
                {
                    Name = "None",
                    Description = "Default category. Products that have not been assigned a category"
                };

                var prod_1 = new Product
                {
                    Name = "BMW M6",
                    Description = "Yet another masterpiece from the world's best car manufacturer",
                    BuyingPrice = 109775,
                    SellingPrice = 114234,
                    UnitsInStock = 12,
                    IsActive = true,
                    ProductCategory = prodCat_1
                };

                var prod_2 = new Product
                {
                    Name = "Nissan Patrol",
                    Description = "A true man's choice",
                    BuyingPrice = 78990,
                    SellingPrice = 86990,
                    UnitsInStock = 4,
                    IsActive = true,
                    ProductCategory = prodCat_1
                };

                var ordr_1 = new Order
                {
                    Discount = 500,
                    Cashier = await dbContext.Users.OrderBy(u => u.UserName).FirstAsync(),
                    Customer = cust_1
                };

                var ordr_2 = new Order
                {
                    Cashier = await dbContext.Users.OrderBy(u => u.UserName).FirstAsync(),
                    Customer = cust_2
                };

                ordr_1.OrderDetails.Add(new()
                {
                    UnitPrice = prod_1.SellingPrice,
                    Quantity = 1,
                    Product = prod_1,
                    Order = ordr_1
                });
                ordr_1.OrderDetails.Add(new()
                {
                    UnitPrice = prod_2.SellingPrice,
                    Quantity = 1,
                    Product = prod_2,
                    Order = ordr_1
                });

                ordr_2.OrderDetails.Add(new()
                {
                    UnitPrice = prod_2.SellingPrice,
                    Quantity = 1,
                    Product = prod_2,
                    Order = ordr_2
                });

                dbContext.Customers.Add(cust_1);
                dbContext.Customers.Add(cust_2);
                dbContext.Customers.Add(cust_3);
                dbContext.Customers.Add(cust_4);

                dbContext.Products.Add(prod_1);
                dbContext.Products.Add(prod_2);

                dbContext.Orders.Add(ordr_1);
                dbContext.Orders.Add(ordr_2);

                await dbContext.SaveChangesAsync();

                logger.LogInformation("Seeding demo data completed");
            }
        }
    }
}
