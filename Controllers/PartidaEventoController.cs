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
    }
}
