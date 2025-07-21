# PromoCode Factory

## Description
PromoCode Factory is a web application for managing promotional codes for customers. It enables the creation and distribution of promotional codes based on customer preferences. The system allows managing employees, customers, partners, and promotional codes through REST API and GraphQL interfaces.

Key features:
- Customer management with preference tracking
- Employee management with roles
- Promotional code generation and assignment to customers
- Partner management with promo code limits

## Technologies Used
- ASP.NET Core 8.0
- Entity Framework Core
- PostgreSQL Database
- GraphQL API with HotChocolate
- REST API with Swagger documentation
- Docker for containerization
- Unit Testing with xUnit

## How to Run

### Using Docker
1. Make sure you have Docker and Docker Compose installed on your machine.
2. Clone the repository and navigate to the project root directory.
3. Run the following command:
   ```
   docker-compose up -d
   ```
4. The application will be accessible at:
   - REST API: http://localhost:8091/swagger
   - GraphQL API: http://localhost:8091/playground

### Using CLI
1. Make sure you have .NET SDK 8.0 installed on your machine.
2. Clone the repository and navigate to the project root directory.
3. Configure the connection string in `src/PromoCodeFactory.WebHost/appsettings.json` and `src/PromoCodeFactory.GraphQL/appsettings.json` files to point to your local PostgreSQL or SQLite database.
4. Navigate to the WebHost project directory:
   ```
   cd src/PromoCodeFactory.WebHost
   ```
5. Run the application:
   ```
   dotnet run
   ```
6. For running the GraphQL API:
   ```
   cd src/PromoCodeFactory.GraphQL
   dotnet run
   ```
7. The REST API will be accessible at: https://localhost:7035/swagger
8. The GraphQL API will be accessible at: https://localhost:7192/playground 