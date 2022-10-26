using System;
using System.Text.Json.Serialization;

namespace Cryptocop.Software.API.Models.Dtos
{
    public class ExchangeDto
    {
        public string Id { get; set; }

        public string Exchange_Name { get; set; }

        public string Exchange_Slug { get; set; }

        public string Base_Asset_Symbol { get; set; }

        public float? Price_Usd { get; set; } = null!;

        public DateTime? Last_Trade_At { get; set; } = null!;
    }
}