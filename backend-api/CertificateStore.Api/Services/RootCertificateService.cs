using CertificateStore.Api.Models;

namespace CertificateStore.Api.Services;

public class RootCertificateService : IRootCertificateService
{
    private static readonly List<RootCertificate> Certificates = new();

    public List<RootCertificate> GetAll()
    {
        return Certificates;
    }

    public RootCertificate? GetById(string id)
    {
        return Certificates.FirstOrDefault(c => c.Id == id);
    }

    public RootCertificate Create(RootCertificate certificate)
    {
        Certificates.Add(certificate);
        return certificate;
    }

    public RootCertificate? Update(string id, RootCertificate certificate)
    {
        var existingCertificate = Certificates.FirstOrDefault(c => c.Id == id);

        if (existingCertificate is null)
        {
            return null;
        }

        existingCertificate.Name = certificate.Name;
        existingCertificate.CertificateData = certificate.CertificateData;

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