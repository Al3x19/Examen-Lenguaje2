using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SistemaContable.API.Database.Entities;

namespace SistemaContable.API.Database
{
    public class SistemaContableSeeder 
    {

        public static async Task LoadDataAsync(
           SistemaContableContext context,
           ILoggerFactory loggerFactory,
           UserManager<UserEntity> userManager,
           RoleManager<IdentityRole> roleManager
           )
        {
            try
            {
                await LoadRolesAndUsersAsync(userManager, roleManager, loggerFactory);
                //await LoadMovementsAsync(loggerFactory, context);
                //await LoadJournalEntriesAsync(loggerFactory, context);
                //await LoadAccountsAsync(loggerFactory, context);
                //await LoadBalancesAsync(loggerFactory, context);
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<SistemaContableSeeder>();
                logger.LogError(e, "Error inicializando la data del API");
            }
        }

        public static async Task LoadRolesAndUsersAsync(
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            ILoggerFactory loggerFactory
    )
        {
            try
            {
                if (!await roleManager.Roles.AnyAsync())
                {

                    await roleManager.CreateAsync(new IdentityRole("User"));
                }

                if (!await userManager.Users.AnyAsync())
                {


                    var user = new UserEntity
                    {
                        Email = "user@gmail.com",
                        UserName = "user@gmail.com",
                    };


                    await userManager.CreateAsync(user, "Temporal01*");


                    await userManager.AddToRoleAsync(user, "User");
                }
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<SistemaContableSeeder>();
                logger.LogError(e.Message);
            }
        }
        public static async Task LoadAccountsAsync(ILoggerFactory loggerFactory,SistemaContableContext context)
        {
            try
            {
                var jsonFilePath = "SeedData/accounts.json";
                var jsonContent = await File.ReadAllTextAsync(jsonFilePath);
                var accounts = JsonConvert.DeserializeObject<List<AccountEntity>>(jsonContent);

                if (!await context.Accounts.AnyAsync())
                {
                    var user = await context.Users.FirstOrDefaultAsync();

                    for (int i = 0; i < accounts.Count; i++)
                    {
                        accounts[i].CreatedBy = user.Id;
                        accounts[i].CreatedDate = DateTime.Now;
                        accounts[i].UpdatedBy = user.Id;
                        accounts[i].UpdatedDate = DateTime.Now;
                    }

                    context.AddRange(accounts);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<SistemaContableSeeder>();
                logger.LogError(e, "Error al ejecutar el Seed de categorias");
            }
        }

        public static async Task LoadBalancesAsync(ILoggerFactory loggerFactory,SistemaContableContext context)
        {
            try
            {
                var jsonFilePath = "SeedData/balances.json";
                var jsonContent = await File.ReadAllTextAsync(jsonFilePath);
                var balances = JsonConvert.DeserializeObject<List<BalanceEntity>>(jsonContent);

                if (!await context.Balances.AnyAsync())
                {
                    var user = await context.Users.FirstOrDefaultAsync();
                    for (int i = 0; i < balances.Count; i++)
                    {
                        balances[i].CreatedBy = user.Id;
                        balances[i].CreatedDate = DateTime.Now;
                        balances[i].UpdatedBy = user.Id;
                        balances[i].UpdatedDate = DateTime.Now;
                    }

                    context.AddRange(balances);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<SistemaContableSeeder>();
                logger.LogError(e, "Error al ejecutar el Seed de Balances");
            }
        }

        public static async Task LoadMovementsAsync(ILoggerFactory loggerFactory,SistemaContableContext context)
        {
            try
            {
                var jsonFilePath = "SeedData/movements.json";
                var jsonContent = await File.ReadAllTextAsync(jsonFilePath);
                var movements = JsonConvert.DeserializeObject<List<MovementEntity>>(jsonContent);

                if (!await context.Movements.AnyAsync())
                {
                    var user = await context.Users.FirstOrDefaultAsync();
                    for (int i = 0; i < movements.Count; i++)
                    {
                        movements[i].CreatedBy = user.Id;
                        movements[i].CreatedDate = DateTime.Now;
                        movements[i].UpdatedBy = user.Id;
                        movements[i].UpdatedDate = DateTime.Now;
                    }

                    context.AddRange(movements);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<SistemaContableSeeder>();
                logger.LogError(e, "Error al ejecutar el Seed de Movements");
            }
        }

        public static async Task LoadJournalEntriesAsync(ILoggerFactory loggerFactory,SistemaContableContext context)
        {
            try
            {
                var jsonFilePath = "SeedData/balances_movements.json";
                var jsonContent = await File.ReadAllTextAsync(jsonFilePath);
                var journalEntries = JsonConvert.DeserializeObject<List<JournalEntryEntity>>(jsonContent);

                if (!await context.JournalEntries.AnyAsync())
                {
                    var user = await context.Users.FirstOrDefaultAsync();
                    for (int i = 0; i <journalEntries.Count; i++)
                    {
                       journalEntries[i].CreatedBy = user.Id;
                       journalEntries[i].CreatedDate = DateTime.Now;
                       journalEntries[i].UpdatedBy = user.Id;
                       journalEntries[i].UpdatedDate = DateTime.Now;
                    }

                    context.AddRange(journalEntries);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                var logger = loggerFactory.CreateLogger<SistemaContableSeeder>();
                logger.LogError(e, "Error al ejecutar el Seed de JournalEntries");
            }
        }
    }
}
