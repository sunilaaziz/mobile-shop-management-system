# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set working directory
WORKDIR /app

# Copy project files
COPY *.csproj ./

# Restore dependencies
RUN dotnet restore

# Copy remaining files
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Use smaller runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0

WORKDIR /app

# Copy built app from build stage
COPY --from=build /app/out ./

# Run the application
ENTRYPOINT ["dotnet", "YourAppName.dll"]