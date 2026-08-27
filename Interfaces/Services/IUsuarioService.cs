using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioModel>> ListarUsuarios();
    }
}
