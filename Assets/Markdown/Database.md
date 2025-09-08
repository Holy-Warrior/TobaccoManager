# Database

## Overview
This project uses a set of C# model classes to represent the main entities in the database. These classes are designed for use with an ORM (such as Entity Framework), which maps them to database tables. Each class corresponds to a table, and navigation properties represent relationships between tables.

## Model Classes and Structure

### 1. User
Represents an application user (admin or regular user).

- **Properties:**
	- `Id` (int, primary key, auto-generated)
	- `Name` (string, required)
	- `Email` (string, required)
	- `Password` (string, required)
	- `SecurityQuestion` (string, required)
	- `SecurityAnswer` (string, required)
	- `Role` (string, required, e.g., "user" or "admin")

**Table structure:**
| Id | Name | Email | Password | SecurityQuestion | SecurityAnswer | Role |
|----|------|-------|----------|------------------|---------------|------|

**Access:**
Users are typically accessed for authentication, authorization, and user management. Queries are performed by email or Id.

---

### 2. Customer
Represents a customer entity.

- **Properties:**
	- `Id` (int, primary key, auto-generated)
	- `Name` (string, required)
	- `Phone` (string, optional)
	- `Address` (string, optional)

**Table structure:**
| Id | Name | Phone | Address |
|----|------|-------|---------|

**Access:**
Customers are accessed for sales, stock delivery, and reporting. At least one of Phone or Address must be provided.

---

### 3. Stock
Represents a stock purchase or delivery event.

- **Properties:**
	- `Id` (int, primary key, auto-generated)
	- `Price` (decimal)
	- `DateReceived` (DateOnly)
	- `DatePaid` (DateOnly?, nullable)
	- `Bundles` (List<Bundle>, navigation property)
	- `NoOfBundles` (int, computed)
	- `GrandTotalPrice` (decimal, computed)

**Table structure:**
| Id | Price | DateReceived | DatePaid |
|----|-------|-------------|----------|

**Access:**
Stocks are accessed to track inventory, payments, and bundle details. The `Bundles` property links to all bundles in this stock.

---

### 4. Bundle
Represents a bundle of tobacco within a stock.

- **Properties:**
	- `Id` (int, primary key, auto-generated)
	- `Weight` (decimal, kg)
	- `PricePerKg` (decimal)
	- `LeafGrade` (enum Grade: A, B, C, D, E)
	- `TotalPrice` (decimal, computed)

**Table structure:**
| Id | Weight | PricePerKg | LeafGrade |
|----|--------|------------|-----------|

**Access:**
Bundles are always accessed via their parent Stock. The `TotalPrice` is calculated as `Weight * PricePerKg`.

---

## Database Relationships
- **Stock** has many **Bundles** (one-to-many)
- **User** and **Customer** are independent tables

## Access Patterns
- Use the ORM's context (e.g., `DbContext`) to query, add, update, or delete entities.
- Navigation properties (e.g., `Stock.Bundles`) allow loading related data.
- Computed properties (e.g., `GrandTotalPrice`) are calculated in code, not stored in the database.

## Example: Accessing Data
```csharp
// Get all stocks with their bundles
var stocks = dbContext.Stocks.Include(s => s.Bundles).ToList();

// Add a new customer
dbContext.Customers.Add(new Customer("John Doe", phone: "123456789"));
dbContext.SaveChanges();

// Get all users with role 'admin'
var admins = dbContext.Users.Where(u => u.Role == "admin").ToList();
```

## Reference Notes
- All primary keys are auto-generated.
- Use navigation properties to traverse relationships.
- Computed properties are for convenience in code and not mapped to database columns.

---

This document serves as a reference for the database structure and how to access model data in code during development.


