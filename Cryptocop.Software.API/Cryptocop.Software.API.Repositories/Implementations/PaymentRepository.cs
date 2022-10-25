using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public PaymentRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddPaymentCard(string email, PaymentCardInputModel paymentCard)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) { throw new System.Exception("User with the email " + email +  " not found."); }
            
            var entity = new PaymentCard
            {
                UserId = user.Id,
                CardHolderName = paymentCard.CardholderName,
                CardNumber = paymentCard.CardNumber,
                Month = paymentCard.Month,
                Year = paymentCard.Year
            };

            _dbContext.PaymentCards.Add(entity);
            _dbContext.SaveChanges();
        }

        public IEnumerable<PaymentCardDto> GetStoredPaymentCards(string email)
        {
            var paymentcards = _dbContext
                            .PaymentCards
                            .Where(a => a.User.Email == email)
                            .Select(a => new PaymentCardDto
                            {
                                Id = a.Id,
                                CardholderName = a.CardHolderName,
                                CardNumber = a.CardNumber,
                                Month = a.Month,
                                Year = a.Year
                            }).ToList();

            return paymentcards;
        }
    }
}