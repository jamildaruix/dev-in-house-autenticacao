using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.RegularExpressions;

namespace Exercicios.Controllers
{
    /// <summary>
    ///  Microsoft.AspNetCore.Http.StatusCode
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StatusCodeController : ControllerBase
    {
        private readonly ILogger<StatusCodeController> logger;

        public StatusCodeController(ILogger<StatusCodeController> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 1xx Informatinal
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExemploInformation")]
        public IActionResult ExemploInformation()
        {
            return StatusCode(100);
        }

        /// <summary>
        /// 2xx Success
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExemploOkSuccess")]
        public IActionResult ExemploOkSuccess([FromQuery] string valor)
        {
            try
            {
                return Ok(new { ferramenta = "VS CODE" });
            }
            catch (Exception ex)
            {
                logger.LogError("Erro:", ex);
                return BadRequest(new { erro = "Erro na aplicação" });
            }
        }


        /// <summary>
        /// 3xx Redirection
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExemploRedirection")]
        public IActionResult ExemploRedirection()
        {
            return StatusCode(StatusCodes.Status301MovedPermanently);
        }


        /// <summary>
        /// 4xx Client Error
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExemploClientError")]
        public IActionResult ExemploClientError()
        {
            return StatusCode(StatusCodes.Status414UriTooLong);
        }


        /// <summary>
        /// 5xx Server Error
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExemploServerError")]
        public IActionResult ExemploServerError()
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }


        [HttpGet("ExemploStatusCodeLargo")]
        public IActionResult ExemploStatusCodeLargo(int valor)
        {
            if (valor > 100)
            {
                return StatusCode(HttpStatusCode.RequestEntityTooLarge.GetHashCode());
            }
            else
            {
                return Ok();
            }
            
        }
    }
}
