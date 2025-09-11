

using System;
using System.Collections.Generic;
using FastReport;
using FastReport.Data;
using Serilog;

namespace PaymentApp
{
	public class Payment
	{
		public int Id { get; set; }
		public string Payer { get; set; }
		public decimal Amount { get; set; }
		public DateTime Date { get; set; }
	}

	public class PaymentService
	{
		private List<Payment> payments = new List<Payment>();

		public void AddPayment(Payment payment)
		{
			Log.Information("Adding payment: {@Payment}", payment);
			payments.Add(payment);
		}

		public List<Payment> GetPayments()
		{
			Log.Debug("Retrieving all payments");
			return payments;
		}
	}

	public class ReportService
	{
		public void GenerateReport(List<Payment> payments, string reportPath)
		{
			Log.Information("Generating report using template: {ReportPath}", reportPath);
			using (Report report = new Report())
			{
				report.Load(reportPath);
				report.RegisterData(payments, "Payments");
				report.Prepare();
				report.SavePrepared("PaymentReport.fpx");
			}
			Log.Information("Report generated and saved as PaymentReport.fpx");
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			// Usage: dotnet run -- [production|debug]
			var mode = args.Length > 0 ? args[0].ToLower() : "production";
			var level = mode == "debug" ? Serilog.Events.LogEventLevel.Debug : Serilog.Events.LogEventLevel.Information;

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Is(level)
				.WriteTo.Console()
				.CreateLogger();

			Log.Information("Application started in {Mode} mode", mode);

			var paymentService = new PaymentService();
			paymentService.AddPayment(new Payment { Id = 1, Payer = "Alice", Amount = 100.50m, Date = DateTime.Now });
			paymentService.AddPayment(new Payment { Id = 2, Payer = "Bob", Amount = 200.75m, Date = DateTime.Now });

			var payments = paymentService.GetPayments();

			var reportService = new ReportService();
			// You need to create a report template file (PaymentReport.frx) in the project directory
			reportService.GenerateReport(payments, "PaymentReport.frx");

			Log.Information("Payments and report generated.");

			Log.CloseAndFlush();
		}
	}
}
