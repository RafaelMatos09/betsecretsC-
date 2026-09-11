
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CampeonatoController : BaseController
    {
        private readonly ICampeonatoService _campeonatoService;
        public CampeonatoController(ICampeonatoService campeonatoService)
        {
            _campeonatoService = campeonatoService;
        }

        [HttpPost("cadastrar-campeonato")]
        public async Task<IActionResult> CadastraCampeonato([FromBody] CampeonatoModel req)
        {
            try
            {
                var campeonato = await _campeonatoService.CadastroCampeonato(req);
                return Ok(campeonato);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        
        [HttpGet("consultar-campeonato/{id:long}")]
        public async Task<IActionResult> ConsultaCampeonato(long id)
        {
            try
            {
                var campeonato = await _campeonatoService.ConsultaCampeonato(id);

                return Ok(campeonato);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpGet("listar-campeonatos")]
        public async Task<IActionResult> ListaCampeonatos()
        {
            try
            {
                var campeonatos = await _campeonatoService.ListaCampeonatos();

                return Ok(campeonatos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPut("atualizar-campeonato")]
        public async Task<IActionResult> AtualizaCampeonato(
            [FromBody] CampeonatoModel req)
        {
            try
            {
                await _campeonatoService.AtualizaCampeonato(req);

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


        [HttpDelete("excluir-campeonato/{id:long}")]
        public async Task<IActionResult> ExcluiCampeonato(long id)
        {
            try
            {
                await _campeonatoService.ExcluiCampeonato(id);

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
