# PaymentApp

A sample .NET payment application with reporting using FastReport.OpenSource (2020.3.22), Serilog logging, and Docker containerization.

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

## Notes
- The report template file `PaymentReport.frx` must be present in the working directory.
- Logging is handled by Serilog and can be set to production or debug mode.
- FastReport.OpenSource 2020.3.22 is used for reporting.

## License
MIT
