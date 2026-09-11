using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TimeController : BaseController
    {
        private readonly ITimeService _timeService;
        public TimeController(ITimeService timeService)
        {
            _timeService = timeService;
        }

        [HttpPost("cadastrar-time")]
        public async Task<IActionResult> CadastrarTime([FromBody] TimesModel req)
        {
            try
            {
                var time = await _timeService.CadastrarTime(req);
                return Ok(time);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("listar-times")]
        public async Task<IActionResult> ListaTimes()
        {
            try
            {
                var times = await _timeService.ListaTimes();
                return Ok(times);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("consulta-times-detalhes")]
        public async Task<IActionResult> ConsultaTimesDetalhes(string? id = null)
        {
            try
            {
                var times = await _timeService.ConsultaTimesDetalhes(id);
                return Ok(times);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        
        [HttpPut("atualizar-time")]
        public async Task<IActionResult> AtualizaTime([FromBody] TimesModel req)
        {
            try
            {
                await _timeService.AtualizaTime(req);

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


        [HttpDelete("desativar-time/{id:long}")]
        public async Task<IActionResult> DesativaTime(long id)
        {
            try
            {
                await _timeService.DesativaTime(id);

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
