using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class PaymentCardInputModel
    {
        [Required(ErrorMessage = "Cardholder name is required.")]
        [MinLength(3, ErrorMessage = "Cardholder name must be at least 3 characters long.")]
        public string CardholderName { get; set; }

        [Required(ErrorMessage = "Card number is required.")]
        [CreditCard(ErrorMessage = "Invalid credit card number.")]
        public string CardNumber { get; set; }

        [Range(1, 12, ErrorMessage = "Month must be a number between 1 and 12.")]
        public int Month { get; set; }

        [Range(0, 99, ErrorMessage = "Year must be a number between 0 and 99.")]
        public int Year { get; set; }
    }
}