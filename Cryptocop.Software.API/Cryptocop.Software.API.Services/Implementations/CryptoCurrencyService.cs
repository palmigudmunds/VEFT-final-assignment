using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class CryptoCurrencyService : ICryptoCurrencyService
    {
        public Task<IEnumerable<CryptoCurrencyDto>> GetAvailableCryptocurrencies()
        {
            throw new System.NotImplementedException();
        }
    }
}