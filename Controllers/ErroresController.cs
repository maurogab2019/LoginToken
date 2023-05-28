using api_ejemplar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_ejemplar.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ErroresController : ControllerBase
    {
        public readonly ApplicationDbContext db;
        // GET: api/<ErroresController>
        public ErroresController()
        {
            db = new ApplicationDbContext();
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { listaErrores= db.ERRORES.ToList()});
        }

        // GET api/<ErroresController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "goalsd";
        }

        // POST api/<ErroresController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ErroresController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ErroresController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
