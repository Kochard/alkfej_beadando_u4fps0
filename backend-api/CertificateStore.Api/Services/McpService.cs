using System.Text.Json;

namespace CertificateStore.Api.Services;

public interface IMcpService
{
    Task<object?> GetStatsAsync();
    Task<object?> GetInsightsAsync();
    Task<object?> GetLatestAsync(int count = 5);
    Task<object?> GetAnomaliesAsync();
    Task<object?> PredictAsync(string partName);
}

public class McpService : IMcpService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<McpService> _logger;

    public McpService(HttpClient httpClient, ILogger<McpService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<object?> GetStatsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/mcp/stats");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling MCP stats endpoint");
        }
        return null;
    }

    public async Task<object?> GetInsightsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/mcp/insights");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling MCP insights endpoint");
        }
        return null;
    }

    public async Task<object?> GetLatestAsync(int count = 5)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/mcp/latest?count={count}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling MCP latest endpoint");
        }
        return null;
    }

    public async Task<object?> GetAnomaliesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/mcp/anomalies");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling MCP anomalies endpoint");
        }
        return null;
    }

    public async Task<object?> PredictAsync(string partName)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/mcp/predict/{partName}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling MCP predict endpoint for {PartName}", partName);
        }
        return null;
    }
}