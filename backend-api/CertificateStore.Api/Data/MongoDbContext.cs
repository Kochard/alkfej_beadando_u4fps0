using CertificateStore.Api.Models;
using CertificateStore.Api.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CertificateStore.Api.Data;

public class MongoDbContext
{
    public IMongoCollection<MeasurementResult> MeasurementResults { get; }

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var mongoSettings = settings.Value;

        var client = new MongoClient(mongoSettings.ConnectionString);
        var database = client.GetDatabase(mongoSettings.DatabaseName);

        MeasurementResults = database.GetCollection<MeasurementResult>(
            mongoSettings.MeasurementResultsCollectionName);
    }
}