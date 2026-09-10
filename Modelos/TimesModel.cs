namespace betsecrets.Modelos
{
    public class TimesModel
    {
        public long? Id { get; set; }
        public long? BairroId { get; set; }
        public string? Nome { get; set; }
        public string? Sigla { get; set; }
        public string? Escudo { get; set; }
        public string? CorPrincipal { get; set; }
        public string? CorSecundaria { get; set; }
        public int AnoFundacao { get; set; }
        public string? Tecnico { get; set; }
        public string? Telefone { get; set; }
        public string? Instagram { get; set; }
        public Boolean? Ativo { get; set; }

    }

    public class TimesDetalheModel
    {
        public long? Id { get; set; }        
        public string? Nome { get; set; }
        public string? Sigla { get; set; }
        public string? Escudo { get; set; }
        public string? CorPrincipal { get; set; }
        public string? CorSecundaria { get; set; }
        public int AnoFundacao { get; set; }
        public string? Tecnico { get; set; }
        public string? Telefone { get; set; }
        public string? Instagram { get; set; }
        public Boolean? Ativo { get; set; }
        public string? BairroNome { get; set; }
        public string? Cidade { get; set; }

    }
}
