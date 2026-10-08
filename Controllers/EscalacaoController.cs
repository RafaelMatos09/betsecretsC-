using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EscalacaoController : BaseController
    {
        private readonly IEscalacaoService _escalacaoService;
        public EscalacaoController(IEscalacaoService escalacaoService)
        {
            _escalacaoService = escalacaoService;
        }
        [HttpPost("cadastrar-escalacao")]
        public async Task<IActionResult> CadastraEscalacao([FromBody] EscalacaoModel req)
        {
            try
            {
                var escalacao = await _escalacaoService.CadastrarEscalacao(req);
                return Ok(escalacao);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
