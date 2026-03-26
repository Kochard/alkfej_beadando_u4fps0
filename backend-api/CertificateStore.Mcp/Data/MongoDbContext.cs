using CertificateStore.Mcp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CertificateStore.Mcp.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoDbSettings _settings;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        _settings = settings.Value;
        var client = new MongoClient(_settings.ConnectionString);
        _database = client.GetDatabase(_settings.DatabaseName);
    }

    public IMongoCollection<MeasurementResult> MeasurementResults =>
        _database.GetCollection<MeasurementResult>(_settings.MeasurementResultsCollectionName);
}