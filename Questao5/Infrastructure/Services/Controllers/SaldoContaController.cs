using Microsoft.AspNetCore.Mvc;
using Questao5.Domain;


namespace SeuNamespace.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SaldoContaController : ControllerBase
    {
        private readonly SaldoContaService _saldoService;

        public SaldoContaController(SaldoContaService saldoService)
        {
            _saldoService = saldoService;
        }

        [HttpGet("{identificacaoContaCorrente}/saldo")]
        public IActionResult ConsultarSaldo(string identificacaoContaCorrente)
        {
            try
            {
                var request = new SaldoRequest { IdentificacaoContaCorrente = identificacaoContaCorrente };
                var saldo = _saldoService.ConsultarSaldo(request);
                return Ok(saldo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
