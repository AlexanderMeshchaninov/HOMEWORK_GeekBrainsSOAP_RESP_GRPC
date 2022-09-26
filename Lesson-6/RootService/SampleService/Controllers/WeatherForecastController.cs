using Microsoft.AspNetCore.Mvc;
using RootServiceReference;
using SampleService.Services;

namespace SampleService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IRootServiceClient _rootServiceClient;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            IRootServiceClient rootServiceClient,
            ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _rootServiceClient = rootServiceClient;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {
            return Ok(await _rootServiceClient.Get());
        }
    }
}