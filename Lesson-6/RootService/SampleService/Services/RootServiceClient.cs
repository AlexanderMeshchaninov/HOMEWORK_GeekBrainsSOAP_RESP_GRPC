using RootServiceReference;
using SampleService.Core;

namespace SampleService.Services
{
    public interface IRootServiceClient : IRootServiceClient<WeatherForecast>
    { }
    public class RootServiceClient : IRootServiceClient
    {
        public RootServiceReference.RootServiceClient Client => _httpClient;
        private readonly RootServiceReference.RootServiceClient _httpClient;
        private readonly ILogger _logger;

        public RootServiceClient(HttpClient httpClient, ILogger<RootServiceClient> logger)
        {
            _httpClient = new RootServiceReference.RootServiceClient("http://localhost:5228/", httpClient);
            _logger = logger;
        }

        public async Task<ICollection<WeatherForecast>> Get()
        {
            return await _httpClient.GetWeatherForecastAsync();
        }
    }
}
