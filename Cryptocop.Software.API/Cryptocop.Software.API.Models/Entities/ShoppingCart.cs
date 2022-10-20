using System.Collections.Generic;

namespace Cryptocop.Software.API.Models.Entities;

public class ShoppingCart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }

    public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
}