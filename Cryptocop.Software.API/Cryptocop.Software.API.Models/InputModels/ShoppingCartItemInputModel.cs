namespace Cryptocop.Software.API.Models.InputModels
{
    public class ShoppingCartItemInputModel
    {
        [Required]
        public string ProductIdentifier { get; set; }

        [Required]
        public float? Quantity { get; set; } = null;
    }
}