using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{
    public class PartidaEventoRepository : IPartidaEventoRepository
    {
        private readonly AppDbContext _context;

        public PartidaEventoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PartidaEventoModel> CadastrarPartidaEvento(PartidaEventoModel req)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("@PartidaId", req.PartidaId);
            dbPara.Add("@JogadorId", req.JogadorId);
            dbPara.Add("@TimeId", req.TimeId);
            dbPara.Add("@Minuto", req.Minuto);
            dbPara.Add("@Tipo", req.Tipo);
            dbPara.Add("@Observacao", req.Observacao);

            var query = @"
                        INSERT INTO partida_eventos
                        (
                            partida_id,
                            jogador_id,
                            time_id,
                            minuto,
                            tipo,
                            observacao
                        )
                        VALUES
                        (
                            @PartidaId,
                            @JogadorId,
                            @TimeId,
                            @Minuto,
                            @Tipo,
                            @Observacao
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
                throw new Exception($"Erro ao cadastrar evento da partida: {ex.Message}", ex);
            }
        }

        
        public async Task<List<PartidaEventoModel>> ListaEventosPartida(long partidaId)
        {
            var query = @"
                        SELECT
                            pe.id,
                            pe.minuto,
                            pe.tipo,
                            pe.observacao,
                            j.nome AS Jogador,
                            t.nome AS Time
                        FROM partida_eventos pe
                        INNER JOIN jogadores j ON j.id = pe.jogador_id
                        INNER JOIN times t ON t.id = pe.time_id
                        WHERE pe.partida_id = @PartidaId
                        ORDER BY pe.minuto";

            var dbPara = new DynamicParameters();

            dbPara.Add("@PartidaId", partidaId);

            try
            {
                var eventos = await _context.GetAllAsync<PartidaEventoModel>(
                    query,
                    dbPara
                );

                return eventos.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao listar eventos da partida: {ex.Message}",
                    ex
                );
            }
        }


        public async Task<List<PartidaEventoModel>> ListaArtilharia(long campeonatoId)
        {
            var query = @"
                        SELECT
                            j.id AS JogadorId,
                            j.nome AS Jogador,
                            j.foto,
                            COUNT(*) AS Gols
                        FROM partida_eventos pe
                        INNER JOIN partidas p ON p.id = pe.partida_id
                        INNER JOIN jogadores j ON j.id = pe.jogador_id
                        WHERE p.campeonato_id = @CampeonatoId
                          AND pe.tipo IN ('gol', 'penalti_marcado')
                        GROUP BY
                            j.id,
                            j.nome,
                            j.foto
                        ORDER BY Gols DESC
                        LIMIT 20";

            var dbPara = new DynamicParameters();

            dbPara.Add("@CampeonatoId", campeonatoId);

            try
            {
                var artilharia = await _context.GetAllAsync<PartidaEventoModel>(
                    query,
                    dbPara
                );

                return artilharia.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao listar artilharia: {ex.Message}",
                    ex
                );
            }
        }


        public async Task AtualizaPartidaEvento(PartidaEventoModel req)
        {
            var query = @"
                        UPDATE partida_eventos
                        SET
                            minuto = @Minuto,
                            observacao = @Observacao
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", req.Id);
            dbPara.Add("@Minuto", req.Minuto);
            dbPara.Add("@Observacao", req.Observacao);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao atualizar evento da partida: {ex.Message}",
                    ex
                );
            }
        }


        public async Task ExcluiPartidaEvento(long id)
        {
            var query = @"
                        DELETE FROM partida_eventos
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
                    $"Erro ao excluir evento da partida: {ex.Message}",
                    ex
                );
            }
        }

    }
}
