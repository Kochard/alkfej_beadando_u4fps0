using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CertificateStore.Mcp.Models;

public class MeasurementResult
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string PartName { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string MeasurementType { get; set; } = string.Empty;
    public double MeasuredValue { get; set; }
    public string Unit { get; set; } = string.Empty;
    public double LowerLimit { get; set; }
    public double UpperLimit { get; set; }
    public string Status { get; set; } = string.Empty;
    public string MeasuredBy { get; set; } = string.Empty;
    public DateTime MeasuredAt { get; set; }
    public string Notes { get; set; } = string.Empty;
}