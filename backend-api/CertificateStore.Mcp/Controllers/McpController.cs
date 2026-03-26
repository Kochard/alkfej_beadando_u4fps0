using CertificateStore.Mcp.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CertificateStore.Mcp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class McpController : ControllerBase
{
    private readonly MongoDbContext _context;

    public McpController(MongoDbContext context)
    {
        _context = context;
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(new
        {
            service = "CertificateStore.Mcp",
            status = "OK",
            timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("stats")]
    public IActionResult Stats()
    {
        var total = _context.MeasurementResults.CountDocuments(x => true);
        var pass = _context.MeasurementResults.CountDocuments(x => x.Status == "PASS");
        var fail = _context.MeasurementResults.CountDocuments(x => x.Status == "FAIL" || x.Status == "NG");

        return Ok(new
        {
            totalResults = total,
            passResults = pass,
            failResults = fail,
            generatedAt = DateTime.UtcNow
        });
    }

    [HttpGet("latest")]
    public IActionResult Latest()
    {
        var latest = _context.MeasurementResults
            .Find(x => true)
            .SortByDescending(x => x.MeasuredAt)
            .Limit(5)
            .ToList();

        return Ok(latest);
    }
}