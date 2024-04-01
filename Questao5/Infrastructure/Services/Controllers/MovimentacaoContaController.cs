using Microsoft.AspNetCore.Mvc;
using static Questao5.Domain.MovimentacaoModels;

namespace SeuNamespace.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovimentacaoContaController : ControllerBase
    {
        private readonly MovimentacaoContaService _movimentacaoService;

        public MovimentacaoContaController(MovimentacaoContaService movimentacaoService)
        {
            _movimentacaoService = movimentacaoService;
        }

        [HttpPost("movimentar")]
        public IActionResult MovimentarConta([FromBody] MovimentacaoRequest request)
        {
            try
            {
                var response = _movimentacaoService.MovimentarConta(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
