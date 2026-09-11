using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using betsecrets.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JogadorController : BaseController
    {
        private readonly IJogadorService _jogadorService;

        public JogadorController(IJogadorService jogadorService)
        {
            _jogadorService = jogadorService;
        }

        [HttpPost("cadastra-jogador")]        
        public async Task<IActionResult> CadastraJogador([FromBody] JogadorModel req)
        {
            try
            {
                var bairro = await _jogadorService.CadastraJogador(req);
                return Ok(bairro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("consultar-jogador/{id}")]
        public async Task<IActionResult> ConsultaJogador(long id)
        {
            try
            {
                var jogador = await _jogadorService.ConsultaJogador(id);
                return Ok(jogador);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("buscar-jogadores")]
        public async Task<IActionResult> BuscaJogadores([FromQuery] string nome)
        {
            try
            {
                var jogadores = await _jogadorService.BuscaJogadores(nome);
                return Ok(jogadores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        
        [HttpPut("atualizar-jogador")]
        public async Task<IActionResult> AtualizaJogador([FromBody] JogadorModel req)
        {
            try
            {
                await _jogadorService.AtualizaJogador(req);

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

        [HttpDelete("excluir-jogador/{id}")]
        public async Task<IActionResult> ExcluiJogador(long id)
        {
            try
            {
                await _jogadorService.ExcluiJogador(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
