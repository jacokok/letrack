using LeTrack.BackgroundServices;

namespace LeTrack.Extensions;

public static class GeneralExtensions
{
    public static IServiceCollection ConfigureGeneral(this IServiceCollection services)
    {
        services.AddSignalR();
        return services;
    }
}