using betsecrets.Interfaces.Repository;
using betsecrets.Interfaces.Services;
using betsecrets.Modelos;

namespace betsecrets.Services
{
    public class PracaService : IPracaService
    {
        private static readonly string[] TiposPermitidos = ["campo", "quadra", "praca"];

        private readonly IPracaRepository _repository;

        public PracaService(IPracaRepository repository)
        {
            _repository = repository;
        }

        public Task<PracaModel> Cadastrar(PracaModel req)
        {
            Validar(req);
            return _repository.Cadastrar(req);
        }

        public Task<List<PracaModel>> Listar(long? bairroId) => _repository.Listar(bairroId);

        public Task<PracaModel?> Consultar(long id) => _repository.Consultar(id);

        public Task Atualizar(PracaModel req)
        {
            if (req.Id is not > 0)
                throw new Exception("Informe a praça que será atualizada.");

            Validar(req);
            return _repository.Atualizar(req);
        }

        public Task Excluir(long id)
        {
            if (id <= 0)
                throw new Exception("Informe a praça que será excluída.");

            return _repository.Excluir(id);
        }

        private static void Validar(PracaModel req)
        {
            req.Nome = req.Nome?.Trim();
            if (string.IsNullOrWhiteSpace(req.Nome))
                throw new Exception("Informe o nome da praça ou do campo.");
            if (req.Nome.Length > 150)
                throw new Exception("O nome da praça pode ter no máximo 150 caracteres.");

            req.Tipo = string.IsNullOrWhiteSpace(req.Tipo)
                ? "campo"
                : req.Tipo.Trim().ToLowerInvariant();
            if (!TiposPermitidos.Contains(req.Tipo))
                throw new Exception("Tipo inválido. Use campo, quadra ou praça.");

            if (req.Latitude is null || req.Longitude is null)
                throw new Exception("Marque o ponto no mapa para salvar a localização.");
            if (req.Latitude is < -90 or > 90 || req.Longitude is < -180 or > 180)
                throw new Exception("Coordenadas da praça inválidas.");

            req.Endereco = string.IsNullOrWhiteSpace(req.Endereco) ? null : req.Endereco.Trim();
            req.Observacoes = string.IsNullOrWhiteSpace(req.Observacoes) ? null : req.Observacoes.Trim();
            if (req.BairroId is <= 0)
                req.BairroId = null;
        }
    }
}
