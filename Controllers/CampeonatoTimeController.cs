using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CampeonatoTimeController : ControllerBase
    {
        private readonly ICampeonatoTimeService _campeonatoTimeService;
        public CampeonatoTimeController(ICampeonatoTimeService campeonatoTimeService)
        {
            _campeonatoTimeService = campeonatoTimeService;
        }
        [HttpPost("cadastrar-campeonato-time")]
        public async Task<IActionResult> CadastrarCampeonatoTime([FromBody] CampeonatoTimeModel req)
        {
            try
            {
                var campeonatoTime = await _campeonatoTimeService.CadastrarCampeonatoTime(req);
                return Ok(campeonatoTime);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        
        [HttpGet("listar-times-campeonato/{campeonatoId:long}")]
        public async Task<IActionResult> ListaTimesCampeonato(long campeonatoId)
        {
            try
            {
                var times = await _campeonatoTimeService.ListaTimesCampeonato(campeonatoId);

                return Ok(times);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPut("atualizar-grupo")]
        public async Task<IActionResult> AtualizaGrupo(
            [FromBody] CampeonatoTimeModel req)
        {
            try
            {
                await _campeonatoTimeService.AtualizaGrupo(req);

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


        [HttpDelete("excluir-campeonato-time/{campeonatoId:long}/{timeId:long}")]
        public async Task<IActionResult> ExcluiCampeonatoTime(
            long campeonatoId,
            long timeId)
        {
            try
            {
                await _campeonatoTimeService.ExcluiCampeonatoTime(
                    campeonatoId,
                    timeId
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
    }
}
