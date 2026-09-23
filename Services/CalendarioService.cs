using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class CalendarioService : ICalendarioService
    {
        private readonly ICalendarioRepository _calendarioRepository;

        public CalendarioService(ICalendarioRepository calendarioRepository)
        {
            _calendarioRepository = calendarioRepository;
        }

        public async Task<CalendarioJogosModel> CadastrarCalendarioJogos(CalendarioJogosModel req)
        {
            return await _calendarioRepository.CadastrarCalendarioJogos(req);
        }
    }
}
