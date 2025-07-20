using System;
using System.Collections.Generic;

namespace PRM_Project.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? OrderId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public virtual Order? Order { get; set; }
}

public partial class AddPaymentDTO
{
    public int? OrderId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }
}

public partial class UpdatePaymentDTO
{
    public int? OrderId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }
}

public partial class PaymentInformation
{
    public int? OrderId { get; set; }
    public decimal Amount { get; set; }
    public string OrderDescription { get; set; }
    public string Name { get; set; }
}

public partial class PaymentResponse
{
    public string OrderDescription { get; set; }
    public string TransactionId { get; set; }
    public int? OrderId { get; set; }
    public string PaymentMethod { get; set; }
    public int PaymentId { get; set; }
    public bool PaymentStatus { get; set; }
    public bool Success { get; set; }
    public string Token { get; set; }
    public string VnPayResponseCode { get; set; }
}