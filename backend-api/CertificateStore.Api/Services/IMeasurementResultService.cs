using CertificateStore.Api.Models;

namespace CertificateStore.Api.Services;

public interface IMeasurementResultService
{
    PagedResult<MeasurementResult> GetAll(int pageNumber, int pageSize);

    MeasurementResult? GetById(string id);

    MeasurementResult Create(MeasurementResult result);

    MeasurementResult? Update(string id, MeasurementResult result);

    bool Delete(string id);
}