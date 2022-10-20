using System.Collections.Generic;

namespace Cryptocop.Software.API.Models.Entities;

public class User
{
    public int Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string HashedPassword { get; set; }

    public ShoppingCart? ShoppingCart { get; set; } = null;

    public ICollection<PaymentCard>? PaymentCards { get; set; } = null;

    public ICollection<Address>? Addresses { get; set; } = null;

    public ICollection<Order>? Orders { get; set; } = null;
}