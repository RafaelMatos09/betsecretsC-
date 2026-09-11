using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class RodadaRepository : IRodadaRepository
    {
        private readonly AppDbContext _context;

        public RodadaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RodadaModel> CadastrarRodada(RodadaModel req)
        {
            var query = @"
                        INSERT INTO rodadas
                        (
                            campeonato_id,
                            numero,
                            fase,
                            data
                        )
                        VALUES
                        (
                            @CampeonatoId,
                            @Numero,
                            @Fase,
                            @Data
                        )
                        RETURNING id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@CampeonatoId", req.CampeonatoId);
            dbPara.Add("@Numero", req.Numero);
            dbPara.Add("@Fase", req.Fase);
            dbPara.Add("@Data", req.Data);

            try
            {
                var id = await _context.ExecuteAsync(query, dbPara);
                req.Id = id;

                return req;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar rodada: {ex.Message}", ex);
            }
        }
        
        public async Task<List<RodadaModel>> ListaRodadasCampeonato(long campeonatoId)
        {
            var query = @"
                        SELECT
                            id,
                            numero,
                            fase,
                            data
                        FROM rodadas
                        WHERE campeonato_id = @CampeonatoId
                        ORDER BY numero";

            var dbPara = new DynamicParameters();

            dbPara.Add("@CampeonatoId", campeonatoId);

            try
            {
                var rodadas = await _context.GetAllAsync<RodadaModel>(
                    query,
                    dbPara
                );

                return rodadas.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao listar rodadas: {ex.Message}",
                    ex
                );
            }
        }


        public async Task AtualizaRodada(RodadaModel req)
        {
            var query = @"
                        UPDATE rodadas
                        SET
                            data = @Data,
                            fase = @Fase
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", req.Id);
            dbPara.Add("@Data", req.Data);
            dbPara.Add("@Fase", req.Fase);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao atualizar rodada: {ex.Message}",
                    ex
                );
            }
        }


        public async Task ExcluiRodada(long id)
        {
            var query = @"
                        DELETE FROM rodadas
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", id);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao excluir rodada: {ex.Message}",
                    ex
                );
            }
        }

    }
}
