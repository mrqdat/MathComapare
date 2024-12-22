using AspNetCoreRateLimit;
using MathComapare.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MathComapare.Controllers
{
    [ApiController]
    [Route("api/math")]
    public class MathcomparisionController : ControllerBase
    {
        private readonly IMathExpressionService _service;
        private readonly ILogger<MathcomparisionController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public MathcomparisionController(IMathExpressionService service
            , ILogger<MathcomparisionController> logger
            , IHttpContextAccessor contextAccessor)
        {
            _service = service;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("generate")]
        [EnableCors("AllowSpecificOrigins")]
        public async Task<IActionResult> Generate([FromQuery] int difficulty)
        {
            try
            {
                var context = _contextAccessor.HttpContext;
                var data = await _service.GenerateExpressions(difficulty);
                string message = $"Request: {context?.Request.Method} {context?.Request.Path} {context?.Request.Body}";
                _logger.LogInformation(message);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Generate));
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("compare")]
        [EnableCors("AllowSpecificOrigins")]
        public async Task<IActionResult> Compare(ComparisionRequest request)
        {
            try
            {
                var context = _contextAccessor.HttpContext;
                var valid = await _service.EvaluateComparison(request);
                string message = $"Request: {context?.Request.Method} {context?.Request.Path} {context?.Request.Body}";
                _logger.LogInformation(message);
                return Ok(valid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Compare));
                return BadRequest(ex.Message);
            }
        }
    }
}
