using PaymentApp.Models;
using Xunit;

namespace PaymentApp.Tests;

public class InvoiceLineItemTests
{
    [Fact]
    public void Total_UnitPriceTimesQuantity_ReturnsCorrectValue()
    {
        var item = new InvoiceLineItem
        {
            Name = "Widget",
            UnitPrice = 25.00m,
            Quantity = 4
        };

        Assert.Equal(100.00m, item.Total);
    }

    [Theory]
    [InlineData(0, 10, 0)]
    [InlineData(99.99, 1, 99.99)]
    [InlineData(10, 0, 0)]
    public void Total_VariousInputs_ComputesCorrectly(decimal unitPrice, int quantity, decimal expectedTotal)
    {
        var item = new InvoiceLineItem
        {
            UnitPrice = unitPrice,
            Quantity = quantity
        };

        Assert.Equal(expectedTotal, item.Total);
    }
}
