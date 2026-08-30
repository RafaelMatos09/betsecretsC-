using betsecrets.Modelos;
using betsecrets.Modelos.Request;
using betsecrets.Modelos.Response;

namespace betsecrets.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<List<UsuarioModel>> ListarUsuarios();
        Task<LoginResponse?> Login(string email, string senha);
        Task<LoginResponse?> Cadastrar(CadastroRequest request);
    }
}
