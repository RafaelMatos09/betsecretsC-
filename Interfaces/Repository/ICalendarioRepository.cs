using betsecrets.Modelos;

namespace betsecrets.Interfaces.Repository
{
    public interface ICalendarioRepository
    {
        Task<CalendarioJogosModel> CadastrarCalendarioJogos(CalendarioJogosModel req);
    }
}
