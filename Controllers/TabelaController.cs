using betsecrets.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TabelaController : BaseController
    {
        private readonly ApiFutebolService _apiFutebolService;

        public TabelaController(ApiFutebolService apiFutebolService)
        {
            _apiFutebolService = apiFutebolService;
        }

        
        [HttpGet("brasileirao")]        
        public async Task<IActionResult> BuscarTabela()
        {
            try
            {
                var tabela = await _apiFutebolService.BuscarTabela();
                return Ok(tabela);
            }
            catch (HttpRequestException ex)
            {
                var statusCode = (int?)ex.StatusCode ?? 502;
                return StatusCode(statusCode, new { mensagem = ex.Message });
            }
        }
    }
}
