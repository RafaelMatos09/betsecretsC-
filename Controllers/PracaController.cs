using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PracaController : BaseController
    {
        private readonly IPracaService _service;

        public PracaController(IPracaService service)
        {
            _service = service;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] PracaModel req)
        {
            try
            {
                var praca = await _service.Cadastrar(req);
                return Ok(praca);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("listar")]
        public async Task<IActionResult> Listar([FromQuery] long? bairroId)
        {
            try
            {
                var pracas = await _service.Listar(bairroId);
                return Ok(pracas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("consultar/{id:long}")]
        public async Task<IActionResult> Consultar(long id)
        {
            try
            {
                var praca = await _service.Consultar(id);
                if (praca is null)
                    return NotFound(new { message = "Praça não encontrada." });

                return Ok(praca);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("atualizar")]
        public async Task<IActionResult> Atualizar([FromBody] PracaModel req)
        {
            try
            {
                await _service.Atualizar(req);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpDelete("excluir/{id:long}")]
        public async Task<IActionResult> Excluir(long id)
        {
            try
            {
                await _service.Excluir(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
