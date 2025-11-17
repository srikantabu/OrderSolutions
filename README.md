---

````md
# Orders Dashboard – Full Stack Application

A full-stack Orders Dashboard built using ASP.NET Core 9 Web API, React + TypeScript, Vite, Axios, and TailwindCSS.

The project implements real-world functionality including pagination, sorting, filtering, debounced search, status updates, and a clean UI/UX.

---

## Tech Stack

### Backend

- .NET 9 Web API
- C#
- In-memory data store (no database required)
- JSON enum serialization (`JsonStringEnumConverter`)
- API versioning (`v1` routes)
- CORS support
- Logging via `ILogger<T>`

### Frontend

- React 19
- TypeScript
- Vite
- Axios
- TailwindCSS

---

## Folder Structure

```text
OrderSolutions/
│
├── OrderApi/                     # Backend (.NET 9 Web API)
│   ├── Controllers/              # OrdersController
│   ├── DTOs/                     # OrdersResponse, OrderDto, UpdateStatusRequest
│   ├── Models/                   # Order, OrderStatus
│   ├── Services/                 # IOrderService, InMemoryOrderService
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── Program.cs
│   └── OrderApi.csproj
│
├── OrderApi.Tests/               # Backend test project (xUnit)
│   ├── InMemoryOrderServiceTests.cs
│   ├── OrdersControllerTests.cs
│   └── OrderApi.Tests.csproj
│
└── client/                       # Frontend (React + TypeScript + Vite)
    ├── src/
    │   ├── components/           # AppHeader, OrdersTable, Filters, SearchBar, Pagination, ErrorBox, etc.
    │   ├── pages/                # LandingPage, OrdersDashboard
    │   ├── services/             # ordersApi.ts (Axios wrapper)
    │   ├── types/                # Order.ts (types and DTO contracts)
    │   └── main.tsx              # App entry
    ├── index.html
    ├── package.json
    ├── tailwind.config.cjs / js
    └── tsconfig*.json
```

---

# Backend – ASP.NET Core 9 Web API

### Features Implemented

- Versioned API (`v1`)
- `GET /v1/orders`

  - Pagination
  - Search (Order ID + Customer Name)
  - Filtering by status
  - Sorting by amount and date

- `POST /v1/orders/{id}/status` to update order status
- Randomized seed data generator in `InMemoryOrderService`
- Enum serialization using `JsonStringEnumConverter`
- Controller + service architecture with DI (`IOrderService`)
- CORS enabled for the React client
- Logging in service and controller for key operations

---

## API Endpoints

### 1. GET `/v1/orders`

Supports query parameters:

- `page` – page number (1-based)
- `limit` – page size
- `search` – search by Order ID or Customer Name (case-insensitive)
- `status` – filter by status (`Pending`, `Completed`, `Cancelled`, or `all`)
- `sortBy` – `amount` or `createddate`
- `order` – `asc` or `desc`

Example:

```http
GET /v1/orders?page=1&limit=10&search=john&status=Pending&sortBy=amount&order=asc
```

Response shape:

```json
{
  "data": [
    {
      "id": 1,
      "customerName": "John Doe",
      "amount": 120.5,
      "status": "Pending",
      "createdDate": "2025-01-01T12:00:00Z"
    }
  ],
  "page": 1,
  "totalPages": 5,
  "totalRecords": 42
}
```

---

### 2. POST `/v1/orders/{id}/status`

Updates the status of a single order.

Example:

```http
POST /v1/orders/12/status
Content-Type: application/json

{ "status": "Completed" }
```

- Returns `200 OK` with a success message if the order exists and is updated.
- Returns `404 NotFound` if the order ID does not exist.
- Returns `400 BadRequest` for invalid status payload.

---

## Program.cs (Key Configuration)

```csharp
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using OrderApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers + enum serialization as strings
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;

    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Api-Version")
    );
});

// In-memory order service
builder.Services.AddSingleton<IOrderService, InMemoryOrderService>();

// CORS for frontend
var corsPolicy = "AllowFrontend";
builder.Services.AddCors(o =>
{
    o.AddPolicy(corsPolicy, policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors(corsPolicy);
app.MapControllers();
app.Run();

// For integration testing (WebApplicationFactory<Program>)
public partial class Program { }
```

