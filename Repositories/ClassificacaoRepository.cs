using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class ClassificacaoRepository : IClassificacaoRepository
    {
        private readonly AppDbContext _context;

        public ClassificacaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ClassificacaoModel> CadastrarClassificacao(ClassificacaoModel req)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("@CampeonatoId", req.CampeonatoId);
            dbPara.Add("@TimeId", req.TimeId);
            dbPara.Add("@Jogos", req.Jogos);
            dbPara.Add("@Vitorias", req.Vitorias);
            dbPara.Add("@Empates", req.Empates);
            dbPara.Add("@Derrotas", req.Derrotas);
            dbPara.Add("@GolsPro", req.GolsPro);
            dbPara.Add("@GolsContra", req.GolsContra);
            dbPara.Add("@SaldoGols", req.SaldoGols);
            dbPara.Add("@Pontos", req.Pontos);

            var query = @"
                        INSERT INTO classificacao
                        (
                            campeonato_id,
                            time_id,
                            jogos,
                            vitorias,
                            empates,
                            derrotas,
                            gols_pro,
                            gols_contra,
                            saldo_gols,
                            pontos
                        )
                        VALUES
                        (
                            @CampeonatoId,
                            @TimeId,
                            @Jogos,
                            @Vitorias,
                            @Empates,
                            @Derrotas,
                            @GolsPro,
                            @GolsContra,
                            @SaldoGols,
                            @Pontos
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
                throw new Exception($"Erro ao cadastrar classificação: {ex.Message}", ex);
            }
        }
    }
}
