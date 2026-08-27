using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Microsoft.EntityFrameworkCore;

namespace betsecrets.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<UsuarioModel>> ListarUsuarios()
        {           
            try
            {
                var result = _context.Usuarios_Bet.ToListAsync();
                return result;
            }
            catch(Exception ex)
            {
                throw new Exception($"Erro ao listar usuários: {ex.Message}");
            }
        }

        public async Task<UsuarioModel?> BuscarPorEmail(string email)
        {
            return await _context.Usuarios_Bet
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
