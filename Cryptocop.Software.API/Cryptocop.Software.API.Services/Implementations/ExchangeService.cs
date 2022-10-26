using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ExchangeService : IExchangeService
    {
        private HttpClient _httpClient;

        public ExchangeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Envelope<ExchangeDto>> GetExchanges(int pageNumber = 1)
        {
            var response = await _httpClient.GetAsync("https://data.messari.io/api/v1/markets?fields=id,exchange_name,exchange_slug,base_asset_symbol,price_usd,last_trade_at&page=" + pageNumber);
            var responseObject = await HttpResponseMessageExtensions.DeserializeJsonToList<ExchangeDto>(response, true);

            return new Envelope<ExchangeDto>
            {
                PageNumber = pageNumber,
                Items = responseObject
            };
        }
    }
}