 **User Registration & Authentication**  
  Secure registration and login using JWT authentication with password hashing and salting.

- **Customer Dashboard**  
  Allows customers to initiate and track international payments.

- **Employee Dashboard**  
  Enables employees to review, verify, and approve or reject transactions before processing.

- **Payment Processing**  
  Integration with SWIFT or other international payment networks for sending payments.

- **Security Measures**  
  Input validation, SSL enforcement, protection against common vulnerabilities (XSS, CSRF, SQL Injection).

---

## Technology Stack

- **Backend:** C# (.NET Core) Web API  
- **Frontend:** React.js  
- **Database:** SQL Server / Azure SQL  
- **Authentication:** JWT (JSON Web Tokens)  
- **Other Tools:** Entity Framework Core, Swagger (for API documentation)

---

## Getting Started

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) (version x.x or higher)  
- [Node.js & npm](https://nodejs.org/) (version x.x or higher)  
- SQL Server or Azure SQL instance  
- IDE (Visual Studio, VS Code, or equivalent)  

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/international-payment-system.git
   cd international-payment-system

2. Backend Setup:

Navigate to the backend folder:

bash
CopyEdit
cd backend
Configure database connection string in appsettings.json.

Run migrations to set up the database schema:

bash
CopyEdit
dotnet ef database update
Start the backend API:

bash
CopyEdit
dotnet run
3. Frontend Setup:

Navigate to the frontend folder:

bash

cd ../frontend
CopyEdit
Install dependencies:

bash
CopyEdit
npm install
Start the React development server:

bash
CopyEdit
npm start
4. Access the application at http://localhost:3000.








   
