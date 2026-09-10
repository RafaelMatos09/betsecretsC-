using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BairroController : BaseController
    {
        private readonly IBairroService _bairroService;

        public BairroController(IBairroService bairroService)
        {
            _bairroService = bairroService;
        }

        
        [HttpPost("cadastrar-bairro")]
        public async Task<IActionResult> CadastraBairro([FromBody] BairroModel req)
        {
            try
            {
                var bairro = await _bairroService.CadastraBairro(req);
                return Ok(bairro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
