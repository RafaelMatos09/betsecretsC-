using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RelatorioController : BaseController
    {
        private readonly IRelatorioService _service;

        public RelatorioController(IRelatorioService service)
        {
            _service = service;
        }

        [HttpGet("jogadores")]
        public async Task<IActionResult> Jogadores([FromQuery] long? campeonatoId, [FromQuery] long? timeId)
        {
            try
            {
                var lista = await _service.ListarJogadores(campeonatoId, timeId);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("estatisticas")]
        public async Task<IActionResult> Estatisticas([FromQuery] long? campeonatoId, [FromQuery] long? timeId)
        {
            try
            {
                var lista = await _service.ListarEstatisticas(campeonatoId, timeId);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("salvar-estatistica")]
        public async Task<IActionResult> SalvarEstatistica([FromBody] SalvarEstatisticaRequest req)
        {
            try
            {
                await _service.SalvarEstatistica(req);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
