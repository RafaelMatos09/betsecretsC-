using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JogadorTimeController : ControllerBase
    {
        private readonly IJogadorTimeService _jogadorTimeService;
        public JogadorTimeController(IJogadorTimeService jogadorTimeService)
        {
            _jogadorTimeService = jogadorTimeService;
        }
        [HttpPost("cadastrar-jogador-time")]
        public async Task<IActionResult> CadastraJogadorTime([FromBody] JogadorTimeModel req)
        {
            try
            {
                var jogadorTime = await _jogadorTimeService.CadastrarJogadorTime(req);
                return Ok(jogadorTime);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
