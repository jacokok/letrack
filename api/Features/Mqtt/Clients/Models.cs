namespace LeTrack.Features.Mqtt.Clients;

public class Request
{
    // No parameters needed for listing all clients
}

public class Response
{
    public List<MqttClientInfo> ConnectedClients { get; set; } = new();
    public MqttBrokerInfo BrokerInfo { get; set; } = new();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class MqttClientInfo
{
    public string ClientId { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public DateTime ConnectedAt { get; set; }
    public TimeSpan ConnectedDuration { get; set; }
    public long BytesSent { get; set; }
    public long BytesReceived { get; set; }
    public int MessagesSent { get; set; }
    public int MessagesReceived { get; set; }
    public string Protocol { get; set; } = string.Empty;
    public bool IsConnected { get; set; }
}

public class MqttBrokerInfo
{
    public string BrokerAddress { get; set; } = string.Empty;
    public bool IsConnected { get; set; }
    public int TotalConnectedClients { get; set; }
    public DateTime LastConnectionTime { get; set; }
    public string Version { get; set; } = string.Empty;
}