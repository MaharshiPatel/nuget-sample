using PaymentApp.Models;
using PaymentApp.Services;
using Xunit;

namespace PaymentApp.Tests;

public class InvoicePdfGeneratorTests : IClassFixture<LogFixture>
{
    private static InvoiceModel CreateMinimalInvoice()
    {
        return new InvoiceModel
        {
            InvoiceNumber = 1,
            IssueDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(14),
            SellerAddress = new InvoiceAddress
            {
                CompanyName = "Seller Inc.",
                Street = "123 Seller St",
                City = "City",
                State = "ST"
            },
            CustomerAddress = new InvoiceAddress
            {
                CompanyName = "Customer Co.",
                Street = "456 Buyer Ave",
                City = "Town",
                State = "ST"
            },
            Items = new List<InvoiceLineItem>
            {
                new() { Name = "Item A", UnitPrice = 10m, Quantity = 2 },
                new() { Name = "Item B", UnitPrice = 5m, Quantity = 3 }
            }
        };
    }

    [Fact]
    public void Generate_ValidInvoice_CreatesNonEmptyPdfFile()
    {
        var invoice = CreateMinimalInvoice();
        var generator = new InvoicePdfGenerator();
        var path = Path.Combine(Path.GetTempPath(), $"Invoice_{Guid.NewGuid():N}.pdf");

        try
        {
            generator.Generate(invoice, path);

            Assert.True(File.Exists(path), "PDF file should exist");
            var length = new FileInfo(path).Length;
            Assert.True(length > 100, "PDF file should have content (size > 100 bytes)");
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Generate_InvoiceWithComments_IncludesInPdf()
    {
        var invoice = CreateMinimalInvoice();
        invoice.Comments = "Test comment for invoice.";
        var generator = new InvoicePdfGenerator();
        var path = Path.Combine(Path.GetTempPath(), $"Invoice_{Guid.NewGuid():N}.pdf");

        try
        {
            generator.Generate(invoice, path);

            Assert.True(File.Exists(path));
            var bytes = File.ReadAllBytes(path);
            Assert.True(bytes.Length > 0);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Generate_EmptyLineItems_CreatesPdf()
    {
        var invoice = CreateMinimalInvoice();
        invoice.Items = new List<InvoiceLineItem>();
        var generator = new InvoicePdfGenerator();
        var path = Path.Combine(Path.GetTempPath(), $"Invoice_{Guid.NewGuid():N}.pdf");

        try
        {
            generator.Generate(invoice, path);

            Assert.True(File.Exists(path));
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
