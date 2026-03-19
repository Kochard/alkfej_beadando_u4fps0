using CertificateStore.Api.Data;
using CertificateStore.Api.Models;
using MongoDB.Driver;

namespace CertificateStore.Api.Services;

public class RootCertificateService : IRootCertificateService
{
    private readonly MongoDbContext _context;

    public RootCertificateService(MongoDbContext context)
    {
        _context = context;
    }

    public List<RootCertificate> GetAll(int pageNumber, int pageSize)
    {
        return _context.RootCertificates
            .Find(_ => true)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToList();
    }

    public RootCertificate? GetById(string id)
    {
        return _context.RootCertificates.Find(c => c.Id == id).FirstOrDefault();
    }

    public RootCertificate Create(RootCertificate certificate)
    {
        _context.RootCertificates.InsertOne(certificate);
        return certificate;
    }

    public RootCertificate? Update(string id, RootCertificate certificate)
    {
        var result = _context.RootCertificates.ReplaceOne(c => c.Id == id, certificate);

        if (result.MatchedCount == 0)
        {
            return null;
        }

        return certificate;
    }

    public bool Delete(string id)
    {
        var result = _context.RootCertificates.DeleteOne(c => c.Id == id);
        return result.DeletedCount > 0;
    }
}