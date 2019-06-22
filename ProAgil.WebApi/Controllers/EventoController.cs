using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repo;
        public EventoController(IProAgilRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _repo.GetAllEventoAsync(true));
            }
            catch (System.Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro no banco de dados");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _repo.GetEventoAsyncById(id,true));
            }
            catch (System.Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro no banco de dados");
            }
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                return Ok(await _repo.GetAllEventoAsyncByTema(tema,true));
            }
            catch (System.Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro no banco de dados");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post(Evento evento)
        {
            try
            {
                _repo.Add(evento);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{evento.Id}",evento);
                }
            }
            catch (System.Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro no banco de dados");
            }
            
            return BadRequest();
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> Put(int eventoId, Evento evento)
        {
            try
            {
                var eventoDB = await _repo.GetEventoAsyncById(eventoId,false);
                if (eventoDB == null) return NotFound();

                _repo.Update(evento);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{evento.Id}",evento);
                }
            }
            catch (System.Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro no banco de dados");
            }
            
            return BadRequest();
        }


        [HttpDelete("{eventoId}")]
        public async Task<IActionResult> Delete(int eventoId)
        {
            try
            {
                var eventoDB = await _repo.GetEventoAsyncById(eventoId,false);
                if (eventoDB == null) return NotFound();

                _repo.Delete(eventoDB);
                if (await _repo.SaveChangesAsync()) return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro no banco de dados");
            }
            
            return BadRequest();
        }
    }

}