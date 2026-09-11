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
                        INSERT INTO bairros
                        (
                            nome,
                            cidade,
                            estado
                        )
                        VALUES
                        (
                            @Nome,
                            @Cidade,
                            @Estado
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
                throw new Exception($"Erro ao cadastrar bairro: {ex.Message}", ex);
            }
        }


        public async Task<List<BairroModel>> ListaBairros()
        {
            var query = @"
                            SELECT
                                id,
                                nome,
                                cidade,
                                estado,
                                created_at AS createdAt
                            FROM bairros
                            ORDER BY nome";

            try
            {
                var bairros = await _context.GetAllAsync<BairroModel>(query);

                return bairros.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar bairros: {ex.Message}", ex);
            }
        }


        public async Task<BairroModel?> ConsultaBairro(int id)
        {
            var query = @"
                        SELECT
                            id,
                            nome,
                            cidade,
                            estado,
                            created_at AS createdAt
                        FROM bairros
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", id);

            try
            {
                var bairro = await _context.GetAllAsync<BairroModel>(query, dbPara);

                return bairro.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar bairro: {ex.Message}", ex);
            }
        }


        public async Task<List<BairroModel>> ListaBairrosPorCidade(string cidade)
        {
            var query = @"
                        SELECT
                            id,
                            nome,
                            cidade,
                            estado
                        FROM bairros
                        WHERE cidade = @Cidade
                        ORDER BY nome";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Cidade", cidade);

            try
            {
                var bairros = await _context.GetAllAsync<BairroModel>(query, dbPara);

                return bairros.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar bairros por cidade: {ex.Message}", ex);
            }
        }


        public async Task AtualizaBairro(BairroModel req)
        {
            var query = @"
                        UPDATE bairros
                        SET
                            nome = @Nome,
                            cidade = @Cidade,
                            estado = @Estado
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", req.Id);
            dbPara.Add("@Nome", req.Nome);
            dbPara.Add("@Cidade", req.Cidade);
            dbPara.Add("@Estado", req.Estado);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar bairro: {ex.Message}", ex);
            }
        }


        public async Task ExcluiBairro(int id)
        {
            var query = @"
                        DELETE FROM bairros
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", id);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir bairro: {ex.Message}", ex);
            }
        }
    }
}
