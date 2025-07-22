using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PRM_Project.Models;

public partial class User : IdentityUser<int>
{
    public string? Email { get; set; }

    public string? Address { get; set; }
    [Required]
    public string Role { get; set; } = null!;

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

public partial class UserDTO
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Role { get; set; } = "Customer"!;
}

public partial class LoginDTO
{
    public string Username { get; set; }

    public string Password { get; set; }
}
