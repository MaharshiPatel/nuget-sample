

using System;
using System.Collections.Generic;
using FastReport;
using FastReport.Data;
using Newtonsoft.Json;
using PaymentApp.Models;
using PaymentApp.Services;
using Serilog;
using Serilog.Enrichers;

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
				.Enrich.WithThreadId()
				.Enrich.WithProcessId()
				.WriteTo.Console()
				.WriteTo.File("logs/paymentapp.log", rollingInterval: RollingInterval.Day)
				.CreateLogger();

			Log.Information("Application started in {Mode} mode", mode);

			var paymentService = new PaymentService();
			paymentService.AddPayment(new Payment { Id = 1, Payer = "Alice", Amount = 100.50m, Date = DateTime.Now });
			paymentService.AddPayment(new Payment { Id = 2, Payer = "Bob", Amount = 200.75m, Date = DateTime.Now });

			var payments = paymentService.GetPayments();

			// Newtonsoft.Json: serialize payments to JSON and save
			var json = JsonConvert.SerializeObject(payments, Formatting.Indented);
			System.IO.File.WriteAllText("payments.json", json);
			Log.Information("Payments written to payments.json");

			// Deserialize example (round-trip)
			var deserialized = JsonConvert.DeserializeObject<List<Payment>>(json);
			Log.Debug("Deserialized {Count} payments from JSON", deserialized?.Count ?? 0);

			var reportService = new ReportService();
			// You need to create a report template file (PaymentReport.frx) in the project directory
			reportService.GenerateReport(payments, "PaymentReport.frx");

			// Generate sample invoice PDF
			var invoice = new InvoiceModel
			{
				InvoiceNumber = 1001,
				IssueDate = DateTime.Today,
				DueDate = DateTime.Today.AddDays(14),
				SellerAddress = new InvoiceAddress
				{
					CompanyName = "PaymentApp Inc.",
					Street = "123 Business Ave",
					City = "New York",
					State = "NY",
					Email = "billing@paymentapp.com",
					Phone = "+1 (555) 123-4567"
				},
				CustomerAddress = new InvoiceAddress
				{
					CompanyName = "Acme Corp",
					Street = "456 Customer St",
					City = "Boston",
					State = "MA",
					Email = "payable@acme.com",
					Phone = "+1 (555) 987-6543"
				},
				Items = new List<InvoiceLineItem>
				{
					new() { Name = "Consulting", UnitPrice = 150.00m, Quantity = 10 },
					new() { Name = "License Fee", UnitPrice = 499.00m, Quantity = 1 },
					new() { Name = "Support", UnitPrice = 75.00m, Quantity = 4 }
				},
				Comments = "Thank you for your business. Payment is due within 14 days."
			};

			var invoicePdfGenerator = new InvoicePdfGenerator();
			invoicePdfGenerator.Generate(invoice, "Invoice.pdf");

			Log.Information("Payments, report, and invoice PDF generated.");

			Log.CloseAndFlush();
		}
	}
}
