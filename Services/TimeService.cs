using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class TimeService : ITimeService
    {
        private readonly ITimeRepository _timeRepository;

        public TimeService(ITimeRepository timeRepository)
        {
            _timeRepository = timeRepository;
        }

        public async Task<TimesModel> CadastrarTime(TimesModel req)
        {
            return await _timeRepository.CadastrarTime(req);
        }

        public async Task<List<TimesModel>> ListaTimes()
        {
            return await _timeRepository.ListaTimes();
        }

        public async Task<List<TimesDetalheModel>> ConsultaTimesDetalhes(string? id = null)
        {
            return await _timeRepository.ConsultaTimesDetalhes(id);
        }
        public Task AtualizaTime(TimesModel req) => _timeRepository.AtualizaTime(req);
        public Task DesativaTime(long id) => _timeRepository.DesativaTime(id);
    }
}
