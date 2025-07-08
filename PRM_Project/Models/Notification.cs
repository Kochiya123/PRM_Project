using System;
using System.Collections.Generic;

namespace PRM_Project.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int? UserId { get; set; }

    public string? Message { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? User { get; set; }
}

public partial class AddNotificationDTO
{
    public int? UserId { get; set; }

    public string? Message { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }
}

public partial class UpdateNotificationDTO
{
    public string? Message { get; set; }

    public bool IsRead { get; set; }
}
