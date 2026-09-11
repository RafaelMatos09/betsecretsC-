using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{   

    public class TimeRepository : ITimeRepository
    {
        private readonly AppDbContext _context;

        public TimeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TimesModel> CadastrarTime(TimesModel req)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("@BairroId", req.BairroId);
            dbPara.Add("@Nome", req.Nome);
            dbPara.Add("@Sigla", req.Sigla);
            dbPara.Add("@Escudo", req.Escudo);
            dbPara.Add("@CorPrincipal", req.CorPrincipal);
            dbPara.Add("@CorSecundaria", req.CorSecundaria);
            dbPara.Add("@AnoFundacao", req.AnoFundacao);
            dbPara.Add("@Tecnico", req.Tecnico);
            dbPara.Add("@Telefone", req.Telefone);
            dbPara.Add("@Instagram", req.Instagram);

            var query = @"
                        INSERT INTO times
                        (
                            bairro_id,
                            nome,
                            sigla,
                            escudo,
                            cor_principal,
                            cor_secundaria,
                            ano_fundacao,
                            tecnico,
                            telefone,
                            instagram
                        )
                        VALUES
                        (
                            @BairroId,
                            @Nome,
                            @Sigla,
                            @Escudo,
                            @CorPrincipal,
                            @CorSecundaria,
                            @AnoFundacao,
                            @Tecnico,
                            @Telefone,
                            @Instagram
                        )
                        RETURNING
                            id,
                            bairro_id AS BairroId,
                            nome,
                            sigla,
                            escudo,
                            cor_principal AS CorPrincipal,
                            cor_secundaria AS CorSecundaria,
                            ano_fundacao AS AnoFundacao,
                            tecnico,
                            telefone,
                            instagram";            

            try
            {
                var time = await _context.ExecuteAsync(
                    query,
                    dbPara
                );

                req.Id = time;
                return req;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao cadastrar time: {ex.Message}", ex);
            }
        }

        public async Task<List<TimesModel>> ListaTimes()
        {
            var query = @"
                        SELECT
                            id,
                            bairro_id AS BairroId,
                            nome,
                            sigla,
                            escudo,
                            cor_principal AS CorPrincipal,
                            cor_secundaria AS CorSecundaria,
                            ano_fundacao AS AnoFundacao,
                            tecnico,
                            telefone,
                            instagram,
                            ativo
                        FROM times";
            try
            {
                var times = await _context.GetAllAsync<TimesModel>(query);
                return times.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar times: {ex.Message}", ex);
            }
        }

        public async Task<List<TimesDetalheModel>> ConsultaTimesDetalhes(string? id = null)
        {
            var query = @"
                        SELECT
                            t.id,
                            t.nome,
                            t.sigla,
                            t.escudo,
                            t.cor_principal AS corPrincipal,
                            t.cor_secundaria AS corSecundaria,
                            t.ano_fundacao,
                            t.tecnico,
                            t.telefone,
                            t.instagram,
                            t.ativo,
                            b.nome AS BairroNome,
                            b.cidade
                        FROM times t
                        INNER JOIN bairros b ON b.id = t.bairro_id";

            var dbPara = new DynamicParameters();

            if (!string.IsNullOrEmpty(id))
            {
                query += @"
                        WHERE t.id = @Id";

                dbPara.Add("@Id", id);
            }

            try
            {
                var times = await _context.GetAllAsync<TimesDetalheModel>(
                    query,
                    dbPara
                );

                return times.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar times: {ex.Message}", ex);
            }
        }        
        public async Task AtualizaTime(TimesModel req)
        {
            var query = @"
                        UPDATE times
                        SET
                            nome = @Nome,
                            sigla = @Sigla,
                            escudo = @Escudo,
                            cor_principal = @CorPrincipal,
                            cor_secundaria = @CorSecundaria,
                            tecnico = @Tecnico,
                            telefone = @Telefone,
                            instagram = @Instagram,
                            updated_at = NOW()
                        WHERE id = @Id";

            var dbPara = new DynamicParameters();

            dbPara.Add("@Id", req.Id);
            dbPara.Add("@Nome", req.Nome);
            dbPara.Add("@Sigla", req.Sigla);
            dbPara.Add("@Escudo", req.Escudo);
            dbPara.Add("@CorPrincipal", req.CorPrincipal);
            dbPara.Add("@CorSecundaria", req.CorSecundaria);
            dbPara.Add("@Tecnico", req.Tecnico);
            dbPara.Add("@Telefone", req.Telefone);
            dbPara.Add("@Instagram", req.Instagram);

            try
            {
                await _context.ExecuteAsync(query, dbPara);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Erro ao atualizar time: {ex.Message}",
                    ex
                );
            }
        }

        public async Task DesativaTime(long id)
        {
            var query = @"
                        UPDATE times
                        SET
                            ativo = FALSE
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
                    $"Erro ao desativar time: {ex.Message}",
                    ex
                );
            }
        }

    }
}
