using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<CryptoCurrencyDto>>("v1/assets?fields=id,symbol,name,slug,&limit=30");
            // var response = await _httpClient.GetAsync("v1/assets?fields=id,symbol,name,slug,metrics/market_data/price_usd,profile/overview&limit=30");
            // var responseObject = await HttpResponseMessageExtensions.DeserializeJsonToList<CryptoCurrencyDto>(response, true);

            return response;
        }
    }
}