using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JogadorTimeController : ControllerBase
    {
        private readonly IJogadorTimeService _jogadorTimeService;
        public JogadorTimeController(IJogadorTimeService jogadorTimeService)
        {
            _jogadorTimeService = jogadorTimeService;
        }

        [HttpPost("cadastrar-jogador-time")]
        public async Task<IActionResult> CadastraJogadorTime([FromBody] JogadorTimeModel req)
        {
            try
            {
                var jogadorTime = await _jogadorTimeService.CadastrarJogadorTime(req);
                return Ok(jogadorTime);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        
        [HttpGet("listar-elenco/{timeId:long}")]
        public async Task<IActionResult> ListaElenco(long timeId)
        {
            try
            {
                var elenco = await _jogadorTimeService.ListaElenco(timeId);

                return Ok(elenco);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpGet("listar-historico-jogador/{jogadorId:long}")]
        public async Task<IActionResult> ListaHistoricoJogador(long jogadorId)
        {
            try
            {
                var historico = await _jogadorTimeService.ListaHistoricoJogador(jogadorId);

                return Ok(historico);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPut("encerrar-vinculo/{id:long}")]
        public async Task<IActionResult> EncerraVinculo(
            long id,
            [FromQuery] DateTime dataFim)
        {
            try
            {
                await _jogadorTimeService.EncerraVinculo(id, dataFim);

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
