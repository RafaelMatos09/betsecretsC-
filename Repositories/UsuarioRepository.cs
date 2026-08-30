using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.Modelos.Response;
using betsecrets.ORM;
using Dapper;
using System.Data;

namespace betsecrets.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioModel>> ListarUsuarios()
        {
            var query = $@"
                SELECT *
                FROM Usuarios_bet";
            try
            {
                var result = await _context.GetAllAsync<UsuarioModel>(query);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar usuários: {ex.Message}");
            }
        }

        public async Task<UsuarioModel?> BuscarPorEmail(string email)
        {
            var dbParam = new DynamicParameters();
            dbParam.Add("@Email", email);

            var query = @"
                        SELECT
                            id         AS Id,
                            nome       AS Nome,
                            username   AS Username,
                            email      AS Email,
                            senha      AS Senha,
                            foto_url   AS FotoUrl,
                            criado_em  AS CriadoEm
                        FROM usuarios_bet
                        WHERE email = @Email";

            try
            {
                var result = await _context.GetAllAsync<UsuarioModel>(query, dbParam);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar usuário por email: {ex.Message}", ex);
            }
        }

        public async void CadastraRegistroAcesso(LoginResponse req)
        {
            if (req?.Usuario == null)
            {
                throw new ArgumentNullException(nameof(req.Usuario), "Usuário não pode ser nulo.");
            }

            // Converte string para bigint (long)
            if (!long.TryParse(req.Usuario.Id, out long usuarioId))
            {
                throw new ArgumentException("Id do usuário é inválido.");
            }

            var dbParam = new DynamicParameters();
            dbParam.Add("@Id", usuarioId);

            var query = @"
                        INSERT INTO registro_acessos_bet
                            (usuario_id, data_acesso, ip, user_agent, sucesso, criado_em)
                        VALUES
                            (@Id, NOW(), '192.168.0.1', 'Windows', TRUE, NOW())";

            try
            {
                await _context.ExecuteAsync(query, dbParam);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar registro de acesso: {ex.Message}", ex);
            }
        }
    }
}
