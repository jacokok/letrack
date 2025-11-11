global using FastEndpoints;

using LeTrack.BackgroundServices;
using LeTrack.Extensions;
using LeTrack.Hubs;
using LeTrack.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddLoggingConfiguration(builder.Configuration);

builder.Services.AddSingleton<MqttService>();
builder.Services.AddHostedService<EventSubscriber>();

builder.Services.ConfigureHealth(builder.Configuration);
builder.Services.ConfigureGeneral();
builder.Services.ConfigureJobs();
builder.Services.ConfigureEF(builder.Configuration, builder.Environment);
builder.Services.ConfigureApi();

var corsSection = builder.Configuration.GetSection("Cors");
builder.Services.ConfigureCors(corsSection.Get<string[]>() ?? [""]);

var app = builder.Build();

await app.Services.ApplyMigrations(app.Lifetime.ApplicationStopping);

app.UseCorsLeTrack();
app.UseApi();
app.MapHub<LeTrackHub>("/hub");
app.UseHealth();
app.Run();
