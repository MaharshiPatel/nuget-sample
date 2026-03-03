using PaymentApp.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PaymentApp.Invoice;

public class InvoiceDocument : IDocument
{
    public InvoiceModel Model { get; }

    public InvoiceDocument(InvoiceModel model)
    {
        Model = model;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);

                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item()
                    .Text($"Invoice #{Model.InvoiceNumber}")
                    .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                column.Item().Text(text =>
                {
                    text.Span("Issue date: ").SemiBold();
                    text.Span($"{Model.IssueDate:d}");
                });

                column.Item().Text(text =>
                {
                    text.Span("Due date: ").SemiBold();
                    text.Span($"{Model.DueDate:d}");
                });
            });

            row.ConstantItem(100).Height(50).Placeholder();
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(5);

            column.Item().Row(row =>
            {
                row.RelativeItem().Component(new AddressComponent("From", Model.SellerAddress));
                row.ConstantItem(50);
                row.RelativeItem().Component(new AddressComponent("Bill To", Model.CustomerAddress));
            });

            column.Item().PaddingTop(20).Element(ComposeTable);

            var totalPrice = Model.Items.Sum(x => x.Total);
            column.Item().AlignRight().PaddingTop(10).Text($"Grand total: {totalPrice:C}").FontSize(14).SemiBold();

            if (!string.IsNullOrWhiteSpace(Model.Comments))
                column.Item().PaddingTop(25).Element(ComposeComments);
        });
    }

    private void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(25);
                columns.RelativeColumn(3);
                columns.RelativeColumn();
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().Element(CellHeaderStyle).Text("#");
                header.Cell().Element(CellHeaderStyle).Text("Description");
                header.Cell().Element(CellHeaderStyle).AlignRight().Text("Unit price");
                header.Cell().Element(CellHeaderStyle).AlignRight().Text("Qty");
                header.Cell().Element(CellHeaderStyle).AlignRight().Text("Total");
            });

            foreach (var item in Model.Items)
            {
                table.Cell().Element(CellStyle).Text((Model.Items.IndexOf(item) + 1).ToString());
                table.Cell().Element(CellStyle).Text(item.Name);
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.UnitPrice:C}");
                table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity.ToString());
                table.Cell().Element(CellStyle).AlignRight().Text($"{item.Total:C}");
            }
        });
    }

    private static IContainer CellHeaderStyle(IContainer c) =>
        c.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);

    private static IContainer CellStyle(IContainer c) =>
        c.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);

    private void ComposeComments(IContainer container)
    {
        container.Background(Colors.Grey.Lighten3).Padding(10).Column(column =>
        {
            column.Spacing(5);
            column.Item().Text("Comments").FontSize(14).SemiBold();
            column.Item().Text(Model.Comments ?? string.Empty);
        });
    }
}
