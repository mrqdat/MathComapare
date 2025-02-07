using AspNetCoreRateLimit;
using MathComapare.Entities;
using MathComapare.Interfaces;
using MathComapare.Models;
using MathComapare.Repositories.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace MathComapare.Controllers
{
    [EnableCors("AllowSpecificOrigins")]
    [ApiController]
    [Route("api/math")]
    public class MathcomparisionController : ControllerBase
    {
        private readonly IMathExpressionService _service;
        private readonly ILogger<MathcomparisionController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;       
        private readonly IMathCompareRepositories _repositories;

        public MathcomparisionController(IMathExpressionService service
            , ILogger<MathcomparisionController> logger
            , IHttpContextAccessor contextAccessor
            
            ,IMathCompareRepositories repositories)
        {
            _service = service;
            _logger = logger;
            _contextAccessor = contextAccessor; 
            _repositories = repositories;
        }

        [HttpGet("generate")]       
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
        public async Task<IActionResult> Compare(ComparisionRequest request)
        {
            try
            {
                var context = _contextAccessor.HttpContext;
                var valid = await _service.EvaluateComparison(request);
                string message = $"Request: {context?.Request.Method} {context?.Request.Path} {context?.Request.Body}";
                _logger.LogInformation($"{valid.ToString()} {message}");
                return Ok(valid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Compare));
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser( Guid user_id)
        {
            var user = await _repositories.GetUserInfoAsync(user_id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] Users request)
        {
            try
            {
                await _repositories.RegisterUserAsync(request);
                return CreatedAtAction(nameof(GetUser), new { id = request.UserId }, request);
            }
            catch(Exception ex) {
                Console.Write(ex.InnerException?.Message ?? ex.Message);
                throw; 
            }
        }
    }
}
