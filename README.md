# Product Catalog & Dynamic QR Code System

A robust, enterprise-ready backend API built with .NET 10, PostgreSQL, and JWT Authentication. This system enables users to perform full CRUD operations on product inventory and dynamically generate QR codes. Instead of embedding static product information directly into the QR code, this system embeds a secure tracking URL containing the unique Product Identifier. This allows inventory data (such as price, name, and dimensions) to be updated in real-time without re-printing physical materials.

---

## 1. System Architecture & Use Case Flowg

The system architecture decouples the administration panel, public client interface, database layer, and generation engine.

```text
[Admin Client (React)] ──( Auth / CRUD )──► [ .NET 10 API Engine ] ──► [ PostgreSQL DB ]
│
( Dynamic URL Generation )
│
▼
[End Consumer] ◄───────( Scans QR )──────── [ QR Code Output ]

```
### Flow 1: Administrator Product Creation & QR Issuance
1. **Authentication**: The system administrator authenticates via the React portal, receiving a cryptographically signed JWT token.
2. **Persistence Request**: The administrator submits product details (Name, Price, Color, Size) via a `POST` request, passing the bearer token in the HTTP authorization header.
3. **Identifier Generation**: The API generates a globally unique identifier (UUIDv4) and persists the record into the PostgreSQL database.
4. **Dynamic Encoding**: The API constructs an absolute URL targeting the public-facing React route (e.g., `https://domain.com/products/{id}`).
5. **QR Compilation**: The URL string is converted into a matrix barcode byte stream, encoded into Base64 format, and returned within the JSON response payload.
6. **Physical Provisioning**: The administrative UI displays the barcode for local caching, distribution, or physical application to inventory items.

### Flow 2: Consumer Retrieval Flow
1. **Physical Capture**: An end-consumer scans the printed QR code using a standard smartphone optical sensor.
2. **Client Routing**: The smartphone interprets the encoded payload as an HTTPS URL and instantiates the system's public web view interface.
3. **State Extraction**: The client-side framework parses the unique product identifier directly from the route parameters.
4. **Data Acquisition**: The web interface issues an unauthenticated asynchronous `GET` request to the backend REST endpoint containing the product identifier.
5. **Hydration**: The API runs an optimized lookup query against the PostgreSQL cluster and streams the current schema values back to the client interface for presentation.

---

## 2. Technology Stack

* **Runtime Environment**: .NET 10.0 SDK (LTS)
* **Database Engine**: PostgreSQL 17
* **Object-Relational Mapper**: Entity Framework Core 10
* **Authorization Protocol**: JSON Web Tokens (JWT) / Microsoft.AspNetCore.Authentication.JwtBearer
* **Matrix Code Generation Engine**: QRCoder Component Architecture
* **API Documentation**: OpenAPI / Swagger UI

---

## 3. Database Schema

The database architecture leverages PostgreSQL primary key sequencing optimized for distributed contexts via UUID serialization.

```sql
CREATE TABLE "Products" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Name" VARCHAR(255) NOT NULL,
    "Price" DECIMAL(18, 2) NOT NULL,
    "Color" VARCHAR(50) NOT NULL,
    "Size" VARCHAR(50) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
```
---

## 4. API Endpoint Specifications
All endpoints communicate via standard application/json payloads. Protected endpoints enforce bearer authorization schemes.

### Authentication Endpoints
#### User Authentication

- Endpoint: POST /api/auth/login

- Access: Public

- Request Payload:

```json
{
  "username": "administrator",
  "password": "SecurePassword123"
}
```

- Response Payload (200 OK):
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2026-05-28T14:11:00Z"
}
```

## Product Infrastructure Endpoints
### Create Product Record
- Endpoint: POST /api/products

- Access: Authenticated (Requires Valid JWT Bearer Token)

- Headers: Authorization: Bearer <token>

- Request Payload:

```json
{
  "name": "Industrial Carbon Fiber Chassis",
  "price": 1249.99,
  "color": "Matte Black",
  "size": "XL"
}
```

- Response Payload (201 Created):

```json

{
  "id": "8f3b2326-cd5b-4b2a-9f5e-bd50fdfce021",
  "name": "Industrial Carbon Fiber Chassis",
  "price": 1249.99,
  "color": "Matte Black",
  "size": "XL",
  "createdAt": "2026-05-28T08:11:00Z",
  "qrCodeBase64Payload": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAM0AAADN..."
}
```

### Fetch Single Product Configuration
- Endpoint: GET /api/products/{id}

- Access: Public (Consumed by React Client scanned from QR Code)

- Response Payload (200 OK):

```json
  {
  "id": "8f3b2326-cd5b-4b2a-9f5e-bd50fdfce021",
  "name": "Industrial Carbon Fiber Chassis",
  "price": 1249.99,
  "color": "Matte Black",
  "size": "XL"
}
```

### Update Product Specifications
- Endpoint: PUT /api/products/{id}

- Access: Authenticated (Requires Valid JWT Bearer Token)

- Headers: Authorization: Bearer <token>

- Request Payload:

```json
{
  "name": "Industrial Carbon Fiber Chassis",
  "price": 1199.99,
  "color": "Matte Black",
  "size": "XL"
}
```

- Response Payload (204 No Content): Empty execution confirmation.

### Evict Product Record
- Endpoint: DELETE /api/products/{id}

- Access: Authenticated (Requires Valid JWT Bearer Token)

- Headers: Authorization: Bearer <token>

- Response Payload (204 No Content): Empty execution confirmation.

---
## 5. OpenAPI and Swagger Engine Interactivity
The system features real-time interface reflection via Swagger middleware pipelines integrated inside the application initialization stack.

### Interface Access
During local development execution phases, the full interactive test sandbox is available at:

```text
https://localhost:5073/swagger/index.html
```

## 6. Operational Instantiation Commands
Verify Development Assets and Packages:

```text
dotnet restore
```

Execute Schema Database Context Migrations:


```text

dotnet ef database update
```
Instantiate Target Runtime Thread:

```text

dotnet run --project ProductCatalog.API
```
## 10. Generated Asset Output Example
When a product record is successfully committed via administrative channels, the configuration interface generates a dynamic vector matrix payload. This can be embedded directly inside client view templates using clean URI interpretation markup.

Target Scan Target URL Layout
```text
[https://domain.com/products/8f3b2326-cd5b-4b2a-9f5e-bd50fdfce021](https://domain.com/products/8f3b2326-cd5b-4b2a-9f5e-bd50fdfce021)

```
![Dynamic Product Routing QR Barcode](assets/qr.png)
