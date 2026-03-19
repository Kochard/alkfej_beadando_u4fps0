using CertificateStore.Api.Models;

namespace CertificateStore.Api.Services;

public class UserCertificateService : IUserCertificateService
{
    private static readonly List<UserCertificate> Certificates = new();

    public List<UserCertificate> GetAll()
    {
        return Certificates;
    }

    public UserCertificate? GetById(string id)
    {
        return Certificates.FirstOrDefault(c => c.Id == id);
    }

    public UserCertificate Create(UserCertificate certificate)
    {
        Certificates.Add(certificate);
        return certificate;
    }

    public UserCertificate? Update(string id, UserCertificate certificate)
    {
        var existingCertificate = Certificates.FirstOrDefault(c => c.Id == id);

        if (existingCertificate is null)
        {
            return null;
        }

        existingCertificate.Username = certificate.Username;
        existingCertificate.CertificateData = certificate.CertificateData;
        existingCertificate.RootCertificateId = certificate.RootCertificateId;

        return existingCertificate;
    }

    public bool Delete(string id)
    {
        var certificate = Certificates.FirstOrDefault(c => c.Id == id);

        if (certificate is null)
        {
            return false;
        }

        Certificates.Remove(certificate);
        return true;
    }
}