# Parking Ticket Management App

A modern ASP.NET Core Web API (.NET 10, C# 13) for managing parking tickets, including buying and validating monthly tickets.

## Features

- **Buy Parking Ticket:**  
  Purchase a monthly parking ticket for a vehicle.

- **Validate Parking Ticket:**  
  Check if a ticket for a given license plate is valid.

- **Health Check:**  
  Simple endpoint to verify the service is running.

- **Input Validation:**  
  Uses [FluentValidation](https://fluentvalidation.net/) for request validation.

## Endpoints

| Method | Route                        | Description                       |
|--------|-----------------------------|-----------------------------------|
| GET    | `/parking-ticket/is-alive`  | Health check                      |
| POST   | `/parking-ticket/buy`       | Buy a monthly parking ticket      |
| GET    | `/parking-ticket/validate`  | Validate a parking ticket         |

## Request/Response Models

### Buy Ticket

**Request:**  
```
{
  "licensePlate": "ABC123"
}
```

**Response:**  
```
{
  "ticketId": "guid",
  "amount": 5.00,
  "validFrom": "2025-10-04T00:00:00Z",
  "validTo": "2025-11-01T00:00:00Z"
}
```

### Validate Ticket

**Request:**  
```
{
  "licensePlate": "ABC123"
}
```

**Response:**  
```
{
  "licensePlate": "ABC123",
  "isValid": true
}
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Visual Studio 2022 or later

### Setup

1. **Clone the repository:**
```
   git clone https://github.com/PiotrekBudny/Parking-Ticket-Management-App.git
```

2. **Restore dependencies:**
```
   dotnet restore
```

3. **Run the application:**
```
   dotnet run
```

4. **API documentation:**  
   OpenAPI/Swagger is available in development mode at `/swagger` or `/openapi`.

### Testing Endpoints

You can use tools like [Postman](https://www.postman.com/) or Visual Studio's HTTP files to test the API.

## Project Structure

- `Controllers/` - API endpoints
- `Logic/` - Business logic and price calculation
- `Memory/` - In-memory ticket storage
- `Models/` - Request and response models
- `Extensions/` - Utility extensions (e.g., DateTime trimming)

## Validation

- All requests are validated using FluentValidation.
- Invalid requests return a `400 Bad Request` with error details.

## License

MIT

---

**Contributions and issues are welcome!**
