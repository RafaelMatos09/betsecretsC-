namespace betsecrets.Modelos
{
    using System.Text.Json.Serialization;

    public class PartidaAoVivoModel
    {
        [JsonPropertyName("partida_id")]
        public int PartidaId { get; set; }

        [JsonPropertyName("campeonato")]
        public CampeonatoResumoModel Campeonato { get; set; } = new();

        [JsonPropertyName("placar")]
        public string Placar { get; set; } = string.Empty;

        [JsonPropertyName("time_mandante")]
        public TimeModel TimeMandante { get; set; } = new();

        [JsonPropertyName("time_visitante")]
        public TimeModel TimeVisitante { get; set; } = new();

        [JsonPropertyName("placar_mandante")]
        public int? PlacarMandante { get; set; }

        [JsonPropertyName("placar_visitante")]
        public int? PlacarVisitante { get; set; }

        [JsonPropertyName("disputa_penalti")]
        public bool DisputaPenalti { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;

        [JsonPropertyName("data_realizacao")]
        public string DataRealizacao { get; set; } = string.Empty;

        [JsonPropertyName("hora_realizacao")]
        public string HoraRealizacao { get; set; } = string.Empty;

        [JsonPropertyName("data_realizacao_iso")]
        public string DataRealizacaoIso { get; set; } = string.Empty;

        [JsonPropertyName("estadio")]
        public EstadioModel? Estadio { get; set; }

        [JsonPropertyName("_link")]
        public string Link { get; set; } = string.Empty;
    }

    public class CampeonatoResumoModel
    {
        [JsonPropertyName("campeonato_id")]
        public int CampeonatoId { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;
    }

    public class EstadioModel
    {
        [JsonPropertyName("estadio_id")]
        public int EstadioId { get; set; }

        [JsonPropertyName("nome_popular")]
        public string NomePopular { get; set; } = string.Empty;
    }
}
