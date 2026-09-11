
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
    }
}
