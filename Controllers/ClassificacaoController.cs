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
        
        [HttpGet("listar-classificacao/{campeonatoId:long}")]
        public async Task<IActionResult> ListaClassificacao(long campeonatoId)
        {
            try
            {
                var classificacao = await _classificacaoService.ListaClassificacao(
                    campeonatoId
                );

                return Ok(classificacao);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPut("recalcular-posicoes/{campeonatoId:long}")]
        public async Task<IActionResult> RecalculaPosicoes(long campeonatoId)
        {
            try
            {
                await _classificacaoService.RecalculaPosicoes(campeonatoId);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }
    }
}
