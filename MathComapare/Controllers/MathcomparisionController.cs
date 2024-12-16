using MathComapare.Models;
using Microsoft.AspNetCore.Mvc;

namespace MathComapare.Controllers
{
    [ApiController]
    [Route("api/math")]
    public class MathcomparisionController : ControllerBase
    {
        private readonly IMathExpressionService _service;

        public MathcomparisionController(IMathExpressionService service)
        {
            _service = service;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> Generate([FromQuery] string difficulty)
        {
            try
            {
                var data = await _service.GenerateExpressions(difficulty);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("compare")]
        public async Task<IActionResult> Compare(ComparisionRequest request)
        {
            try
            {
                var valid = await _service.EvaluateComparison(request);
                return Ok(valid);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
