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
COPY PaymentApp/PaymentReport.frx .
ENTRYPOINT ["dotnet", "PaymentApp.dll"]
