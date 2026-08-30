namespace betsecrets.Modelos
{
    using System.Text.Json.Serialization;

    public class TabelaCampeonatoModel
    {
        [JsonPropertyName("posicao")]
        public int Posicao { get; set; }

        [JsonPropertyName("pontos")]
        public int Pontos { get; set; }

        [JsonPropertyName("time")]
        public TimeModel Time { get; set; } = new();

        [JsonPropertyName("jogos")]
        public int Jogos { get; set; }

        [JsonPropertyName("vitorias")]
        public int Vitorias { get; set; }

        [JsonPropertyName("empates")]
        public int Empates { get; set; }

        [JsonPropertyName("derrotas")]
        public int Derrotas { get; set; }

        [JsonPropertyName("gols_pro")]
        public int GolsPro { get; set; }

        [JsonPropertyName("gols_contra")]
        public int GolsContra { get; set; }

        [JsonPropertyName("saldo_gols")]
        public int SaldoGols { get; set; }

        [JsonPropertyName("aproveitamento")]
        public int Aproveitamento { get; set; }

        [JsonPropertyName("variacao_posicao")]
        public int VariacaoPosicao { get; set; }

        [JsonPropertyName("ultimos_jogos")]
        public List<string> UltimosJogos { get; set; } = new();

        [JsonPropertyName("faixa_classificacao")]
        public string FaixaClassificacao { get; set; } = string.Empty;
    }

    public class TimeModel
    {
        [JsonPropertyName("time_id")]
        public int TimeId { get; set; }

        [JsonPropertyName("nome_popular")]
        public string NomePopular { get; set; } = string.Empty;

        [JsonPropertyName("sigla")]
        public string Sigla { get; set; } = string.Empty;

        [JsonPropertyName("escudo")]
        public string Escudo { get; set; } = string.Empty;
    }
}
