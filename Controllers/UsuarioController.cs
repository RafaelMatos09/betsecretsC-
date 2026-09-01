using betsecrets.Interfaces.Services;
using betsecrets.Modelos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace betsecrets.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioService usuarioService, ILogger<UsuarioController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpGet("listar")]
        public async Task<IActionResult> ListarUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.ListarUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar usuários");
                return StatusCode(503, new { mensagem = "Serviço de banco de dados indisponível. Verifique ConnectionStrings__Postgres no Render." });
            }
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string senha)
        {
            try
            {
                var result = await _usuarioService.Login(email, senha);

                if (result is null)
                    return Unauthorized(new { mensagem = "Email ou senha inválidos." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no login para {Email}", email);
                return StatusCode(503, new { mensagem = "Serviço de banco de dados indisponível. Verifique ConnectionStrings__Postgres no Render." });
            }
        }

        [AllowAnonymous]
        [HttpPost("cadastro")]
        public async Task<IActionResult> Cadastro([FromBody] CadastroRequest request)
        {
            try
            {
                var result = await _usuarioService.Cadastrar(request);

                if (result is null)
                    return BadRequest(new { mensagem = "Não foi possível criar a conta. Verifique os dados informados." });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no cadastro para {Email}", request.Email);
                return StatusCode(503, new { mensagem = "Serviço de banco de dados indisponível. Verifique ConnectionStrings__Postgres no Render." });
            }
        }
    }
}
