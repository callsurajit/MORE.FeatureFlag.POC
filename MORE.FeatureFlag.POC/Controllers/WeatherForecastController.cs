using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace MORE.FeatureFlag.POC.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IFeatureManager _featureManager;

        public WeatherForecastController(
            IFeatureManager featureManager,
            ILogger<WeatherForecastController> logger) {
            _featureManager = featureManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get() {
            var isAdvancedEnable = await _featureManager.IsEnabledAsync("feature-poc");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                AdvancedEnabled = isAdvancedEnable ? true : false,
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();


        }

        [HttpGet("advanced")]
        [FeatureGate("feature-poc")]
        public IEnumerable<WeatherForecast> GetAdvanced() {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                AdvancedEnabled = true,
                RainExpected = Random.Shared.Next(0, 100).ToString() + " %",
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}