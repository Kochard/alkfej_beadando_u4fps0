using CertificateStore.Api.Data;
using CertificateStore.Api.Models;
using MongoDB.Driver;

namespace CertificateStore.Api.Services;

public class MeasurementResultService : IMeasurementResultService
{
    private readonly MongoDbContext _context;

    public MeasurementResultService(MongoDbContext context)
    {
        _context = context;
    }

    public List<MeasurementResult> GetAll(int pageNumber, int pageSize)
    {
        return _context.MeasurementResults
            .Find(_ => true)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToList();
    }

    public MeasurementResult? GetById(string id)
    {
        return _context.MeasurementResults.Find(r => r.Id == id).FirstOrDefault();
    }

    public MeasurementResult Create(MeasurementResult result)
    {
        _context.MeasurementResults.InsertOne(result);
        return result;
    }

    public MeasurementResult? Update(string id, MeasurementResult result)
    {
        var updateResult = _context.MeasurementResults.ReplaceOne(r => r.Id == id, result);

        if (updateResult.MatchedCount == 0)
        {
            return null;
        }

        return result;
    }

    public bool Delete(string id)
    {
        var deleteResult = _context.MeasurementResults.DeleteOne(r => r.Id == id);
        return deleteResult.DeletedCount > 0;
    }
}