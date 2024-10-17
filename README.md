# LightNap

LightNap is a starter kit web application designed to provide a robust starting point for Single Page Applications (SPA) using a stack of .NET Core Web API, Angular, and PrimeNG. It includes built-in support for .NET Core Identity, JWT token management, and administrative features for managing identity, offering a solid foundation to be extended for any application scenario.

## Features

- **.NET Core Web API**: Backend services built with .NET Core.
- **SQL Server**: A data provider implementation for SQL Server.
- **.NET Core Identity**: Out-of-the-box support for user authentication and authorization.
- **JWT Token Management**: Secure token-based authentication.
- **Identity Management**: Administrative features for managing user roles and permissions.
- **Angular**: Frontend framework for building dynamic user interfaces.
- **PrimeNG**: Rich set of UI components for Angular.

## Getting Started

### Prerequisites

- .NET Core SDK
- Node.js and npm
- Angular CLI

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/sharplogic/LightNap.git
   cd LightNap
   ```

2. **Backend Setup:**
   - Navigate to the `src` directory:
     ```bash
     cd src
     ```
   - Restore .NET Core dependencies:
     ```bash
     dotnet restore
     ```
   - Update the database:
     ```bash
     dotnet ef database update
     ```
   - Run the application:
     ```bash
     dotnet run --project LightNap.WebApi
     ```

3. **Frontend Setup:**
   - Navigate to the `LightNap.Angular` directory:
     ```bash
     cd LightNap.Angular
     ```
   - Install Angular dependencies:
     ```bash
     npm install
     ```
   - Run the Angular application:
     ```bash
     ng serve
     ```

### Usage

- Access the application at `http://localhost:4200`.

## Project Structure

- `LightNap.Angular`: Angular project with PrimeNG components based on the [sakai-ng](https://github.com/primefaces/sakai-ng) template.
- `LightNap.Core`: .NET shared library for common server-side components.
- `LightNap.DataProviders.SqlServer`: SQL Server data provider implementation including migrations and utilities.
- `LightNap.MaintenanceService`: .NET Core console project to run maintenance tasks.
- `LightNap.WebApi`: .NET Core Web API project.
