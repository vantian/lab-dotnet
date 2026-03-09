FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files only — layer cache is reused until a .csproj changes
COPY BizCover.Api.Cars/BizCover.Api.Cars.csproj               BizCover.Api.Cars/
COPY BizCover.Application/BizCover.Application.csproj         BizCover.Application/
COPY BizCover.Infrastructure/BizCover.Infrastructure.csproj   BizCover.Infrastructure/
COPY BizCover.Domain/BizCover.Domain.csproj                   BizCover.Domain/
RUN dotnet restore "BizCover.Api.Cars/BizCover.Api.Cars.csproj"

# Copy full source
COPY . .
RUN dotnet build "BizCover.Api.Cars/BizCover.Api.Cars.csproj" -c Release --no-restore

FROM build AS publish
RUN dotnet publish "BizCover.Api.Cars/BizCover.Api.Cars.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "BizCover.Api.Cars.dll"]
