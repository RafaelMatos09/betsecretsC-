using betsecrets.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PartidasController : BaseController
    {
        private readonly ApiFutebolService _apiFutebolService;

        public PartidasController(ApiFutebolService apiFutebolService)
        {
            _apiFutebolService = apiFutebolService;
        }
        
        [HttpGet("ao-vivo")]
        public async Task<IActionResult> BuscarAoVivo()
        {
            try
            {
                var partidas = await _apiFutebolService.BuscarPartidasAoVivo();
                return Ok(partidas);
            }
            catch (HttpRequestException ex)
            {
                var statusCode = (int?)ex.StatusCode ?? 502;
                return StatusCode(statusCode, new { mensagem = ex.Message });
            }
        }
    }
}
