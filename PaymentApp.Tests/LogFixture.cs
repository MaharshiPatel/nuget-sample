using Serilog;

namespace PaymentApp.Tests;

/// <summary>
/// Configures Serilog so that code under test (e.g. PaymentService, InvoicePdfGenerator) does not throw when logging.
/// </summary>
public sealed class LogFixture : IDisposable
{
    public LogFixture()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Fatal()
            .WriteTo.Console()
            .CreateLogger();
    }

    public void Dispose() => Log.CloseAndFlush();
}
