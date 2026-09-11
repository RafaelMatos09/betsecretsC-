using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RodadaController : BaseController
    {
        private readonly IRodadaService _rodadaService;
        public RodadaController(IRodadaService rodadaService)
        {
            _rodadaService = rodadaService;
        }

        [HttpPost("cadastrar-rodada")]
        public async Task<IActionResult> CadastrarRodada(RodadaModel req)
        {
            try
            {
                var result = await _rodadaService.CadastrarRodada(req);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("listar-rodadas-campeonato/{campeonatoId:long}")]
        public async Task<IActionResult> ListaRodadasCampeonato(long campeonatoId)
        {
            try
            {
                var rodadas = await _rodadaService.ListaRodadasCampeonato(campeonatoId);

                return Ok(rodadas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPut("atualizar-rodada")]
        public async Task<IActionResult> AtualizaRodada(
            [FromBody] RodadaModel req)
        {
            try
            {
                await _rodadaService.AtualizaRodada(req);

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


        [HttpDelete("excluir-rodada/{id:long}")]
        public async Task<IActionResult> ExcluiRodada(long id)
        {
            try
            {
                await _rodadaService.ExcluiRodada(id);

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
