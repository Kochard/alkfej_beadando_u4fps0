namespace CertificateStore.Api.Settings;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;

    public string MeasurementResultsCollectionName { get; set; } = string.Empty;
}