using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public OrderRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public IEnumerable<OrderDto> GetOrders(string email)
        {
            var orders = _dbContext.Orders.Where(o => o.Email == email);
            if (orders == null) { throw new System.Exception("Order with the email " + email +  " not found."); }
            
            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                Email = o.Email,
                FullName = o.FullName,
                StreetName = o.StreetName,
                HouseNumber = o.HouseNumber,
                ZipCode = o.ZipCode,
                Country = o.Country,
                City = o.City,
                CardholderName = o.CardholderName,
                CreditCard = o.MaskedCreditCard,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    ProductIdentifier = oi.ProductIdentifier,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            }).ToList();
        }

        public OrderDto CreateNewOrder(string email, OrderInputModel order)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) { throw new System.Exception("User with the email " + email +  " not found."); }

            var address = _dbContext.Addresses.FirstOrDefault(a => a.Id == order.AddressId);
            if (address == null) { throw new System.Exception("Address with the id " + order.AddressId +  " not found."); }

            var paymentCard = _dbContext.PaymentCards.FirstOrDefault(pc => pc.Id == order.PaymentCardId);
            if (paymentCard == null) { throw new System.Exception("Payment card with the id " + order.PaymentCardId +  " not found."); }

            var userShoppingCart = _dbContext.ShoppingCarts.FirstOrDefault(sc => sc.UserId == user.Id);
            if (userShoppingCart == null) { throw new System.Exception("Shopping cart for user with the email " + email +  " not found."); }

            var userShoppingCartItems = _dbContext.ShoppingCartItems.Where(sci => sci.ShoppingCartId == userShoppingCart.Id);

            var cartTotalPrice = userShoppingCartItems.Sum(sci => sci.UnitPrice * sci.Quantity);

            // Masking credit card number
            var cardNumber = paymentCard.CardNumber;
            var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);
            var requiredMask = new String('X', cardNumber.Length - lastDigits.Length);
            var maskedCreditCard = string.Concat(requiredMask, lastDigits);

            var orderEntity = new Order
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City,
                CardholderName = paymentCard.CardHolderName,
                MaskedCreditCard = maskedCreditCard,
                OrderDate = DateTime.Now.ToString("dd.MM.yyyy"),
                TotalPrice = cartTotalPrice
            };

            _dbContext.Orders.Add(orderEntity);
            _dbContext.SaveChanges();

            userShoppingCartItems.ToList().ForEach(sci => {
                var entity = new OrderItem
                {
                    OrderId = orderEntity.Id,
                    ProductIdentifier = sci.ProductIdentifier,
                    Quantity = sci.Quantity,
                    UnitPrice = sci.UnitPrice,
                    TotalPrice = sci.UnitPrice * sci.Quantity
                };
                _dbContext.OrderItems.Add(entity);
            });
        
            _dbContext.SaveChanges();

            return new OrderDto
            {
                Id = orderEntity.Id,
                Email = orderEntity.Email,
                FullName = orderEntity.FullName,
                StreetName = orderEntity.StreetName,
                HouseNumber = orderEntity.HouseNumber,
                ZipCode = orderEntity.ZipCode,
                Country = orderEntity.Country,
                City = orderEntity.City,
                CardholderName = orderEntity.CardholderName,
                CreditCard = cardNumber,
                OrderDate = orderEntity.OrderDate,
                TotalPrice = orderEntity.TotalPrice
            };
        }
    }
}