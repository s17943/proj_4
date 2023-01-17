
using Microsoft.AspNetCore.Mvc;
using PROJ_4.Models;
using PROJ_4.Services;
using PROJ_4.Services.Exceptions;



namespace PROJ_4.Controllers
{
    [Route("api/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly IDBService DBService;

        public AnimalsController(IDBService DBService)
        {
            this.DBService = DBService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnimals([FromQuery] string orderBy)
        {
            List<Animal> animals = null;
            try { animals = DBService.GetAnimals(orderBy); }
            catch (DbIsEmptyException) { return NotFound("Database appears to be empty"); }
            catch (NoMatchingColumnException) { return BadRequest("Data can only be ordered by name, description, category and area"); }
            return Ok(animals);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnimal([FromBody] Animal animal)
        {
            try { DBService.CreateAnimal(animal); }
            catch (Exception) { return BadRequest("Invalid request"); }
            return Ok("Entry added");
        }

        [HttpPut("{idAnimal}")]
        public async Task<IActionResult> ChangeAnimal([FromRoute] int idAnimal, [FromBody] Animal animal)
        {
            try { DBService.ChangeAnimal(idAnimal, animal); }
            catch (NoQueryException) { return NotFound($"There is no entry with the following id: {idAnimal}!"); }
            catch (Exception) { return BadRequest("Invalid request"); }
            return Ok("Entry updated");
        }

        [HttpDelete("{idAnimal}")]
        public async Task<IActionResult> DeleteAnimal([FromRoute] int idAnimal)
        {
            try { DBService.DeleteAnimal(idAnimal); }
            catch (NoQueryException) { return NotFound($"No such animal found with ID {idAnimal}!"); }
            catch (Exception) { return BadRequest("Invalid request"); }
            return Ok("Entry deleted");
        }


    }
}