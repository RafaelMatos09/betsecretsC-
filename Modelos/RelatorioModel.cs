namespace betsecrets.Modelos
{
    public class RelatorioJogadorPartidaModel
    {
        public long? PartidaId { get; set; }
        public long? CalendarioId { get; set; }
        public long? TimeId { get; set; }
        public DateTime? DataHora { get; set; }
        public DateTime? DataPrevista { get; set; }
        public TimeSpan? HorarioPrevisto { get; set; }
        public long? TimeCasaId { get; set; }
        public long? TimeVisitanteId { get; set; }
        public string? TimeCasa { get; set; }
        public string? TimeVisitante { get; set; }
        public int? GolsCasa { get; set; }
        public int? GolsVisitante { get; set; }
        public bool Titular { get; set; }
        public short? NumeroCamisa { get; set; }
        public short? MinutoEntrada { get; set; }
        public short? MinutoSaida { get; set; }
        public int Gols { get; set; }
        public int CartoesAmarelos { get; set; }
        public int CartoesVermelhos { get; set; }
        public string? StatusCalendario { get; set; }
    }

    public class RelatorioJogadorModel
    {
        public long JogadorId { get; set; }
        public string? Nome { get; set; }
        public string? Foto { get; set; }
        public string? Posicao { get; set; }
        public long? TimeId { get; set; }
        public string? TimeNome { get; set; }
        public int Jogos { get; set; }
        public int Titulares { get; set; }
        public int Gols { get; set; }
        public int CartoesAmarelos { get; set; }
        public int CartoesVermelhos { get; set; }
        public List<RelatorioJogadorPartidaModel> Partidas { get; set; } = new();
    }

    public class EstatisticaLadoModel
    {
        public long? TimeId { get; set; }
        public string? TimeNome { get; set; }
        public string? Escudo { get; set; }
        public bool Registrada { get; set; }
        public int Gols { get; set; }
        public int Chutes { get; set; }
        public int ChutesGol { get; set; }
        public decimal? Posse { get; set; }
        public int Escanteios { get; set; }
        public int Faltas { get; set; }
        public int Impedimentos { get; set; }
        public int CartoesAmarelos { get; set; }
        public int CartoesVermelhos { get; set; }
        public int Passes { get; set; }
        public int Laterais { get; set; }
    }

    public class RelatorioEstatisticaModel
    {
        public long PartidaId { get; set; }
        public long? CalendarioId { get; set; }
        public long? CampeonatoId { get; set; }
        public DateTime? DataHora { get; set; }
        public DateTime? DataPrevista { get; set; }
        public TimeSpan? HorarioPrevisto { get; set; }
        public string? Local { get; set; }
        public string? Status { get; set; }
        public EstatisticaLadoModel Casa { get; set; } = new();
        public EstatisticaLadoModel Visitante { get; set; } = new();
    }

    public class SalvarEstatisticaRequest
    {
        public long PartidaId { get; set; }
        public long TimeId { get; set; }
        public int Chutes { get; set; }
        public int ChutesGol { get; set; }
        public decimal? Posse { get; set; }
        public int Escanteios { get; set; }
        public int Faltas { get; set; }
        public int Impedimentos { get; set; }
        public int CartoesAmarelos { get; set; }
        public int CartoesVermelhos { get; set; }
        public int Passes { get; set; }
        public int Laterais { get; set; }
    }

    internal class RelatorioJogadorLinha : RelatorioJogadorPartidaModel
    {
        public long JogadorId { get; set; }
        public string? Nome { get; set; }
        public string? Foto { get; set; }
        public string? Posicao { get; set; }
        public string? TimeNome { get; set; }
    }

    internal class EstatisticaLinha
    {
        public long PartidaId { get; set; }
        public long? CalendarioId { get; set; }
        public long? CampeonatoId { get; set; }
        public DateTime? DataHora { get; set; }
        public DateTime? DataPrevista { get; set; }
        public TimeSpan? HorarioPrevisto { get; set; }
        public string? Local { get; set; }
        public string? Status { get; set; }
        public int? GolsCasa { get; set; }
        public int? GolsVisitante { get; set; }
        public long? TimeCasaId { get; set; }
        public string? TimeCasa { get; set; }
        public string? EscudoCasa { get; set; }
        public long? TimeVisitanteId { get; set; }
        public string? TimeVisitante { get; set; }
        public string? EscudoVisitante { get; set; }
        public long? EstatisticaCasaId { get; set; }
        public int? ChutesCasa { get; set; }
        public int? ChutesGolCasa { get; set; }
        public decimal? PosseCasa { get; set; }
        public int? EscanteiosCasa { get; set; }
        public int? FaltasCasa { get; set; }
        public int? ImpedimentosCasa { get; set; }
        public int? AmarelosCasa { get; set; }
        public int? VermelhosCasa { get; set; }
        public int? PassesCasa { get; set; }
        public int? LateraisCasa { get; set; }
        public long? EstatisticaVisitanteId { get; set; }
        public int? ChutesVisitante { get; set; }
        public int? ChutesGolVisitante { get; set; }
        public decimal? PosseVisitante { get; set; }
        public int? EscanteiosVisitante { get; set; }
        public int? FaltasVisitante { get; set; }
        public int? ImpedimentosVisitante { get; set; }
        public int? AmarelosVisitante { get; set; }
        public int? VermelhosVisitante { get; set; }
        public int? PassesVisitante { get; set; }
        public int? LateraisVisitante { get; set; }
    }

    internal class EventoContagem
    {
        public long PartidaId { get; set; }
        public long TimeId { get; set; }
        public string? Tipo { get; set; }
        public int Total { get; set; }
    }
}
