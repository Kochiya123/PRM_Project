﻿using System;
using System.Collections.Generic;

namespace PRM_Project.Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int? CartId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual Product? Product { get; set; }
}

public partial class AddCartItemDTO
{
    public int ProductId { get; set; }

    public int Quantity { get; set; }
}

public partial class UpdateCartItemDTO
{
    public long cartItemId { get; set; }

    public int Quantity { get; set; }
}

public class CartItemResponse
{
    public int CartItemId { get; set; }

    public int? CartId { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public decimal Subtotal { get; set; }

    public string ProductName { get; set; }

    public string ProductImage { get; set; }
}


