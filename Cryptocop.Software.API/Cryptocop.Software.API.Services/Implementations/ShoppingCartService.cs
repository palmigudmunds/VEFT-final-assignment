using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Helpers;
using Cryptocop.Software.API.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ICryptoCurrencyService _cryptoCurrencyService;
        private HttpClient _httpClient;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, ICryptoCurrencyService cryptoCurrencyService, HttpClient httpClient)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _cryptoCurrencyService = cryptoCurrencyService;
            _httpClient = httpClient;
        }
        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            return _shoppingCartRepository.GetCartItems(email);
        }

        public async Task AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItem)
        {   

            var symbol = shoppingCartItem.ProductIdentifier.ToUpper();

            bool isValidProductIdentifier = false;
            if (symbol == "BTC" || symbol == "ETH" || symbol == "USDT" || symbol == "XMR")
            {
                isValidProductIdentifier = true;
            }
            if (!isValidProductIdentifier)
            {
                throw new System.Exception("Invalid product identifier");
            }
            
            var response = await _httpClient.GetAsync("https://data.messari.io/api/v1/assets/" + symbol + "/metrics?fields=symbol,market_data/price_usd");
            var responseObject = await HttpResponseMessageExtensions.DeserializeJsonToObject<CryptoCurrencyDto>(response, true);

            _shoppingCartRepository.AddCartItem(email, shoppingCartItem, responseObject.Price_Usd);
        }

        public void RemoveCartItem(string email, int id)
        {
            _shoppingCartRepository.RemoveCartItem(email, id);
        }

        public void UpdateCartItemQuantity(string email, int id, float quantity)
        {
            _shoppingCartRepository.UpdateCartItemQuantity(email, id, quantity);
        }

        public void ClearCart(string email)
        {
            _shoppingCartRepository.ClearCart(email);
        }
    }
}
