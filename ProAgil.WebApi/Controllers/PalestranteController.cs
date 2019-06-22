using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PalestranteController : ControllerBase
    {
        private readonly IProAgilRepository _repo;

        public PalestranteController(IProAgilRepository repo)
        {
            _repo = repo;    
        }

        [HttpGet("/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _repo.GetPalestranteAsyncById(id,false));
            }
            catch (System.Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro no banco de dados!");
            }
        }

        [HttpGet("/getByName/{nome}")]
        public async Task<IActionResult> GetByNome(string nome)
        {
            try 
            {
                return Ok(await _repo.GetAllPalestrantesAsyncByName(nome,false));
            } catch (System.Exception)
            {   
                return StatusCode(
                            StatusCodes.Status500InternalServerError,
                            "Ocorre um erro no banco de dados!");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Put(Palestrante palestrante)
        {
            try
            {
                _repo.Add(palestrante);
                if (await _repo.SaveChangesAsync()) 
                {
                    return Created($"/api/palestrante/{palestrante.Id}",palestrante);
                }
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Ocorreu um erro no banco de dados!");
            }

            return BadRequest();
        }

        [HttpPut("{palestranteId}")]
        public async Task<IActionResult> Put(int palestranteId, Palestrante palestrante)
        {
            try
            {
                var palestranteDB = await _repo.GetPalestranteAsyncById(palestranteId,false);
                if (palestranteDB == null) return NotFound();

                _repo.Update(palestrante);
                 if (await _repo.SaveChangesAsync()) 
                {
                    return Created($"/api/palestrante/{palestrante.Id}",palestrante);
                }
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Ocorreu um erro no banco de dados!");
            }

            return BadRequest();
        }

        [HttpDelete("{palestranteId}")]
        public async Task<IActionResult> Delete(int palestranteId)
        {
            try
            {
                var palestranteDB = await _repo.GetPalestranteAsyncById(palestranteId,false);
                if (palestranteDB == null) return NotFound();

                _repo.Delete(palestranteDB);
                if (await _repo.SaveChangesAsync()) return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Ocorreu um erro no banco de dados!");
            }

            return BadRequest();
        }
    }
}