using PaymentApp;
using Xunit;

namespace PaymentApp.Tests;

public class PaymentServiceTests : IClassFixture<LogFixture>
{
    private readonly PaymentService _sut = new();

    [Fact]
    public void GetPayments_Initially_ReturnsEmptyList()
    {
        var payments = _sut.GetPayments();

        Assert.NotNull(payments);
        Assert.Empty(payments);
    }

    [Fact]
    public void AddPayment_AddsPayment_GetPaymentsReturnsIt()
    {
        var payment = new Payment
        {
            Id = 1,
            Payer = "Alice",
            Amount = 100.50m,
            Date = new DateTime(2025, 3, 1)
        };

        _sut.AddPayment(payment);

        var payments = _sut.GetPayments();
        Assert.Single(payments);
        Assert.Equal(1, payments[0].Id);
        Assert.Equal("Alice", payments[0].Payer);
        Assert.Equal(100.50m, payments[0].Amount);
        Assert.Equal(new DateTime(2025, 3, 1), payments[0].Date);
    }

    [Fact]
    public void AddPayment_MultiplePayments_GetPaymentsReturnsAll()
    {
        _sut.AddPayment(new Payment { Id = 1, Payer = "Alice", Amount = 10m, Date = DateTime.Today });
        _sut.AddPayment(new Payment { Id = 2, Payer = "Bob", Amount = 20m, Date = DateTime.Today });

        var payments = _sut.GetPayments();

        Assert.Equal(2, payments.Count);
        Assert.Equal(1, payments[0].Id);
        Assert.Equal(2, payments[1].Id);
    }
}
