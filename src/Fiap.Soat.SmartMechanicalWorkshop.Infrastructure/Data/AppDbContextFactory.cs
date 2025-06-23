using Fiap.Soat.SmartMechanicalWorkshop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
   using Microsoft.EntityFrameworkCore.Design;
   using Microsoft.Extensions.Configuration;
   using System.IO;

   public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
   {
       public AppDbContext CreateDbContext(string[] args)
       {
           var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

           var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
           optionsBuilder.UseMySql(
               configuration.GetConnectionString("DbConnectionString"),
               new MySqlServerVersion(new Version(8, 4, 5))
           );

           return new AppDbContext(optionsBuilder.Options);
       }
   }
   