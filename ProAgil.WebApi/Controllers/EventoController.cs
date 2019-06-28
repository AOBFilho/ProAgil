using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.WebApi.Dtos;

namespace ProAgil.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IProAgilRepository _repo;
        private readonly IMapper _mapper;
        public EventoController(IProAgilRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _repo.GetAllEventoAsync(true);
                var result = _mapper.Map<IEnumerable<EventoDto>>(eventos); 
                return Ok(result);
            }
            catch (System.Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro no banco de dados");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evento = await _repo.GetEventoAsyncById(id, true);
                var result = _mapper.Map<EventoDto>(evento); 
                return Ok(result);
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
                var eventos = await _repo.GetAllEventoAsyncByTema(tema, true);
                var result = _mapper.Map<IEnumerable<EventoDto>>(eventos); 
                return Ok(result);
            }
            catch (System.Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro no banco de dados");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                _repo.Add(evento);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
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
        public async Task<IActionResult> Put(int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _repo.GetEventoAsyncById(eventoId, false);
                if (evento == null) return NotFound();
                _mapper.Map(model,evento);
                _repo.Update(evento);
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{evento.Id}", _mapper.Map<EventoDto>(evento));
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
                var eventoDB = await _repo.GetEventoAsyncById(eventoId, false);
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