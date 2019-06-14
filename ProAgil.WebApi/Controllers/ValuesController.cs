using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProAgil.WebApi.Data;
using ProAgil.WebApi.Models;

namespace ProAgil.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public readonly DataContext _context;
        public ValuesController(DataContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _context.Eventos.ToListAsync(); 
                return Ok(results);    
            }
            catch (System.Exception)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Ocorreu uma falha inesperada ao tentar acessar o banco de dados!"
                );
            }
            
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var results = await _context.Eventos.FirstOrDefaultAsync(x => x.EventoId == id);
                return Ok(results);    
            }
            catch (System.Exception)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Ocorreu uma falha inesperada ao tentar acessar o banco de dados!"
                );
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
