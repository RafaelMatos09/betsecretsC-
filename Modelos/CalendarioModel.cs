namespace betsecrets.Modelos
{
    public class CalendarioJogosModel
    {
        public long Id { get; set; }
        public long CampeonatoId { get; set; }
        public long RodadaId { get; set; }
        public long PartidaId { get; set; }
        public long TimeCasaId { get; set; }
        public long TimeVisitanteId { get; set; }
        public DateOnly DataPrevista { get; set; }
        public TimeOnly HorarioPrevisto { get; set; }
        public string? LocalPrevisto { get; set; }
        public string? Status { get; set; }
        public string? Observacoes { get; set; }
    }
}
