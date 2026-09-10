using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using betsecrets.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JogadorController : BaseController
    {
        private readonly IJogadorService _jogadorService;

        public JogadorController(IJogadorService jogadorService)
        {
            _jogadorService = jogadorService;
        }

        [HttpGet("cadastra-jogador")]        
        public async Task<IActionResult> CadastraJogador([FromBody] JogadorModel req)
        {
            try
            {
                var bairro = await _jogadorService.CadastraJogador(req);
                return Ok(bairro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}
