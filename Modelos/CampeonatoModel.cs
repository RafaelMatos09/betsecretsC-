namespace betsecrets.Modelos
{
    public class CampeonatoModel
    {
        public long? Id { get; set; }
        public string? Nome { get; set; }
        public int Temporada { get; set; }
        public string? Descricao { get; set; }
        public string? Tipo { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? Status { get; set; }
        public string? Logo { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
