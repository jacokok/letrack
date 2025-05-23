using LeTrack.Data;
using Microsoft.EntityFrameworkCore;

namespace LeTrack.Extensions;

public static class DataExtensions
{
    public static IServiceCollection ConfigureEF(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSnakeCaseNamingConvention();
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            );
            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });
        return services;
    }
}