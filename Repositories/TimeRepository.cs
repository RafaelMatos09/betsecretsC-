using betsecrets.Interfaces.Repository;
using betsecrets.Modelos;
using betsecrets.ORM;
using Dapper;

namespace betsecrets.Repositories
{   

    public class TimeRepository : ITimeRepository
    {
        private readonly AppDbContext _context;

        public TimeRepository(AppDbContext appContext)
        {
            _context = appContext;
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
    }
}
