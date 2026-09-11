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
    }
}
