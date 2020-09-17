# Everybus API

## Database setup
1. Run the application in Visual Studio using the Docker Compose option
2. The application will likely fail
3. Run `dotnet ef database update --project .\EveryBus.csproj -v` to migrate the database and setup the tables