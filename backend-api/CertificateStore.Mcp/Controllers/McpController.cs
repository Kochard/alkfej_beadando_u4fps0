using CertificateStore.Mcp.Data;
using CertificateStore.Mcp.Models;
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
            timestamp = DateTime.UtcNow,
            version = "1.0.0"
        });
    }

    [HttpGet("stats")]
    public IActionResult Stats()
    {
        var total = _context.MeasurementResults.CountDocuments(x => true);
        var pass = _context.MeasurementResults.CountDocuments(x => x.Status == "PASS");
        var fail = _context.MeasurementResults.CountDocuments(x => x.Status == "FAIL" || x.Status == "NG");

        var passRate = total > 0 ? (double)pass / total * 100 : 0;

        return Ok(new
        {
            totalResults = total,
            passResults = pass,
            failResults = fail,
            passRatePercent = Math.Round(passRate, 2),
            generatedAt = DateTime.UtcNow
        });
    }

    [HttpGet("latest")]
    public IActionResult Latest([FromQuery] int count = 5)
    {
        var latest = _context.MeasurementResults
            .Find(x => true)
            .SortByDescending(x => x.MeasuredAt)
            .Limit(Math.Min(count, 50)) // Max 50 results
            .ToList();

        return Ok(new
        {
            results = latest,
            count = latest.Count,
            requested = count,
            generatedAt = DateTime.UtcNow
        });
    }

    [HttpGet("insights")]
    public IActionResult Insights()
    {
        var totalResults = _context.MeasurementResults.CountDocuments(x => true);

        // Get measurement types distribution
        var measurementTypes = _context.MeasurementResults
            .Aggregate()
            .Group(x => x.MeasurementType, g => new { Type = g.Key, Count = g.Count() })
            .ToList();

        // Get parts with most measurements
        var topParts = _context.MeasurementResults
            .Aggregate()
            .Group(x => x.PartName, g => new { Part = g.Key, Count = g.Count() })
            .SortByDescending(x => x.Count)
            .Limit(10)
            .ToList();

        // Get recent trends (last 24 hours)
        var yesterday = DateTime.UtcNow.AddDays(-1);
        var recentResults = _context.MeasurementResults
            .Find(x => x.MeasuredAt >= yesterday)
            .ToList();

        var recentPassRate = recentResults.Count > 0
            ? (double)recentResults.Count(x => x.Status == "PASS") / recentResults.Count * 100
            : 0;

        return Ok(new
        {
            totalMeasurements = totalResults,
            measurementTypeDistribution = measurementTypes,
            topMeasuredParts = topParts,
            recentActivity = new
            {
                last24Hours = recentResults.Count,
                passRate = Math.Round(recentPassRate, 2)
            },
            generatedAt = DateTime.UtcNow
        });
    }

    [HttpGet("anomalies")]
    public IActionResult Anomalies()
    {
        // Find measurements outside normal ranges
        var allResults = _context.MeasurementResults.Find(x => true).ToList();

        var anomalies = allResults
            .Where(x => x.MeasuredValue < x.LowerLimit || x.MeasuredValue > x.UpperLimit)
            .Select(x => new
            {
                x.Id,
                x.PartName,
                x.SerialNumber,
                x.MeasurementType,
                x.MeasuredValue,
                x.LowerLimit,
                x.UpperLimit,
                deviation = x.MeasuredValue < x.LowerLimit
                    ? x.LowerLimit - x.MeasuredValue
                    : x.MeasuredValue - x.UpperLimit,
                x.Status,
                x.MeasuredAt
            })
            .OrderByDescending(x => Math.Abs(x.deviation))
            .Take(20)
            .ToList();

        return Ok(new
        {
            anomalies = anomalies,
            totalAnomalies = anomalies.Count,
            generatedAt = DateTime.UtcNow
        });
    }

    [HttpGet("predict/{partName}")]
    public IActionResult Predict(string partName)
    {
        // Simple prediction based on historical data
        var partResults = _context.MeasurementResults
            .Find(x => x.PartName.ToLower() == partName.ToLower())
            .ToList();

        if (!partResults.Any())
        {
            return NotFound(new { message = $"No data found for part: {partName}" });
        }

        var avgValue = partResults.Average(x => x.MeasuredValue);
        var stdDev = Math.Sqrt(partResults.Sum(x => Math.Pow(x.MeasuredValue - avgValue, 2)) / partResults.Count);
        var passRate = (double)partResults.Count(x => x.Status == "PASS") / partResults.Count * 100;

        // Get typical limits from recent data
        var recentResults = partResults
            .OrderByDescending(x => x.MeasuredAt)
            .Take(10)
            .ToList();

        var typicalLower = recentResults.Min(x => x.LowerLimit);
        var typicalUpper = recentResults.Max(x => x.UpperLimit);

        return Ok(new
        {
            partName = partName,
            prediction = new
            {
                expectedValue = Math.Round(avgValue, 3),
                standardDeviation = Math.Round(stdDev, 3),
                confidenceRange = new
                {
                    lower = Math.Round(avgValue - stdDev, 3),
                    upper = Math.Round(avgValue + stdDev, 3)
                },
                typicalLimits = new
                {
                    lower = typicalLower,
                    upper = typicalUpper
                },
                historicalPassRate = Math.Round(passRate, 2)
            },
            dataPoints = partResults.Count,
            lastUpdated = partResults.Max(x => x.MeasuredAt),
            generatedAt = DateTime.UtcNow
        });
    }
}