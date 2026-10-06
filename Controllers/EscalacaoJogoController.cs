using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EscalacaoJogoController : BaseController
    {
        private readonly IEscalacaoJogoService _service;

        public EscalacaoJogoController(IEscalacaoJogoService service)
        {
            _service = service;
        }

        [HttpPost("salvar")]
        public async Task<IActionResult> Salvar([FromBody] EscalacaoJogoModel req)
        {
            try
            {
                var item = await _service.Salvar(req);
                return Ok(item);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("salvar-time")]
        public async Task<IActionResult> SalvarTime([FromBody] SalvarEscalacaoTimeRequest req)
        {
            try
            {
                await _service.SalvarTime(req);
                var lista = await _service.ListarPorTime(req.PartidaId, req.TimeId);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("listar-time/{partidaId:long}/{timeId:long}")]
        public async Task<IActionResult> ListarPorTime(long partidaId, long timeId)
        {
            try
            {
                var lista = await _service.ListarPorTime(partidaId, timeId);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("listar-partida/{partidaId:long}")]
        public async Task<IActionResult> ListarPorPartida(long partidaId)
        {
            try
            {
                var lista = await _service.ListarPorPartida(partidaId);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("minuto-saida/{id:long}")]
        public async Task<IActionResult> RegistrarSaida(long id, [FromQuery] short minuto)
        {
            try
            {
                await _service.RegistrarSaida(id, minuto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("minuto-entrada/{id:long}")]
        public async Task<IActionResult> RegistrarEntrada(long id, [FromQuery] short minuto)
        {
            try
            {
                await _service.RegistrarEntrada(id, minuto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("jogos-titular/{jogadorId:long}/{campeonatoId:long}")]
        public async Task<IActionResult> JogosTitular(long jogadorId, long campeonatoId)
        {
            try
            {
                var total = await _service.ContarJogosTitular(jogadorId, campeonatoId);
                return Ok(new { jogosTitular = total });
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
