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

        public async Task<UsuarioModel?> CadastrarUsuario(UsuarioModel usuario)
        {
            var dbParam = new DynamicParameters();
            dbParam.Add("@Nome", usuario.Nome);
            dbParam.Add("@UserName", usuario.UserName);
            dbParam.Add("@Email", usuario.Email);
            dbParam.Add("@Senha", usuario.Senha);
            dbParam.Add("@FotoUrl", usuario.FotoUrl);

            var query = @"
                        INSERT INTO usuarios_bet
                            (nome, username, email, senha, foto_url, criado_em)
                        VALUES
                            (@Nome, @UserName, @Email, @Senha, @FotoUrl, NOW())
                        RETURNING
                            id         AS Id,
                            nome       AS Nome,
                            username   AS UserName,
                            email      AS Email,
                            foto_url   AS FotoUrl";

            try
            {
                var result = await _context.GetAllAsync<UsuarioModel>(query, dbParam);
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar usuário: {ex.Message}", ex);
            }
        }

        public async Task CadastraRegistroAcesso(LoginResponse req)
        {
            if (req?.Usuario == null)
                return;

            if (!long.TryParse(req.Usuario.Id, out long usuarioId))
                return;

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
            catch(Exception ex)
            {
                throw new Exception($"Erro ao cadastrar usuário: {ex.Message}", ex);
            }
        }
    }
}
