using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        private HttpClient _httpClient;

        public CryptoCurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<CryptoCurrencyDto>> GetAvailableCryptocurrencies()
        {
            var response = await _httpClient.GetAsync("v2/assets?fields=id,symbol,name,slug,metrics/market_data/price_usd,profile/general/overview/project_details&limit=30");
            var responseObject = await HttpResponseMessageExtensions.DeserializeJsonToList<CryptoCurrencyDto>(response, true);

            var filteredResponse = responseObject.Where(cc => cc.Symbol == "BTC" || cc.Symbol == "ETH" || cc.Symbol == "USDT" || cc.Symbol == "XMR");

            return filteredResponse;
        }
    }
}