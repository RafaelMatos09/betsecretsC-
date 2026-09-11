using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PartidaController : BaseController
    {
        private readonly IPartidaService _partidaService;

        public PartidaController(IPartidaService partidaService)
        {
            _partidaService = partidaService;
        }

        [HttpPost("cadastrar-partida")]
        public async Task<IActionResult> CadastraPartida([FromBody] PartidaModel req)
        {
            try
            {
                var partida = await _partidaService.CadastrarPartida(req);
                return Ok(partida);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
       
        [HttpGet("consultar-partida/{id:long}")]
        public async Task<IActionResult> ConsultaPartida(long id)
        {
            try
            {
                var partida = await _partidaService.ConsultaPartida(id);

                return Ok(partida);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpGet("listar-partidas-campeonato/{campeonatoId:long}")]
        public async Task<IActionResult> ListaPartidasCampeonato(
            long campeonatoId,
            [FromQuery] long? rodadaId)
        {
            try
            {
                var partidas = await _partidaService.ListaPartidasCampeonato(
                    campeonatoId,
                    rodadaId
                );

                return Ok(partidas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPut("registrar-resultado/{id:long}")]
        public async Task<IActionResult> RegistraResultado(
            long id,
            [FromQuery] int golsCasa,
            [FromQuery] int golsVisitante)
        {
            try
            {
                await _partidaService.RegistraResultado(
                    id,
                    golsCasa,
                    golsVisitante
                );

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


        [HttpPut("reagendar-partida/{id:long}")]
        public async Task<IActionResult> ReagendaPartida(
            long id,
            [FromQuery] DateTime dataHora)
        {
            try
            {
                await _partidaService.ReagendaPartida(id, dataHora);

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


        [HttpDelete("excluir-partida/{id:long}")]
        public async Task<IActionResult> ExcluiPartida(long id)
        {
            try
            {
                await _partidaService.ExcluiPartida(id);

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
