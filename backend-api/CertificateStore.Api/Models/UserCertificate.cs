namespace CertificateStore.Api.Models;

public class UserCertificate
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Username { get; set; } = string.Empty;

    public string CertificateData { get; set; } = string.Empty;

    public string RootCertificateId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}