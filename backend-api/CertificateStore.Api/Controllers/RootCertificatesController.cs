using CertificateStore.Api.DTOs;
using CertificateStore.Api.Models;
using CertificateStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CertificateStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RootCertificatesController : ControllerBase
{
    private readonly IRootCertificateService _rootCertificateService;

    public RootCertificatesController(IRootCertificateService rootCertificateService)
    {
        _rootCertificateService = rootCertificateService;
    }

    [HttpGet]
    public ActionResult<List<RootCertificate>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("pageNumber and pageSize must be greater than 0.");
        }

        return Ok(_rootCertificateService.GetAll(pageNumber, pageSize));
    }

    [HttpGet("{id}")]
    public ActionResult<RootCertificate> GetById(string id)
    {
        var certificate = _rootCertificateService.GetById(id);

        if (certificate is null)
        {
            return NotFound();
        }

        return Ok(certificate);
    }

    [HttpPost]
    public ActionResult<RootCertificate> Create(CreateRootCertificateDto dto)
    {
        var certificate = new RootCertificate
        {
            Name = dto.Name,
            CertificateData = dto.CertificateData
        };

        var createdCertificate = _rootCertificateService.Create(certificate);

        return CreatedAtAction(nameof(GetById), new { id = createdCertificate.Id }, createdCertificate);
    }

    [HttpPut("{id}")]
    public ActionResult<RootCertificate> Update(string id, UpdateRootCertificateDto dto)
    {
        var certificate = new RootCertificate
        {
            Id = id,
            Name = dto.Name,
            CertificateData = dto.CertificateData
        };

        var updatedCertificate = _rootCertificateService.Update(id, certificate);

        if (updatedCertificate is null)
        {
            return NotFound();
        }

        return Ok(updatedCertificate);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var deleted = _rootCertificateService.Delete(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}