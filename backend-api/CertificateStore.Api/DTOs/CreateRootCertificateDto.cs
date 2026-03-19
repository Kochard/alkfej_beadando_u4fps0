namespace CertificateStore.Api.DTOs;

public class CreateRootCertificateDto
{
    public string Name { get; set; } = string.Empty;

    public string CertificateData { get; set; } = string.Empty;
}