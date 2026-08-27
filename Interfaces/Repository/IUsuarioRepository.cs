using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IUsuarioRepository
    {
        Task<List<UsuarioModel>> ListarUsuarios();
        Task<UsuarioModel?> BuscarPorEmail(string email);
    }
}
