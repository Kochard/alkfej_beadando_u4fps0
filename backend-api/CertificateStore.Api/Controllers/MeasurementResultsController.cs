using CertificateStore.Api.DTOs;
using CertificateStore.Api.Models;
using CertificateStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CertificateStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeasurementResultsController : ControllerBase
{
    private readonly IMeasurementResultService _measurementResultService;

    public MeasurementResultsController(IMeasurementResultService measurementResultService)
    {
        _measurementResultService = measurementResultService;
    }

    [HttpGet]
    public ActionResult<List<MeasurementResult>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("pageNumber and pageSize must be greater than 0.");
        }

        return Ok(_measurementResultService.GetAll(pageNumber, pageSize));
    }

    [HttpGet("{id}")]
    public ActionResult<MeasurementResult> GetById(string id)
    {
        var result = _measurementResultService.GetById(id);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public ActionResult<MeasurementResult> Create(CreateMeasurementResultDto dto)
    {
        var result = new MeasurementResult
        {
            PartName = dto.PartName,
            SerialNumber = dto.SerialNumber,
            MeasurementType = dto.MeasurementType,
            MeasuredValue = dto.MeasuredValue,
            Unit = dto.Unit,
            LowerLimit = dto.LowerLimit,
            UpperLimit = dto.UpperLimit,
            Status = dto.Status,
            MeasuredBy = dto.MeasuredBy,
            MeasuredAt = dto.MeasuredAt,
            Notes = dto.Notes
        };

        var createdResult = _measurementResultService.Create(result);

        return CreatedAtAction(nameof(GetById), new { id = createdResult.Id }, createdResult);
    }

    [HttpPut("{id}")]
    public ActionResult<MeasurementResult> Update(string id, UpdateMeasurementResultDto dto)
    {
        var result = new MeasurementResult
        {
            Id = id,
            PartName = dto.PartName,
            SerialNumber = dto.SerialNumber,
            MeasurementType = dto.MeasurementType,
            MeasuredValue = dto.MeasuredValue,
            Unit = dto.Unit,
            LowerLimit = dto.LowerLimit,
            UpperLimit = dto.UpperLimit,
            Status = dto.Status,
            MeasuredBy = dto.MeasuredBy,
            MeasuredAt = dto.MeasuredAt,
            Notes = dto.Notes
        };

        var updatedResult = _measurementResultService.Update(id, result);

        if (updatedResult is null)
        {
            return NotFound();
        }

        return Ok(updatedResult);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var deleted = _measurementResultService.Delete(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}