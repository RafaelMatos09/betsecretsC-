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

        public async Task<LoginResponse?> Login(LoginRequest request)
        {
            var usuario = await _usuarioRepository
                .BuscarPorEmail(request.Email);

            if (usuario == null)
                return null;

            var senhaValida = BCrypt.Net.BCrypt.Verify(
                request.Senha,
                usuario.Senha
            );

            if (!senhaValida)
                return null;

            var token = GerarToken(usuario);

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
