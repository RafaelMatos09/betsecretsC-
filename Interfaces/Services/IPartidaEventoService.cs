using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface IPartidaEventoService
    {
        Task<PartidaEventoModel> CadastrarPartidaEvento(PartidaEventoModel req);
    }
}
