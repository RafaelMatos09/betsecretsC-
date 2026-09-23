using betsecrets.Modelos;

namespace betsecrets.Interfaces.Services
{
    public interface ICalendarioService
    {
        Task<CalendarioJogosModel> CadastrarCalendarioJogos(CalendarioJogosModel req);
    }
}
