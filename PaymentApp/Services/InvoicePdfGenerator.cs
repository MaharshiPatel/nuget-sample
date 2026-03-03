using PaymentApp.Invoice;
using PaymentApp.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Serilog;

namespace PaymentApp.Services;

/// <summary>
/// Generates invoice PDFs using QuestPDF.
/// </summary>
public class InvoicePdfGenerator
{
    /// <summary>
    /// Generates a PDF file for the given invoice model.
    /// </summary>
    /// <param name="invoice">Invoice data.</param>
    /// <param name="outputPath">Full path for the output PDF file.</param>
    public void Generate(InvoiceModel invoice, string outputPath)
    {
        Log.Information("Generating invoice PDF: #{InvoiceNumber} -> {OutputPath}", invoice.InvoiceNumber, outputPath);

        QuestPDF.Settings.License = LicenseType.Community;

        var document = new InvoiceDocument(invoice);
        Document.Create(container => document.Compose(container)).GeneratePdf(outputPath);

        Log.Information("Invoice PDF saved: {OutputPath}", outputPath);
    }
}
