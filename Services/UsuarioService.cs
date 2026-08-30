using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;
using betsecrets.Modelos.Request;
using betsecrets.Modelos.Response;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace betsecrets.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public UsuarioService(IUsuarioRepository usuarioRepository,
                                IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }


        public async Task<List<UsuarioModel>> ListarUsuarios()
        {
            var usuarios = await _usuarioRepository.ListarUsuarios();

            return usuarios;
        }

        public async Task<LoginResponse?> Login(string email, string senha)
        {
            if (email == null ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(senha))
            {
                return null;
            }

            var usuario = await _usuarioRepository.BuscarPorEmail(email);

            if (usuario == null)
                return null;

            var senhaValida = BCrypt.Net.BCrypt.Verify(
                senha,
                usuario.Senha
            );

            if (!senhaValida)
                return null;            

            var token = GerarToken(usuario);

            var registroAcessoRequest = new LoginResponse
            {
                Usuario = usuario,
                Token = string.Empty
            };

            _usuarioRepository.CadastraRegistroAcesso(registroAcessoRequest);

            return new LoginResponse
            {
                Token = token,
                Usuario = new UsuarioModel
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    UserName = usuario.UserName,
                    Email = usuario.Email,
                    FotoUrl = usuario.FotoUrl
                }
            };
        }

        public async Task<LoginResponse?> Cadastrar(CadastroRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Senha))
            {
                return null;
            }

            var emailExistente = await _usuarioRepository.BuscarPorEmail(request.Email);
            if (emailExistente != null)
                return null;

            var userName = request.Email.Split('@')[0];

            var novoUsuario = new UsuarioModel
            {
                Nome = request.Nome.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                UserName = userName,
                Senha = BCrypt.Net.BCrypt.HashPassword(request.Senha),
                FotoUrl = string.Empty
            };

            var usuarioCriado = await _usuarioRepository.CadastrarUsuario(novoUsuario);
            if (usuarioCriado == null)
                return null;

            var token = GerarToken(usuarioCriado);

            return new LoginResponse
            {
                Token = token,
                Usuario = new UsuarioModel
                {
                    Id = usuarioCriado.Id,
                    Nome = usuarioCriado.Nome,
                    UserName = usuarioCriado.UserName,
                    Email = usuarioCriado.Email,
                    FotoUrl = usuarioCriado.FotoUrl
                }
            };
        }

        private string GerarToken(UsuarioModel usuario)
        {
            var key = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT Key não configurada.");

            var issuer = _configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer não configurado.");

            var audience = _configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("JWT Audience não configurado.");

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            );

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, usuario.Nome ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
