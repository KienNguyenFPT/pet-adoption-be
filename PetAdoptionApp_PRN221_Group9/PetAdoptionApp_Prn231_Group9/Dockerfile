# Use ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

# Use .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["PetAdoptionApp_PRN221_Group9/PetAdoptionApp_Prn231_Group9/PetAdoptionApp_Prn231_Group9.csproj", "PetAdoptionApp/"]
COPY ["PetAdoptionApp_PRN221_Group9/DataAccessObjects/DataAccessObjects.csproj", "DataAccessObjects/"]
COPY ["PetAdoptionApp_PRN221_Group9/BusinessObjects/BusinessObjects.csproj", "BusinessObjects/"]
COPY ["PetAdoptionApp_PRN221_Group9/BusinessLogicLayer/BusinessLogicLayer.csproj", "BusinessLogicLayer/"]

# Restore project dependencies
RUN dotnet restore "PetAdoptionApp/PetAdoptionApp_Prn231_Group9.csproj"

# Set the working directory to the project folder
WORKDIR "/src/PetAdoptionApp"

# Copy the rest of the code
COPY . .

# Build the project with detailed output
RUN dotnet build "PetAdoptionApp_Prn231_Group9.csproj" -c $BUILD_CONFIGURATION --verbosity detailed -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "PetAdoptionApp_Prn231_Group9.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Final stage / runtime
FROM base AS final
WORKDIR /app

# Expose HTTP and HTTPS ports
EXPOSE 80
EXPOSE 443

# Copy the published output
COPY --from=publish /app/publish .

# Copy the appsettings.json file if it exists in the project root
COPY ["PetAdoptionApp_PRN221_Group9/PetAdoptionApp_Prn231_Group9/appsettings.json", "./"]

# Set environment variables
ENV DatabaseConnection="Server=(local);Uid=sa;Pwd=12345;Database=PetAdoptionApp;TrustServerCertificate=True;Encrypt=True"
ENV JWTSection__Issuer="PetAdoptionSystem"
ENV JWTSection__Audience="PAAAdmin"
ENV JWTSection__SecretKey="This Is More Than 128 bits Secret Key"

# Specify certificate and key paths if using HTTPS
# Ensure you have copied these files to the correct location in the container
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/https/cert.crt"
ENV ASPNETCORE_Kestrel__Certificates__Default__KeyPath="/https/cert.key"

# Set the ASP.NET Core URLs
ENV ASPNETCORE_URLS="http://+:80;http://+:443"

# Entry point for the application
ENTRYPOINT ["dotnet", "PetAdoptionApp_Prn231_Group9.dll"]
