using CertificateStore.Api.Models;
using CertificateStore.Api.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CertificateStore.Api.Data;

public class MongoDbContext
{
    public IMongoCollection<RootCertificate> RootCertificates { get; }

    public IMongoCollection<UserCertificate> UserCertificates { get; }

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var mongoSettings = settings.Value;

        var client = new MongoClient(mongoSettings.ConnectionString);
        var database = client.GetDatabase(mongoSettings.DatabaseName);

        RootCertificates = database.GetCollection<RootCertificate>(
            mongoSettings.RootCertificatesCollectionName);

        UserCertificates = database.GetCollection<UserCertificate>(
            mongoSettings.UserCertificatesCollectionName);
    }
}