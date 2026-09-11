namespace betsecrets.Modelos
{
    public class PartidaModel
    {
        public long? Id { get; set; }
        public long? CampeonatoId { get; set; }
        public long? RodadaId { get; set; }
        public long? TimeCasaId { get; set; }
        public long? TimeVisitanteId { get; set; }
        public int? GolsCasa { get; set; }
        public int? GolsVisitante { get; set; }
        public string? Local { get; set; }
        public DateTime? DataHora { get; set; }
        public string? Status { get; set; }
        public string? Arbitro { get; set; }
        public string? TimeCasa { get; set; }
        public string? EscudoCasa { get; set; }
        public string? TimeVisitante { get; set; }
        public string? EscudoVisitante { get; set; }
        public int? RodadaNumero { get; set; }

    }
}
