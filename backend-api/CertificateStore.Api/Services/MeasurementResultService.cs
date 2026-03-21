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

    public PagedResult<MeasurementResult> GetAll(int pageNumber, int pageSize)
    {
        var totalCount = (int)_context.MeasurementResults.CountDocuments(_ => true);

        var items = _context.MeasurementResults
            .Find(_ => true)
            .SortByDescending(r => r.MeasuredAt)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToList();

        var totalPages = totalCount == 0
            ? 1
            : (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<MeasurementResult>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
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