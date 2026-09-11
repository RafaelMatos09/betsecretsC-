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
    }
}
