using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarioJogoController : BaseController
    {
        private readonly ICalendarioJogoService _service;

        public CalendarioJogoController(ICalendarioJogoService service)
        {
            _service = service;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] CalendarioJogoModel req)
        {
            try
            {
                var jogo = await _service.Cadastrar(req);
                return Ok(jogo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("listar")]
        public async Task<IActionResult> Listar([FromQuery] long? campeonatoId, [FromQuery] long? timeId)
        {
            try
            {
                var jogos = await _service.Listar(campeonatoId, timeId);
                return Ok(jogos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("consultar/{id:long}")]
        public async Task<IActionResult> Consultar(long id)
        {
            try
            {
                var jogo = await _service.Consultar(id);
                if (jogo is null)
                    return NotFound(new { message = "Jogo do calendário não encontrado." });

                return Ok(jogo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("confirmar/{id:long}")]
        public async Task<IActionResult> Confirmar(long id, [FromBody] ConfirmarCalendarioRequest req)
        {
            try
            {
                await _service.Confirmar(id, req);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("vincular-partida/{id:long}")]
        public async Task<IActionResult> VincularPartida(long id, [FromBody] VincularPartidaRequest req)
        {
            try
            {
                await _service.VincularPartida(id, req.PartidaId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("atualizar-status/{id:long}")]
        public async Task<IActionResult> AtualizarStatus(long id, [FromBody] AtualizarStatusCalendarioRequest req)
        {
            try
            {
                await _service.AtualizarStatus(id, req);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("oficializar/{id:long}")]
        public async Task<IActionResult> Oficializar(long id)
        {
            try
            {
                var jogo = await _service.Oficializar(id);
                return Ok(jogo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("excluir/{id:long}")]
        public async Task<IActionResult> Excluir(long id)
        {
            try
            {
                await _service.Excluir(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
