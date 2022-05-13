using Microsoft.AspNetCore.Mvc;

namespace Exercicios.Controllers
{
    /// <summary>
    ///  Microsoft.AspNetCore.Http.StatusCode
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StatusCodeController : ControllerBase
    {

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
        public IActionResult ExemploOkSuccess()
        {
            return Ok();
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
    }
}
