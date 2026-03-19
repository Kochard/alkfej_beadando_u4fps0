namespace CertificateStore.Api.DTOs;

public class UpdateUserCertificateDto
{
    public string Username { get; set; } = string.Empty;

    public string CertificateData { get; set; } = string.Empty;

    public string RootCertificateId { get; set; } = string.Empty;
}