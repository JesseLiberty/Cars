using Cars.Data.Entities;
using Cars.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ILogger<CarController> _logger;
        readonly CarRepository carRepository;

        public CarController(ILogger<CarController> logger,
            CarRepository carRepository)
        {
            _logger = logger;
            this.carRepository = carRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Car>> GetAll(bool returnDeletedRecords = false)
        {
            return await carRepository.GetAll(returnDeletedRecords);
        }
        
        [HttpGet("{id}")]
        public async Task<Car> Get(int id)
        {
            // TODO: return 404 if not found?
            return await carRepository.Get(id);
        }
        
    }
}