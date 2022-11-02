using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class ShoppingCartItemInputModel
    {
        [Required]
        public string ProductIdentifier { get; set; }

        [Required]
        [Range(0.009999999, float.MaxValue, ErrorMessage = "The field Quantity must be between 0.01 and 3.4028234663852886E+38.")]
        public float? Quantity { get; set; } = null!;
    }
}