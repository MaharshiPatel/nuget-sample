namespace PaymentApp.Models;

/// <summary>
/// Data model for an invoice used when generating PDFs.
/// </summary>
public class InvoiceModel
{
    public int InvoiceNumber { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }

    public InvoiceAddress SellerAddress { get; set; } = null!;
    public InvoiceAddress CustomerAddress { get; set; } = null!;

    public List<InvoiceLineItem> Items { get; set; } = new();
    public string? Comments { get; set; }
}

public class InvoiceLineItem
{
    public string Name { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    public decimal Total => UnitPrice * Quantity;
}

public class InvoiceAddress
{
    public string CompanyName { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
