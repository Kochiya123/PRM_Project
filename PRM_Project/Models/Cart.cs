﻿using System;
using System.Collections.Generic;

namespace PRM_Project.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }
}

public partial class AddCartDTO
{
    public int? UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;
}

public partial class UpdateCartDTO
{

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = null!;

}

public class CartResponse
{
    public long CartId { get; set; }
    public long? UserId { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
    public List<CartItemResponse> Items { get; set; }
    public int ItemCount { get; set; }
}

public class updateCartResponse 
{
    public long CartId { get; set; }
    public long? UserId { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
    public CartItemResponse Item { get; set; }
    public int ItemCount { get; set; }
}
