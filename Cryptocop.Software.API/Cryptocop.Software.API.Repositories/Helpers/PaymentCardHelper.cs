using System;

namespace Cryptocop.Software.API.Repositories.Helpers
{
    public class PaymentCardHelper
    {
        public static string MaskPaymentCard(string paymentCardNumber)
        {
            var lastDigits = paymentCardNumber.Substring(paymentCardNumber.Length - 4, 4);
            var requiredMask = new String('X', paymentCardNumber.Length - lastDigits.Length);
            var maskedCreditCard = string.Concat(requiredMask, lastDigits);

            return maskedCreditCard;
        }
    }
}