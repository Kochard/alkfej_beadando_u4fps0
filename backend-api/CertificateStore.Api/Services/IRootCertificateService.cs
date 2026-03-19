using CertificateStore.Api.Models;

namespace CertificateStore.Api.Services;

public interface IRootCertificateService
{
    List<RootCertificate> GetAll();

    RootCertificate? GetById(string id);

    RootCertificate Create(RootCertificate certificate);

    RootCertificate? Update(string id, RootCertificate certificate);

    bool Delete(string id);
}