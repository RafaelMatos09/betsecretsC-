using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class BairroRepository : IBairroRepository
    {

        private readonly AppDbContext _context;

        public BairroRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BairroModel> CadastraBairro(BairroModel req)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("@Nome", req.Nome);
            dbPara.Add("@Cidade", req.Cidade);
            dbPara.Add("@Estado", req.Estado);

            var query = @"
                        INSERT INTO bairros (nome, cidade, estado)
                        VALUES (@Nome, @Cidade, @Estado)
                        RETURNING id";
            try
            {
                var id = await _context.ExecuteAsync(query, dbPara);
                req.Id = id.ToString();
                return req;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar bairro: {ex.Message}");
            }
        }
    }
}
