using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface IPartidaRepository
    {
        Task<PartidaModel> CadastrarPartida(PartidaModel req);
    }
}
