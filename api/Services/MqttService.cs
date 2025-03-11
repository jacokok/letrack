using MQTTnet;

namespace LeTrack.Services;

public class MqttService
{
    private static readonly MqttClientFactory Factory = new();
    private readonly IMqttClient _mqttClient = Factory.CreateMqttClient();
    private readonly IConfiguration _config;

    public MqttService(IConfiguration config)
    {
        _config = config;
    }

    public async Task ConnectMqttAsync(CancellationToken cancellationToken)
    {
        if (_mqttClient.IsConnected) return;

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(_config["Config:MqttBroker"])
            .Build();

        await _mqttClient.ConnectAsync(options, cancellationToken);
    }
    public MqttClientFactory GetMqttFactory()
    {
        return Factory;
    }
    public IMqttClient GetMqttClient()
    {
        return _mqttClient;
    }
    public void Publish(string topic, string payload)
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithRetainFlag()
            .Build();

        _mqttClient.PublishAsync(message);
    }
}