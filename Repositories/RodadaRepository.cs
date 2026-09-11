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
    }
}
