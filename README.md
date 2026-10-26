# ElectronicShop Ecosystem

A high-signal, full-stack distributed system designed to demonstrate professional-grade software engineering patterns. This project bridges the gap between a cross-platform **.NET MAUI** client and a robust **ASP.NET Core Web API**, unified by a shared contract layer.

## 🏗 Architecture & Design Patterns

This project is built on a 4-layer distributed architecture to ensure scalability, maintainability, and strict separation of concerns.

### 1. Presentation Layer (.NET MAUI)
*   **Pattern:** Advanced **MVVM (Model-View-ViewModel)** using `CommunityToolkit.Mvvm`.
*   **Modularization:** Implements a **Sub-ViewModel pattern** (e.g., `IProfileSubViewModel`) to manage complex, multi-step user flows without creating "Massive ViewModels".
*   **Decoupled Navigation:** Utilizes a specialized `ICommunicateWithView` interface to allow ViewModels to trigger navigation and handle popup results without direct dependencies, making the UI layer highly testable.

### 2. Logic Layer (Client-Side Services)
*   **Pattern:** Service-Oriented Architecture.
*   **Abstraction:** Uses interfaces (e.g., `ICategoriesService`) to decouple the UI from the underlying API implementation, facilitating easier testing and potential provider swaps.

### 3. API Layer (ASP.NET Core 8)
*   **Pattern:** Controller-Service-Repository pattern.
*   **Security:** Implements **JWT (JSON Web Token) Authentication** with granular, **Claim-based Authorization** (Permission-based access control) to ensure secure resource access.
*   **Dependency Injection:** Heavy use of built-in DI containers to manage lifetimes (Scoped/Transient) and promote loose coupling.

### 4. Contract Layer (Shared Assembly)
*   **Pattern:** **DTO (Data Transfer Object) Pattern**.
*   **Single Source of Truth:** All data exchange between the Client and the API is governed by a shared assembly, ensuring type-safety and preventing serialization mismatches across the distributed system.

---

## 🛠 Tech Stack

| Component | Technology |
| :--- | :--- |
| **Client (Mobile/Desktop)** | .NET MAUI (Android, iOS, Windows, MacCatalyst) |
| **Backend (API)** | ASP.NET Core 8, C# |
| **Data Access** | Entity Framework Core |
| **Communication** | RESTful API / JSON |
| **Authentication** | JWT (Bearer Tokens) |
| **UI Toolkits** | CommunityToolkit.Maui, CommunityToolkit.Mvvm |

---

## 🚀 Key Engineering Highlights

*   **Contract-Driven Development:** By utilizing a shared `Shared.csproj`, the system ensures that the Client and Server are always synchronized through strongly-typed DTOs.
*   **Granular Security:** The API doesn't just check if a user is logged in; it checks specific permissions (e.g., `ModifyProducts`) using custom claims.
*   **Modular UI Logic:** The use of sub-viewmodels for complex pages (like User Profile) demonstrates advanced state management in mobile applications.

---

## 🗺 Roadmap (Development Pipeline)

This project is designed as a scalable MVP. The following features are planned for upcoming iterations to move towards a full production-ready state:

*   [ ] **Security Hardening:** Implementation of **Salted Password Hashing** (using BCrypt) and enhanced TLS configuration.
*   [ ] **Payment Integration:** Implementation of a **Payment Gateway Abstraction** (Mocked implementation for testing, ready for Stripe/PayPal integration).
*   [ ] **Testing Suite:** Comprehensive Unit Testing for ViewModels and Integration Testing for API endpoints.
*   [ ] **Observability:** Integration of structured logging (Serilog) and centralized error handling middleware.

---

## ⚙️ Getting Started

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download)
* [.MAUI Workload](https://learn.microsoft.com/en-us/dotnet/net/maui/get-started)

### Installation & Execution
1. **Clone the repository**
2. **Run the Backend:**
   ```bash
   cd ElectronicShopAPI
   dotnet run
   ```
3. **Run the Client:**
   ```bash
   cd ElectronicShopApp
   dotnet build -t:Run -f net10.0-windows10.0.19041.0
   ```