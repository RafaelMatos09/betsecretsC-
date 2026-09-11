using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    public class ClassificacaoController : BaseController
    {
        private readonly IClassificacaoService _classificacaoService;
        public ClassificacaoController(IClassificacaoService classificacaoService)
        {
            _classificacaoService = classificacaoService;
        }

        [HttpPost("cadastrar-classificacao")]
        public async Task<IActionResult> CadastraClassificacao([FromBody] ClassificacaoModel req)
        {
            try
            {
                var classificacao = await _classificacaoService.CadastrarClassificacao(req);
                return Ok(classificacao);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
