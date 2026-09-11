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


        public async Task<List<ClassificacaoModel>> ListaClassificacao(long campeonatoId)
        {
            var query = @"
                        SELECT
                            c.posicao AS Posicao,
                            t.nome AS Time,
                            t.escudo,
                            c.jogos AS Jogos,
                            c.vitorias AS Vitorias,
                            c.empates AS Empates,
                            c.derrotas AS Derrotas,
                            c.gols_pro AS GolsPro,
                            c.gols_contra AS GolsContra,
                            c.saldo_gols AS SaldoGols,
                            c.pontos AS Pontos
                        FROM classificacao c
                        INNER JOIN times t ON t.id = c.time_id
                        WHERE c.campeonato_id = @CampeonatoId
                        ORDER BY
                            c.pontos DESC,
                            c.saldo_gols DESC,
                            c.gols_pro DESC";

            var dbPara = new DynamicParameters();

            dbPara.Add("@CampeonatoId", campeonatoId);

            try
            {
                var classificacao = await _context.GetAllAsync<ClassificacaoModel>(
                    query,
                    dbPara
                );

                return classificacao.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao listar classificação: {ex.Message}",
                    ex
                );
            }
        }


        public async Task RecalculaPosicoes(long campeonatoId)
        {
            var query = @"
                        WITH ranking AS
                        (
                            SELECT
                                id,
                                ROW_NUMBER() OVER (
                                    ORDER BY
                                        pontos DESC,
                                        saldo_gols DESC,
                                        gols_pro DESC
                                ) AS pos
                            FROM classificacao
                            WHERE campeonato_id = @CampeonatoId
                        )
                        UPDATE classificacao c
                        SET posicao = r.pos
                        FROM ranking r
                        WHERE c.id = r.id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@CampeonatoId", campeonatoId);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao recalcular posições: {ex.Message}",
                    ex
                );
            }
        }
    }
}


