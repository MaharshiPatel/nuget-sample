# PaymentApp

A sample .NET payment application with reporting using FastReport.OpenSource (2020.3.22), **PDF invoice generation** using QuestPDF, Serilog logging, and Docker containerization.

## Prerequisites
- [.NET SDK 6.0+](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)

## Build and Run Locally

1. **Restore NuGet Packages**
   ```sh
   dotnet restore PaymentApp
   ```

2. **Build the Project**
   ```sh
   dotnet build PaymentApp
   ```

3. **Run the Application**
   - Production mode (default):
     ```sh
     dotnet run --project PaymentApp
     ```
   - Debug mode (verbose logs):
     ```sh
     dotnet run --project PaymentApp -- debug
     ```

## Running unit tests

The solution includes a test project `PaymentApp.Tests` (xUnit). Run all tests:

```sh
dotnet test PaymentApp.Tests
```

Tests cover:
- **PaymentService** — add/get payments, initial empty list.
- **InvoiceLineItem** — `Total` (unit price × quantity).
- **InvoicePdfGenerator** — generates a non-empty PDF, supports comments and empty line items.

## Containerize with Docker

1. **Create a Dockerfile in the PaymentApp directory:**
   ```dockerfile
   FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
   WORKDIR /app

   FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
   WORKDIR /src
   COPY . .
   RUN dotnet restore PaymentApp/PaymentApp.csproj
   RUN dotnet publish PaymentApp/PaymentApp.csproj -c Release -o /app/publish

   FROM base AS final
   WORKDIR /app
   COPY --from=build /app/publish .
   ENTRYPOINT ["dotnet", "PaymentApp.dll"]
   ```

2. **Build the Docker Image:**
   ```sh
   docker build -t paymentapp ./PaymentApp
   ```

3. **Run the Docker Container:**
   - Production mode:
     ```sh
     docker run --rm paymentapp
     ```
   - Debug mode:
     ```sh
     docker run --rm paymentapp debug
     ```

## Invoice PDF generation

The app includes a PDF invoice tool built with [QuestPDF](https://www.questpdf.com/). On each run it generates `Invoice.pdf` in the working directory with sample seller/customer addresses and line items.

- **Models**: `PaymentApp/Models/Invoice.cs` — `InvoiceModel`, `InvoiceLineItem`, `InvoiceAddress`.
- **Generator**: `PaymentApp/Services/InvoicePdfGenerator.cs` — call `Generate(invoice, "path/to/Invoice.pdf")` to create a PDF.
- **Template**: `PaymentApp/Invoice/InvoiceDocument.cs` and `AddressComponent.cs` define the layout (header, addresses, line items table, total, comments).

To generate a custom invoice from code, build an `InvoiceModel`, create an `InvoicePdfGenerator`, and call `Generate(invoice, outputPath)`.

## Notes

- The report template file `PaymentReport.frx` must be present in the working directory.
- Logging is handled by Serilog and can be set to production or debug mode.
- FastReport.OpenSource 2020.3.22 is used for reporting.
- QuestPDF is used for invoice PDFs (Community license for development/evaluation).

## License
MIT
