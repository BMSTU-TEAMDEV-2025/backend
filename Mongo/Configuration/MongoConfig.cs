namespace Mongo.Configuration;

public class MongoConfig
{
    public required string Url { get; set; }
    public required string Database { get; set; }
    public string? ApplicationName { get; set; }
    public required TimeSpan ConnectTimeout { get; set; }
    public required TimeSpan SocketTimeout { get; set; }
    public required TimeSpan ServerSelectionTimeout { get; set; }
}
