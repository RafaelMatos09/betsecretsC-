using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarioController : BaseController
    {
        private readonly ICalendarioService _calendarioService;
        public CalendarioController(ICalendarioService calendarioService)
        {
            _calendarioService = calendarioService;
        }

        [HttpPost("cadastrar-calendario-jogos")]
        public async Task<IActionResult> CadastraCalendarioJogos([FromBody] CalendarioJogosModel req)
        {
            try
            {
                var calendario = await _calendarioService.CadastrarCalendarioJogos(req);
                return Ok(calendario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
