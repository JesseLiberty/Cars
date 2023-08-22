using Cars.Data.Entities;
using Cars.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers
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
        readonly CarRepository carRepository;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            CarRepository carRepository)
        {
            _logger = logger;
            this.carRepository = carRepository;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<Car>> Get()
        {
            return await carRepository.GetAll();
        }
    }
}