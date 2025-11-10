using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
namespace LeTrack.Extensions;

public static class HealthExtensions
{
    public static void ConfigureHealth(this IServiceCollection services, IConfiguration configuration)
    {
        var mqttBrokerSection = configuration.GetSection("Config:MqttBroker");
        string mqttBroker = mqttBrokerSection.Get<string>() ?? "localhost";

        services.AddHealthChecks()
            .AddProcessAllocatedMemoryHealthCheck(1750)
            .AddTcpHealthCheck(o =>
            {
                o.AddHost(mqttBroker, 1883);
            }, name: "MQTT")
            .AddNpgSql(configuration.GetConnectionString("DefaultConnection")!);
    }

    public static IApplicationBuilder UseHealth(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        return app;
    }
}