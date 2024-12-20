using AspNetCoreRateLimit;
using MathComapare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MathComapare.Controllers
{
    [ApiController]
    [Route("api/math")]
    public class MathcomparisionController : ControllerBase
    {
        private readonly IMathExpressionService _service;
        private readonly ILogger<MathcomparisionController> _logger;

        public MathcomparisionController(IMathExpressionService service, ILogger<MathcomparisionController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("generate")]
        //[ServiceFilter(ralimitfiler)]
        public async Task<IActionResult> Generate([FromQuery] int difficulty)
        {
            try
            {
                var data = await _service.GenerateExpressions(difficulty);
                _logger.LogInformation("{ data }", data);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("compare")]
        public async Task<IActionResult> Compare(ComparisionRequest request)
        {
            try
            {
                var valid = await _service.EvaluateComparison(request);
                _logger.LogInformation("{ data }", valid);
                return Ok(valid);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
