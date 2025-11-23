# Parking Ticket Management App

A small ASP.NET Core Web API (targeting .NET 10) that implements buying, validating and using monthly parking tickets. The service uses an in-memory store and is intended as training exercise.

## Quick overview

- **Entry point:** `Parking-Ticket-Management-App/Program.cs`
- **Controller:** `Parking-Ticket-Management-App/Controllers/ParkingTicketController.cs`
- **Business logic:** `Parking-Ticket-Management-App/Logic/` (buy/validate/use/price handlers)
- **In-memory storage:** `Parking-Ticket-Management-App/Memory/ParkingTicketMemory.cs` and `MemoryHandler.cs`
- **Models:** `Parking-Ticket-Management-App/Memory/Models/ParkingTicket.cs` and controller models under `Controllers/Models/`
- **Utilities:** `Parking-Ticket-Management-App/Utils/SystemDateTimeProvider.cs` and `Utils/Extensions/DateTimeExtensions.cs`

## Endpoints

| Method | Route                                      | Description |
|--------|--------------------------------------------|-------------|
| GET    | `/parking-ticket/is-alive`                 | Health check |
| POST   | `/parking-ticket/buy`                      | Buy a monthly ticket (request body: `BuyTicketRequest`) |
| GET    | `/parking-ticket/validate`                 | Validate ticket by license plate (query or model: `ValidateTicketRequest`) |
| PATCH  | `/parking-ticket/{TicketId:guid}/use`      | Mark a ticket as used (body: `UseTicketRequest`) |

Swagger/OpenAPI is available in development when running the project.

## Key behaviours / notes

- Tickets are stored in-process (in-memory). Restarting the app clears stored tickets.
- Requests are validated using FluentValidation validators placed alongside the models.
- Price calculation is implemented in `Logic/PriceCalculationLogicHandler.cs` (see `PricePerDay`).
- `BuyTicketLogicHandler` calculates `ValidFrom`/`ValidTo` using Central European time logic.

## Tests

Unit tests are in the `Parking-Ticket-Management-App-Tests` project:

- `BuyTicketLogicTests.cs`
- `PriceCalculationLogicTests.cs`
- `ValidateTicketLogicTests.cs`
- `UseTicketLogicTests.cs`

Run tests with:

```powershell
dotnet test
```

## Build & run

Prerequisites:

- .NET 10 SDK

Restore, build and run the API from the repository root:

```powershell
dotnet restore
dotnet run --project Parking-Ticket-Management-App
```

Open `http://localhost:5000` (or the URL shown by the app) and the Swagger UI when running in Development.

## Example requests

Buy ticket (POST `/parking-ticket/buy`):

```json
{
  "licensePlate": "AB12345"
}
```

Validate ticket (GET `/parking-ticket/validate?licensePlate=AB12345`)

Use ticket (PATCH `/parking-ticket/{ticketId}/use`)

## CI

There is a GitHub Actions pipeline that builds the solution and runs tests on push/PR to `main` (see `.github/workflows/`).

## License

MIT

---

Contributions, bug reports and improvements are welcome. If you'd like, I can also add example HTTP files or Postman collection for quick manual testing.
