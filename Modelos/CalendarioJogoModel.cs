namespace betsecrets.Modelos
{
    public class CalendarioJogoModel
    {
        public long? Id { get; set; }
        public long? CampeonatoId { get; set; }
        public long? RodadaId { get; set; }
        public long? PartidaId { get; set; }
        public long? TimeCasaId { get; set; }
        public long? TimeVisitanteId { get; set; }
        public DateTime? DataPrevista { get; set; }
        public TimeSpan? HorarioPrevisto { get; set; }
        public string? LocalPrevisto { get; set; }
        public string? Status { get; set; }
        public string? Observacoes { get; set; }
        public string? TimeCasa { get; set; }
        public string? TimeVisitante { get; set; }
        public string? SiglaCasa { get; set; }
        public string? SiglaVisitante { get; set; }
        public string? EscudoCasa { get; set; }
        public string? EscudoVisitante { get; set; }
        public int? RodadaNumero { get; set; }
        public string? CampeonatoNome { get; set; }
    }
}
