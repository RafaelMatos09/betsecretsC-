using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class PracaRepository : IPracaRepository
    {
        private readonly AppDbContext _context;

        public PracaRepository(AppDbContext context)
        {
            _context = context;
        }

        private const string SelectPraca = @"
                        SELECT
                            p.id,
                            p.bairro_id AS BairroId,
                            p.nome,
                            p.endereco,
                            p.tipo,
                            p.latitude,
                            p.longitude,
                            p.observacoes,
                            b.nome AS BairroNome,
                            (
                                SELECT COUNT(*)::int
                                FROM calendario_jogos cj
                                WHERE cj.praca_id = p.id
                                  AND cj.status <> 'cancelado'
                            ) AS JogosMarcados
                        FROM pracas p
                        LEFT JOIN bairros b ON b.id = p.bairro_id";

        public async Task<PracaModel> Cadastrar(PracaModel req)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("@BairroId", req.BairroId);
            dbPara.Add("@Nome", req.Nome);
            dbPara.Add("@Endereco", req.Endereco);
            dbPara.Add("@Tipo", req.Tipo);
            dbPara.Add("@Latitude", req.Latitude);
            dbPara.Add("@Longitude", req.Longitude);
            dbPara.Add("@Observacoes", req.Observacoes);

            var query = @"
                        INSERT INTO pracas
                        (
                            bairro_id,
                            nome,
                            endereco,
                            tipo,
                            latitude,
                            longitude,
                            observacoes
                        )
                        VALUES
                        (
                            @BairroId,
                            @Nome,
                            @Endereco,
                            @Tipo,
                            @Latitude,
                            @Longitude,
                            @Observacoes
                        )
                        RETURNING id";

            try
            {
                var id = await _context.ExecuteScalarAsync<long>(query, dbPara);
                return await Consultar(id) ?? req;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar praça: {ex.Message}", ex);
            }
        }

        public async Task<List<PracaModel>> Listar(long? bairroId)
        {
            var query = SelectPraca + @"
                        WHERE (@BairroId::BIGINT IS NULL OR p.bairro_id = @BairroId)
                        ORDER BY p.nome";

            var dbPara = new DynamicParameters();
            dbPara.Add("@BairroId", bairroId);

            try
            {
                var pracas = await _context.GetAllAsync<PracaModel>(query, dbPara);
                return pracas.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar praças: {ex.Message}", ex);
            }
        }

        public async Task<PracaModel?> Consultar(long id)
        {
            var query = SelectPraca + " WHERE p.id = @Id";
            var dbPara = new DynamicParameters();
            dbPara.Add("@Id", id);

            try
            {
                var pracas = await _context.GetAllAsync<PracaModel>(query, dbPara);
                return pracas.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar praça: {ex.Message}", ex);
            }
        }

        public async Task Atualizar(PracaModel req)
        {
            var query = @"
                        UPDATE pracas
                        SET bairro_id = @BairroId,
                            nome = @Nome,
                            endereco = @Endereco,
                            tipo = @Tipo,
                            latitude = @Latitude,
                            longitude = @Longitude,
                            observacoes = @Observacoes,
                            updated_at = NOW()
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();
            dbPara.Add("@Id", req.Id);
            dbPara.Add("@BairroId", req.BairroId);
            dbPara.Add("@Nome", req.Nome);
            dbPara.Add("@Endereco", req.Endereco);
            dbPara.Add("@Tipo", req.Tipo);
            dbPara.Add("@Latitude", req.Latitude);
            dbPara.Add("@Longitude", req.Longitude);
            dbPara.Add("@Observacoes", req.Observacoes);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar praça: {ex.Message}", ex);
            }
        }

        public async Task Excluir(long id)
        {
            var query = "DELETE FROM pracas WHERE id = @Id";
            var dbPara = new DynamicParameters();
            dbPara.Add("@Id", id);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir praça: {ex.Message}", ex);
            }
        }
    }
}
