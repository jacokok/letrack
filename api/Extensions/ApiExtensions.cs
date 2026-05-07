using System.Text.Json.Serialization;
using FastEndpoints.Swagger;
using LeTrack.Extensions.Swagger;
using Namotion.Reflection;

namespace LeTrack.Extensions;

public static class ApiExtensions
{
    public static void ConfigureApi(this IServiceCollection services)
    {
        services.AddResponseCompression(o =>
        {
            o.EnableForHttps = true;
        });

        services.AddFastEndpoints().SwaggerDocument(o =>
        {
            o.EnableJWTBearerAuth = false;
            o.DocumentSettings = s =>
            {
                s.Title = AppDomain.CurrentDomain.FriendlyName;
                s.Version = "v1";
                s.OperationProcessors.Add(new CustomOperationsProcessor());
                s.SchemaSettings.SchemaNameGenerator = new CustomSchemaNameGenerator(false);
                s.MarkNonNullablePropsAsRequired();
            };
        });
    }

    public static IApplicationBuilder UseApi(this IApplicationBuilder app)
    {
        app.UseResponseCompression();
        app.UseDefaultExceptionHandler()
        .UseFastEndpoints(c =>
        {
            c.Serializer.Options.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            c.Endpoints.Configurator = ep =>
            {
                ep.Options(x => x.Produces<InternalErrorResponse>(500));
            };
        });
        app.UseSwaggerGen(_ => { }, ui => ui.Path = string.Empty);
        return app;
    }
}
