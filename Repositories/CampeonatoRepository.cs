using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class CampeonatoRepository : ICampeonatoRepository
    {
        private readonly AppDbContext _context;

        public CampeonatoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CampeonatoModel> CadastroCampeonato(CampeonatoModel req)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("@Nome", req.Nome);
            dbPara.Add("@Temporada", req.Temporada);
            dbPara.Add("@Descricao", req.Descricao);
            dbPara.Add("@Tipo", req.Tipo);
            dbPara.Add("@DataInicio", req.DataInicio);
            dbPara.Add("@DataFim", req.DataFim);
            dbPara.Add("@Status", req.Status);
            dbPara.Add("@Logo", req.Logo);

            var query = @"
                        INSERT INTO campeonatos
                        (
                            nome,
                            temporada,
                            descricao,
                            tipo,
                            data_inicio,
                            data_fim,
                            status,
                            logo
                        )
                        VALUES
                        (
                            @Nome,
                            @Temporada,
                            @Descricao,
                            @Tipo,
                            @DataInicio,
                            @DataFim,
                            @Status,
                            @Logo
                        )
                        RETURNING id";            

            try
            {
                var id = await _context.ExecuteAsync(query, dbPara);

                req.Id = id;

                return req;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar campeonato: {ex.Message}", ex);
            }
        }
    }
}
