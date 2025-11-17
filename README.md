---
# Orders Dashboard – Full Stack Application

A complete full-stack Orders Dashboard built using ASP.NET Core 9 Web API, React + TypeScript, Vite, Axios, and TailwindCSS.

This project implements real-world functionality including pagination, sorting, filtering, debounced search, status updates, and clean UI/UX.
---

## Tech Stack

### Backend

- .NET 9 Web API
- C#
- In-memory data store (no database required)
- JSON Enum serialization
- CORS support

### Frontend

- React 19
- TypeScript
- Vite
- Axios
- TailwindCSS

---

## Folder Structure

```
OrdersSolution/
│
├── OrderApi/                 # Backend (.NET 9)
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Program.cs
│   └── OrderApi.csproj
│
└── client/                   # Frontend (React + TS)
    ├── src/
    │   ├── components/
    │   ├── api/
    │   ├── types/
    │   └── main.tsx
    ├── index.html
    ├── package.json
    └── tailwind.config.js
```

---

# Backend – ASP.NET Core 9 Web API

### Features Implemented

- GET /orders

  - Pagination
  - Search (Order ID + Customer Name)
  - Filtering by status
  - Sorting by amount and date

- POST /orders/{id}/status to update order status
- Randomized seed data generator
- Enum serialization via JsonStringEnumConverter
- Controller + service architecture
- CORS enabled for frontend

---

## API Endpoints

### GET /orders

Supports query parameters:

```
page, limit, search, status, sortBy, order
```

Example:

```
/orders?page=1&limit=10&search=john&status=pending&sortBy=amount&order=asc
```

---

### POST /orders/{id}/status

Example:

```
/orders/12/status
```

Body:

```json
{ "status": "Completed" }
```

---

## Program.cs Configuration

```csharp
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddSingleton<IOrderService, InMemoryOrderService>();

builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowFrontend");
app.MapControllers();
app.Run();
```

---

# Frontend – React + TypeScript + Vite

### Features Implemented

- Modular component structure
- TailwindCSS for styling
- Table sorting with ASC/DESC indicators
- Filters and pagination
- Debounced search (300ms)
- Axios service layer
- Status badges with readable text
- Loading and error states

---

## package.json (Main Dependencies)

```json
"dependencies": {
  "axios": "^1.13.2",
  "react": "^19.2.0",
  "react-dom": "^19.2.0"
},
"devDependencies": {
  "tailwindcss": "^3.4.18",
  "typescript": "~5.9.3",
  "vite": "^7.2.2",
  "@vitejs/plugin-react": "^5.1.0"
}
```

---

# Running the Project

## 1. Run Backend

```
cd OrderApi
dotnet run
```

Backend runs at:

```
http://localhost:5179
```

---

## 2. Run Frontend

```
cd client
npm install
npm run dev
```

Frontend runs at:

```
http://localhost:5173
```

---

# Screens / Features

- Orders table with sorting and pagination
- Search bar with debouncing
- Status dropdown filter
- Status update functionality
- Clean, centered layout using TailwindCSS
- Error and loading handling

---

# Testing

- Validated using Postman
- Verified CORS setup
- Checked JSON formats
- Confirmed debounced search calls
- Tested status update endpoint

---

# Purpose of the Project

This project demonstrates:

- Full-stack development capability
- API design and filtering/sorting/pagination logic
- React + TypeScript component architecture
- Real-world frontend behavior (debounce, UI state, sorting indicators)
- Clean and maintainable code structure

---

# License

MIT License.

---
