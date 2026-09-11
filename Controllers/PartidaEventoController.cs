using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PartidaEventoController : BaseController
    {
        private readonly IPartidaEventoService _partidaEventoService;

        public PartidaEventoController(IPartidaEventoService partidaEventoService)
        {
            _partidaEventoService = partidaEventoService;
        }

        [HttpPost("cadastrar-partida-evento")]
        public async Task<IActionResult> CadastraPartidaEvento([FromBody] PartidaEventoModel req)
        {
            try
            {
                var partidaEvento = await _partidaEventoService.CadastrarPartidaEvento(req);
                return Ok(partidaEvento);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        
        [HttpGet("listar-eventos-partida/{partidaId:long}")]
        public async Task<IActionResult> ListaEventosPartida(long partidaId)
        {
            try
            {
                var eventos = await _partidaEventoService.ListaEventosPartida(partidaId);

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpGet("listar-artilharia/{campeonatoId:long}")]
        public async Task<IActionResult> ListaArtilharia(long campeonatoId)
        {
            try
            {
                var artilharia = await _partidaEventoService.ListaArtilharia(campeonatoId);

                return Ok(artilharia);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPut("atualizar-partida-evento")]
        public async Task<IActionResult> AtualizaPartidaEvento(
            [FromBody] PartidaEventoModel req)
        {
            try
            {
                await _partidaEventoService.AtualizaPartidaEvento(req);

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


        [HttpDelete("excluir-partida-evento/{id:long}")]
        public async Task<IActionResult> ExcluiPartidaEvento(long id)
        {
            try
            {
                await _partidaEventoService.ExcluiPartidaEvento(id);

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
