using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Exercicios.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIKeyExemploController : ControllerBase
    {
        [HttpGet()]
        public ActionResult Get([FromHeader(Name = "apikey")][Required] string requiredHeader)
        {
            return Ok("Acessou com sucesso método que válida a api key");
        }
    }
}
