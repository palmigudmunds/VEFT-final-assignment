using System.Text.Json.Serialization;

namespace Cryptocop.Software.API.Models.Dtos
{
    public class CryptoCurrencyDto
    {
        public string Id { get; set; }

        public string Symbol { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        // [JsonPropertyName("price_usd")]
        public float Price_Usd { get; set; }

        // [JsonPropertyName("project_details")]
        public string Project_Details { get; set; }
    }
}