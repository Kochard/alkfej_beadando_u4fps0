using CertificateStore.Api.Models;

namespace CertificateStore.Api.Services;

public interface IUserCertificateService
{
    List<UserCertificate> GetAll();

    UserCertificate? GetById(string id);

    UserCertificate Create(UserCertificate certificate);

    UserCertificate? Update(string id, UserCertificate certificate);

    bool Delete(string id);
}