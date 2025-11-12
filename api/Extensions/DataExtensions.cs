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
            options.UseNpgsql(options =>
            {
                configuration.GetConnectionString("DefaultConnection");
                options.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);

                options.EnableRetryOnFailure(maxRetryCount: 1);
                options.CommandTimeout(30);
            }
            );
            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });
        return services;
    }

    public static async Task ApplyMigrations(this IServiceProvider serviceProvider, CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (db.Database.IsNpgsql())
        {
            await db.Database.MigrateAsync(ct);
        }
    }
}