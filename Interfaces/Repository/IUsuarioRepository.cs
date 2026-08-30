using betsecrets.Modelos;
using betsecrets.Modelos.Response;

namespace betsecrets.Interfaces.Repository
{
    public interface IUsuarioRepository
    {
        Task<List<UsuarioModel>> ListarUsuarios();
        Task<UsuarioModel?> BuscarPorEmail(string email);
        void CadastraRegistroAcesso(LoginResponse req);
    }
}
