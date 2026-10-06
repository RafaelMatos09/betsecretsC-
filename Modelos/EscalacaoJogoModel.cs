namespace betsecrets.Modelos
{
    public class EscalacaoJogoModel
    {
        public long? Id { get; set; }
        public long? PartidaId { get; set; }
        public long? JogadorId { get; set; }
        public long? TimeId { get; set; }
        public bool Titular { get; set; }
        public short? NumeroCamisa { get; set; }
        public string? Posicao { get; set; }
        public short? MinutoEntrada { get; set; }
        public short? MinutoSaida { get; set; }
        public string? Nome { get; set; }
        public string? Foto { get; set; }
        public int? JogosTitular { get; set; }
    }

    public class SalvarEscalacaoTimeRequest
    {
        public long PartidaId { get; set; }
        public long TimeId { get; set; }
        public List<EscalacaoJogoModel> Jogadores { get; set; } = new();
    }

    public class ConfirmarCalendarioRequest
    {
        public DateTime DataPrevista { get; set; }
        public TimeSpan? HorarioPrevisto { get; set; }
        public string? LocalPrevisto { get; set; }
    }

    public class AtualizarStatusCalendarioRequest
    {
        public string Status { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
    }

    public class VincularPartidaRequest
    {
        public long PartidaId { get; set; }
    }
}
