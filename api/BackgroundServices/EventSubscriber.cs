using System.Text.Json;
using LeTrack.Data;
using LeTrack.Entities;
using LeTrack.Features.Events;
using LeTrack.Services;
using MQTTnet;
using MQTTnet.Client;

namespace LeTrack.BackgroundServices;

public class EventSubscriber : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<EventSubscriber> _logger;
    private readonly MqttService _mqttService;

    public EventSubscriber(ILogger<EventSubscriber> logger, IServiceScopeFactory serviceScopeFactory, MqttService mqttService)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _mqttService = mqttService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (!_mqttService.GetMqttClient().IsConnected)
                {
                    if (!await _mqttService.GetMqttClient().TryPingAsync())
                    {
                        await _mqttService.ConnectMqttAsync(stoppingToken);
                        _logger.LogInformation("The MQTT client is connected.");
                        _mqttService.GetMqttClient().ApplicationMessageReceivedAsync += ReceiveMessage;
                        var mqttSubscribeOptions = _mqttService.GetMqttFactory().CreateSubscribeOptionsBuilder().WithTopicFilter("event/#").Build();
                        await _mqttService.GetMqttClient().SubscribeAsync(mqttSubscribeOptions, stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MQTT connection error");
            }
            finally
            {
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }

    private async Task ReceiveMessage(MqttApplicationMessageReceivedEventArgs e)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var eventModel = JsonSerializer.Deserialize<SaveEvent>(e.ApplicationMessage.ConvertPayloadToString());
        _logger.LogInformation("Id: {id}, Track: {track}, Timestamp: {ts}", eventModel?.Id, eventModel?.TrackId, eventModel?.Timestamp);

        if (eventModel == null)
        {
            _logger.LogError("Received eventModel that we could not parse and was null");
            return;
        }

        Event evt = new()
        {
            Id = eventModel.Id,
            TrackId = eventModel.TrackId,
            Timestamp = eventModel.Timestamp
        };

        if (db.Event.Any(x => x.Id == evt.Id))
        {
            _logger.LogInformation("Event already exists");
            return;
        }

        await db.Event.AddAsync(evt);
        await db.SaveChangesAsync();

        await new SaveEvent
        {
            Id = eventModel.Id,
            TrackId = eventModel.TrackId,
            Timestamp = eventModel.Timestamp
        }.PublishAsync(Mode.WaitForNone);

        return;
    }
}