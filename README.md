# CreditWorks - Vehicle & Weight Category Management System

This project is a web application developed for the CreditWorks technical assessment. It manages vehicles and their corresponding weight categories, adhering to strict business rules regarding weight ranges, boundary handling, and dynamic categorization updates.

---

## Technical Stack

- **Backend API**: ASP.NET Core (.NET 10), C#
- **ORM & Database**: Entity Framework Core 10 with **SQLite**
- **Frontend**: Vue 3, TypeScript, Vite
- **Testing**: xUnit / NUnit (Backend tests)

---

## Setup & Running Instructions

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/)
- [Node.js](https://nodejs.org/) (v18+ recommended)

### 1. Database Configuration & Setup

The project uses SQLite, so no standalone database engine installation is required. The database file will be created automatically upon applying Entity Framework migrations or running the application.

1. Navigate to the API directory:
   ```bash
   cd api
   ```
2. Apply database migrations and seed initial data (Manufacturers & Categories):
   ```bash
   dotnet ef database update
   ```

### 2. Running the Application

#### Backend (API)

```bash
cd api
dotnet run
```

The API will start at `http://localhost:5288`.

#### Frontend (Vue 3 Client)

```bash
cd client
npm ci
npm run dev
```

Open your browser at `http://localhost:5173`.

### 3. Running Automated Tests

To run the automated test suite for domain validation, range logic, and boundary checks:

#### API Tests

```bash
dotnet test tests
```

#### Client Tests

```bash
npm run test:unit
```
