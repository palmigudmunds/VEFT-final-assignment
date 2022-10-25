using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public ShoppingCartRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) { throw new System.Exception("User with the email " + email +  " not found."); }

            var userShoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(sc => sc.UserId == user.Id);

            var cartItems = _dbContext
                            .ShoppingCartItems
                            .Where(sci => sci.ShoppingCartId == userShoppingCart.Id)
                            .Select(sci => new ShoppingCartItemDto
                            {
                                Id = sci.Id,
                                ProductIdentifier = sci.ProductIdentifier,
                                Quantity = sci.Quantity,
                                UnitPrice = sci.UnitPrice,
                                TotalPrice = sci.UnitPrice * sci.Quantity
                            }).ToList();

            return cartItems;
        }

        public void AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItem, float priceInUsd)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) { throw new System.Exception("User with the email " + email +  " not found."); }
            
            var userShoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(sc => sc.UserId == user.Id);
            if (userShoppingCart == null) {
                user.ShoppingCart = new ShoppingCart();
                _dbContext.ShoppingCarts.Add(user.ShoppingCart);
                _dbContext.SaveChanges();
            }

            var entity = new ShoppingCartItem
            {
                ShoppingCartId = user.ShoppingCart.Id,
                ProductIdentifier = shoppingCartItem.ProductIdentifier,
                Quantity = shoppingCartItem.Quantity.Value,
                UnitPrice = priceInUsd
            };

            _dbContext.ShoppingCartItems.Add(entity);
            _dbContext.SaveChanges();
        }

        public void RemoveCartItem(string email, int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) { throw new System.Exception("User with the email " + email +  " not found."); }

            var userShoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(sc => sc.UserId == user.Id);

            var entity = _dbContext.ShoppingCartItems.FirstOrDefault(sci => sci.Id == id && sci.ShoppingCartId == userShoppingCart.Id);
            if (entity == null) { throw new System.Exception("Shopping Cart Item with id " + id + " not found for user with email " + email + "."); }

            _dbContext.ShoppingCartItems.Remove(entity);
            _dbContext.SaveChanges();
        }

        public void UpdateCartItemQuantity(string email, int id, float quantity)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) { throw new System.Exception("User with the email " + email +  " not found."); }

            var userShoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(sc => sc.UserId == user.Id);

            var entity = _dbContext.ShoppingCartItems.FirstOrDefault(sci => sci.Id == id && sci.ShoppingCartId == userShoppingCart.Id);
            if (entity == null) { throw new System.Exception("Shopping Cart Item with id " + id + " not found for user with email " + email + "."); }

            entity.Quantity = quantity;
            _dbContext.SaveChanges();
        }

        public void ClearCart(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) { throw new System.Exception("User with the email " + email +  " not found."); }

            var userShoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(sc => sc.UserId == user.Id);

            _dbContext.RemoveRange(_dbContext.ShoppingCartItems.Where(sci => sci.ShoppingCartId == userShoppingCart.Id));
            _dbContext.SaveChanges();
        }

        public void DeleteCart(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) { throw new System.Exception("User with the email " + email +  " not found."); }

            var userShoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(sc => sc.UserId == user.Id);

            _dbContext.Remove(userShoppingCart);
            _dbContext.SaveChanges();
        }
    }
}