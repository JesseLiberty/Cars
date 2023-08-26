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
        public async Task<ActionResult<Car>> Get(int id)
        {
            var car = await carRepository.Get(id);
            if (car == null)
            {
                return NotFound();
            }
            return car;
        }
        
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Car car)
        {
            if (car.Id == 0)
            {
                return BadRequest("Id must be set");
            }

            await carRepository.UpsertAsync(car);
            return NoContent();
        }
        
        [HttpPost]
        public async Task<ActionResult<Car>> Post([FromBody] Car car)
        {
            var newId = await carRepository.UpsertAsync(car);
            if (newId > 0)
            {
                car.Id = newId;
            }
            return CreatedAtAction(nameof(Get), new { id = car.Id }, car);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int affectedRows = await carRepository.DeleteAsync(id);
            if (affectedRows == 0)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}