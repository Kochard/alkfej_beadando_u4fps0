using CertificateStore.Api.DTOs;
using CertificateStore.Api.Models;
using CertificateStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CertificateStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserCertificatesController : ControllerBase
{
    private readonly IUserCertificateService _userCertificateService;

    public UserCertificatesController(IUserCertificateService userCertificateService)
    {
        _userCertificateService = userCertificateService;
    }

    [HttpGet]
    public ActionResult<List<UserCertificate>> GetAll()
    {
        return Ok(_userCertificateService.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<UserCertificate> GetById(string id)
    {
        var certificate = _userCertificateService.GetById(id);

        if (certificate is null)
        {
            return NotFound();
        }

        return Ok(certificate);
    }

    [HttpPost]
    public ActionResult<UserCertificate> Create(CreateUserCertificateDto dto)
    {
        var certificate = new UserCertificate
        {
            Username = dto.Username,
            CertificateData = dto.CertificateData,
            RootCertificateId = dto.RootCertificateId
        };

        var createdCertificate = _userCertificateService.Create(certificate);

        return CreatedAtAction(nameof(GetById), new { id = createdCertificate.Id }, createdCertificate);
    }

    [HttpPut("{id}")]
    public ActionResult<UserCertificate> Update(string id, UpdateUserCertificateDto dto)
    {
        var certificate = new UserCertificate
        {
            Id = id,
            Username = dto.Username,
            CertificateData = dto.CertificateData,
            RootCertificateId = dto.RootCertificateId
        };

        var updatedCertificate = _userCertificateService.Update(id, certificate);

        if (updatedCertificate is null)
        {
            return NotFound();
        }

        return Ok(updatedCertificate);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var deleted = _userCertificateService.Delete(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}