using AbpCoreWebAPI.AppServices;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Threading;

namespace AbpCoreWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDummyAppService _dummyService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDummyAppService dummyService)
        {
            _logger = logger;
            _dummyService = dummyService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            string msg = await _dummyService.HelloAsync("GetWeatherForecast");
            _logger.LogInformation(msg);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}