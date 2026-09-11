using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface IPartidaService
    {
        Task<PartidaModel> CadastrarPartida(PartidaModel req);
    }
}
