using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class JogadorTimeRepository : IJogadorTimeRepository
    {
        private readonly AppDbContext _context;

        public JogadorTimeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JogadorTimeModel> CadastrarJogadorTime(JogadorTimeModel req)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("@JogadorId", req.JogadorId);
            dbPara.Add("@TimeId", req.TimeId);
            dbPara.Add("@NumeroCamisa", req.NumeroCamisa);                       
            
            var query = @"
                        INSERT INTO jogadores_times
                        (
                            jogador_id,
                            time_id,
                            numero_camisa,
                            data_inicio                        
                        )
                        VALUES
                        (
                            @JogadorId,
                            @TimeId,
                            @NumeroCamisa,
                            NOW()                         
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
                throw new Exception($"Erro ao cadastrar jogador no time: {ex.Message}");
            }
        }
        
        public async Task<List<JogadorTimeModel>> ListaElenco(long timeId)
        {
            var query = @"
                        SELECT
                            jt.id,
                            j.id AS JogadorId,
                            j.nome,
                            j.apelido,
                            j.foto,
                            j.posicao,
                            jt.numero_camisa AS NumeroCamisa,
                            jt.data_inicio AS DataInicio
                        FROM jogadores_times jt
                        INNER JOIN jogadores j ON j.id = jt.jogador_id
                        WHERE jt.time_id = @TimeId
                          AND jt.ativo = TRUE
                        ORDER BY jt.numero_camisa";

            var dbPara = new DynamicParameters();

            dbPara.Add("@TimeId", timeId);

            try
            {
                var elenco = await _context.GetAllAsync<JogadorTimeModel>(
                    query,
                    dbPara
                );

                return elenco.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao listar elenco: {ex.Message}",
                    ex
                );
            }
        }


        public async Task<List<JogadorTimeModel>> ListaHistoricoJogador(long jogadorId)
        {
            var query = @"
                        SELECT
                            t.nome AS TimeNome,
                            jt.numero_camisa AS NumeroCamisa,
                            jt.data_inicio AS DataInicio,
                            jt.data_fim AS DataFim,
                            jt.ativo
                        FROM jogadores_times jt
                        INNER JOIN times t ON t.id = jt.time_id
                        WHERE jt.jogador_id = @JogadorId
                        ORDER BY jt.data_inicio DESC";

            var dbPara = new DynamicParameters();

            dbPara.Add("@JogadorId", jogadorId);

            try
            {
                var historico = await _context.GetAllAsync<JogadorTimeModel>(
                    query,
                    dbPara
                );

                return historico.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao listar histórico do jogador: {ex.Message}",
                    ex
                );
            }
        }


        public async Task EncerraVinculo(long id, DateTime dataFim)
        {
            var query = @"
                        UPDATE jogadores_times
                        SET
                            ativo = FALSE,
                            data_fim = @DataFim
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", id);
            dbPara.Add("@DataFim", dataFim);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao encerrar vínculo: {ex.Message}",
                    ex
                );
            }
        }

    }
}
