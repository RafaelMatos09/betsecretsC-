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
        
        public async Task<CampeonatoModel?> ConsultaCampeonato(long id)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", id);

            var query = @"
                        SELECT
                            id,
                            nome,
                            temporada,
                            descricao,
                            tipo,
                            data_inicio AS DataInicio,
                            data_fim AS DataFim,
                            status,
                            logo
                        FROM campeonatos
                        WHERE id = @Id";            

            try
            {
                var campeonato = await _context.GetAllAsync<CampeonatoModel>(query, dbPara);

                return campeonato.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar campeonato: {ex.Message}", ex);
            }
        }


        public async Task<List<CampeonatoModel>> ListaCampeonatos()
        {
            var query = @"
                            SELECT
                                id,
                                nome,
                                temporada,
                                tipo,
                                status
                            FROM campeonatos
                            ORDER BY
                                temporada DESC,
                                data_inicio DESC ";

            try
            {
                var campeonatos = await _context.GetAllAsync<CampeonatoModel>(query);

                return campeonatos.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao listar campeonatos: {ex.Message}",
                    ex
                );
            }
        }


        public async Task AtualizaCampeonato(CampeonatoModel req)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", req.Id);
            dbPara.Add("@Nome", req.Nome);
            dbPara.Add("@Descricao", req.Descricao);
            dbPara.Add("@Status", req.Status);
            dbPara.Add("@DataFim", req.DataFim);

            var query = @"
                        UPDATE campeonatos
                        SET
                            nome = @Nome,
                            descricao = @Descricao,
                            status = @Status,
                            data_fim = @DataFim
                        WHERE id = @Id";           

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar campeonato: {ex.Message}", ex);
            }
        }


        public async Task ExcluiCampeonato(long id)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", id);

            var query = @"
                        DELETE FROM campeonatos
                        WHERE id = @Id";            

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir campeonato: {ex.Message}", ex);
            }
        }


    }
}
