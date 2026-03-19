using CertificateStore.Api.Data;
using CertificateStore.Api.Models;
using MongoDB.Driver;

namespace CertificateStore.Api.Services;

public class UserCertificateService : IUserCertificateService
{
    private readonly MongoDbContext _context;

    public UserCertificateService(MongoDbContext context)
    {
        _context = context;
    }

    public List<UserCertificate> GetAll()
    {
        return _context.UserCertificates.Find(_ => true).ToList();
    }

    public UserCertificate? GetById(string id)
    {
        return _context.UserCertificates.Find(c => c.Id == id).FirstOrDefault();
    }

    public UserCertificate Create(UserCertificate certificate)
    {
        _context.UserCertificates.InsertOne(certificate);
        return certificate;
    }

    public UserCertificate? Update(string id, UserCertificate certificate)
    {
        var result = _context.UserCertificates.ReplaceOne(
            c => c.Id == id,
            certificate);

        if (result.MatchedCount == 0)
            return null;

        return certificate;
    }

    public bool Delete(string id)
    {
        var result = _context.UserCertificates.DeleteOne(c => c.Id == id);
        return result.DeletedCount > 0;
    }
}