---

# Frontend – React + TypeScript + Vite

### Features Implemented

- Modular component structure
- TailwindCSS styling
- Orders table with:

  - Sorting on Amount and Date (ASC/DESC), with arrow indicators
  - Pagination (page size 10)
  - Status drop-down for updating each order (`Pending`, `Completed`, `Cancelled`)

- Search bar:

  - Debounced by 300 ms to avoid excessive API calls
  - Searches by Order ID and Customer Name

- Filter dropdown:

  - `All`, `Pending`, `Completed`, `Cancelled`

- Landing page:

  - Simple landing screen with navigation to the Orders Dashboard

- Shared header/navigation component:

  - Used on both Landing and Dashboard pages for consistent layout

- Loading overlay:

  - Spinner overlay while data is loading

- Error handling:

  - Dedicated `ErrorBox` with a “Retry” action

---

## Axios API Layer (`ordersApi.ts`)

- Centralized Axios instance with `baseURL` configured via environment (`VITE_API_URL`).
- Functions:

  - `getOrders(params)` → calls `GET /v1/orders` with query params
  - `updateStatus(id, status)` → calls `POST /v1/orders/{id}/status`

---

## package.json (Main Dependencies)

```json
"dependencies": {
  "axios": "^1.13.2",
  "react": "^19.2.0",
  "react-dom": "^19.2.0"
},
"devDependencies": {
  "@vitejs/plugin-react": "^5.1.0",
  "autoprefixer": "^10.4.22",
  "postcss": "^8.5.6",
  "tailwindcss": "^3.4.18",
  "typescript": "~5.9.3",
  "vite": "^7.2.2"
}
```

---

# Running the Project

## 1. Run Backend

From solution root:

```bash
cd OrderApi
dotnet run
```

Backend will run on (from launchSettings or console):

```text
http://localhost:5179
```

---

## 2. Run Frontend

From solution root:

```bash
cd client
npm install
npm run dev
```

Frontend runs at:

```text
http://localhost:5173
```

Make sure the backend CORS origin matches this URL.

---

# Testing

## Backend Testing

Backend tests are implemented using xUnit in the `OrderApi.Tests` project.

### Running Tests

From solution root:

```bash
dotnet test
```

### Test Coverage

- `InMemoryOrderServiceTests`

  - `Seed_ShouldCreateOrders`
    Ensures the in-memory service seeds a non-empty set of orders on initialization.
  - `UpdateStatus_ShouldUpdate_WhenOrderExists`
    Verifies that updating an existing order’s status returns `true` and that the stored status is actually changed.
  - `UpdateStatus_ShouldReturnFalse_WhenOrderNotFound`
    Confirms that attempts to update a non-existent order return `false`.

- `OrdersControllerTests`

  - `GetOrders_ShouldReturnOk_AndData`
    Verifies `GET /v1/orders` returns `200 OK` and a non-empty `data` collection.
  - `UpdateStatus_ShouldReturnNotFound_ForInvalidId`
    Verifies `POST /v1/orders/{id}/status` returns `404 NotFound` for a non-existent order ID.
  - `GetOrders_FilterByStatus_ShouldReturnOnlyThatStatus`
    Ensures filtering by `status=Pending` only returns orders with status `"Pending"`.
  - `GetOrders_SearchByCustomerName_ShouldReturnMatchingResults`
    Uses a real customer name from the API and verifies that search results only contain matching names.

---

## Frontend Testing

- No automated frontend test runner is configured.
- Behavior has been manually verified for:

  - Debounced search
  - Filters and pagination
  - Sorting toggles
  - Status update interactions
  - Loading + error handling UI

---

# Purpose of the Project

This project demonstrates:

- Full-stack development across .NET and React.
- API design with filtering, sorting, pagination, and versioned routes.
- Clean separation of concerns (controller/service/DTO layers).
- React + TypeScript component design with a small but realistic UI.
- Practical handling of UX behaviors: debounced search, loading overlay, error messages, and sort feedback.
- Basic but meaningful automated backend testing.

---

# License

MIT License.

```

```
