using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class PaymentCardInputModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Cardholder name")]
        public string CardholderName { get; set; }

        [Required]
        [CreditCard]
        public string CardNumber { get; set; }

        [Range(1, 12)]
        public int Month { get; set; }

        [Range(0, 99)]
        public int Year { get; set; }
    }
}