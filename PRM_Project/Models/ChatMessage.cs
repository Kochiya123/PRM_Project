using System;
using System.Collections.Generic;

namespace PRM_Project.Models;

public partial class ChatMessage
{
    public int ChatMessageId { get; set; }

    public int? UserId { get; set; }

    public string? Message { get; set; }

    public DateTime SentAt { get; set; }

    public virtual User? User { get; set; }
}

public partial class AddChatMessageDTO
{
    public int? UserId { get; set; }

    public string? Message { get; set; }

    public DateTime SentAt { get; set; }
}

public partial class UpdateChatMessageDTO
{
    public string? Message { get; set; }

    public DateTime SentAt { get; set; }
}