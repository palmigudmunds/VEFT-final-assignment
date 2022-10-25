using System.Text.Json.Serialization;

namespace Cryptocop.Software.API.Models.Dtos
{
    public class CryptoCurrencyDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        // [JsonPropertyName("price_usd")]
        // public float? PriceInUsd { get; set; }

        // [JsonPropertyName("overview")]
        // public string ProjectDetails { get; set; }
    }
}