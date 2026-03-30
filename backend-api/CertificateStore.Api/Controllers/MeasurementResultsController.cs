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
    private readonly IMcpService _mcpService;

    public MeasurementResultsController(
        IMeasurementResultService measurementResultService,
        IMcpService mcpService)
    {
        _measurementResultService = measurementResultService;
        _mcpService = mcpService;
    }

    [HttpGet]
    public ActionResult<PagedResult<MeasurementResult>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1)
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

    // MCP Integration Endpoints

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var stats = await _mcpService.GetStatsAsync();
        var insights = await _mcpService.GetInsightsAsync();
        var latest = await _mcpService.GetLatestAsync(10);

        return Ok(new
        {
            stats = stats,
            insights = insights,
            latestResults = latest,
            generatedAt = DateTime.UtcNow
        });
    }

    [HttpGet("analytics")]
    public async Task<IActionResult> GetAnalytics()
    {
        var insights = await _mcpService.GetInsightsAsync();
        var anomalies = await _mcpService.GetAnomaliesAsync();

        return Ok(new
        {
            insights = insights,
            anomalies = anomalies,
            generatedAt = DateTime.UtcNow
        });
    }

    [HttpGet("predict/{partName}")]
    public async Task<IActionResult> Predict(string partName)
    {
        var prediction = await _mcpService.PredictAsync(partName);
        if (prediction == null)
        {
            return NotFound(new { message = $"No prediction data available for part: {partName}" });
        }

        return Ok(prediction);
    }

    [HttpGet("health")]
    public async Task<IActionResult> Health()
    {
        var mcpHealth = await _mcpService.GetStatsAsync() != null;

        return Ok(new
        {
            service = "CertificateStore.Api",
            status = "OK",
            mcpIntegration = mcpHealth ? "Connected" : "Disconnected",
            timestamp = DateTime.UtcNow,
            version = "1.0.0"
        });
    } // Force rebuild
}