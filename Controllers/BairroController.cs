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

        [HttpGet("listar-bairros")]
        public async Task<IActionResult> ListaBairros()
        {
            try
            {
                var bairros = await _bairroService.ListaBairros();
                return Ok(bairros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }       

        [HttpGet("consultar-bairro/{id}")]
        public async Task<IActionResult> ConsultaBairro(int id)
        {
            try
            {
                var bairro = await _bairroService.ConsultaBairro(id);

                return Ok(bairro);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpGet("listar-bairros-cidade/{cidade}")]
        public async Task<IActionResult> ListaBairrosPorCidade(string cidade)
        {
            try
            {
                var bairros = await _bairroService.ListaBairrosPorCidade(cidade);

                return Ok(bairros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpPut("atualizar-bairro")]
        public async Task<IActionResult> AtualizaBairro(
            [FromBody] BairroModel req)
        {
            try
            {
                await _bairroService.AtualizaBairro(req);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }


        [HttpDelete("excluir-bairro/{id}")]
        public async Task<IActionResult> ExcluiBairro(int id)
        {
            try
            {
                await _bairroService.ExcluiBairro(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message
                });
            }
        }
    }
}
