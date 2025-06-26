using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class AssociationContextFactory : IDesignTimeDbContextFactory<AssociationDbContext>
    {
        public AssociationDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "WebApi");

            Console.WriteLine("Base path: " + basePath);


            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            var connection = configuration.GetConnectionString("DefaultConnection");

            Console.WriteLine("ENV: " + environment);
            Console.WriteLine("CONNECTION: " + connection);

            var optionsBuilder = new DbContextOptionsBuilder<AssociationDbContext>();
            optionsBuilder.UseNpgsql(connection);

            return new AssociationDbContext(optionsBuilder.Options);
        }
    }
}
