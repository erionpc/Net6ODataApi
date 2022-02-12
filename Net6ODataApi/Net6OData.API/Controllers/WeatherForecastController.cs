using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Swashbuckle.AspNetCore.Annotations;

namespace Net6OData.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static IEnumerable<WeatherForecast> SampleData = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Id = index,
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns sample weather forecast data
        /// </summary>
        /// <example>
        /// https://localhost:7023/WeatherForecast?$select=date,temperatureC,summary&$top=2
        /// </example>
        [EnableQuery]
        [HttpGet]
        [SwaggerOperation(summary: "Get weather forecasts", description: "Get sample weather forecasts")]
        public IActionResult GetAsync() =>
            Ok(SampleData);

        /// <summary>
        /// Modifies an existing weather forecast
        /// </summary>
        /// <example>
        /// HTTP PATCH: https://localhost:7023/WeatherForecast/1
        /// [
        ///   {
        ///     "path": "/summary",
        ///     "op": "replace",
        ///     "value": "patched summary"
        ///   }
        /// ]
        /// </example>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [SwaggerOperation(summary: "Patch  weather forecast", description: "Modify existing weather forecast")]
        public IActionResult PatchAsync(int id, [FromBody] JsonPatchDocument<WeatherForecast> patchDocument)
        {
            var forecastToModify = SampleData.FirstOrDefault(x => x.Id == id);
            if (forecastToModify == null)
                return NotFound();

            patchDocument.ApplyTo(forecastToModify);

            return NoContent();
        }
    }
}