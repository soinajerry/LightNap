# LightNap

LightNap (**light**weight .**N**ET Core/**A**ngular/**P**rimeNG) is a full stack starter kit designed to provide a boost to Single Page Applications (SPA). It includes built-in support for .NET Core Identity, JWT token management, and administrative features for managing identity, offering a solid foundation to be extended for any application scenario.

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
   - Navigate to the `lightnap-ng` directory:
     ```bash
     cd lightnap-ng
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

- `lightnap-ng`: Angular project with PrimeNG components based on the [sakai-ng](https://github.com/primefaces/sakai-ng) template.
- `LightNap.Core`: .NET shared library for common server-side components.
- `LightNap.DataProviders.SqlServer`: SQL Server data provider implementation including migrations and utilities.
- `LightNap.MaintenanceService`: .NET Core console project to run maintenance tasks.
- `LightNap.WebApi`: .NET Core Web API project.

### Configuring LightNap.WebApi

- **appsettings.json**: Configuration settings for the Web API.

#### ConnectionStrings
- **ConnectionStrings.DefaultConnection**: The connection string for the default database.

#### SiteSettings
- **SiteSettings.AutomaticallyApplyEfMigrations**: Boolean indicating whether to automatically apply EF migrations. (e.g., true)
- **SiteSettings.RequireTwoFactorForNewUsers**: Boolean indicating whether to require two-factor authentication for new users. (e.g., false)
- **SiteSettings.SiteUrlRootForEmails**: The root URL used in emails. (e.g., "https://localhost:4200/")
- **SiteSettings.UseSameSiteStrictCookies**: Boolean indicating whether to use SameSite strict cookies. (e.g., false)

#### Administrators
- **Administrators**: List of administrator accounts to seed.
  - **Email**: The email address of the administrator. (e.g., "admin@admin.com")
  - **UserName**: The username of the administrator. (e.g., "admin")
  - **Password**: The password of the administrator. (e.g., "A2m!nPassword")

#### Jwt
- **Jwt.Key**: The secret key used for JWT token generation. (e.g., "PutAGuidHereSoYouMeetThe256+BitRequirement")
- **Jwt.Issuer**: The issuer of the JWT token. (e.g., "https://www.yourdomain.com")
- **Jwt.Audience**: The audience of the JWT token. (e.g., "https://www.yourdomain.com")
- **Jwt.ExpirationMinutes**: The expiration time of the JWT token in minutes. (e.g., 120)

#### Smtp
- **Smtp.Host**: The SMTP server host. (e.g., "smtp.sendgrid.net")
- **Smtp.Port**: The SMTP server port. (e.g., 587)
- **Smtp.EnableSsl**: Boolean indicating whether to enable SSL for SMTP. (e.g., true)
- **Smtp.User**: The SMTP user. (e.g., "apikey")
- **Smtp.Password**: The SMTP password or API key. (e.g., "Your SendGrid API Key>")
- **Smtp.FromEmail**: The email address used for sending emails. (e.g., "hello@yourdomain.com")
- **Smtp.FromDisplayName**: The display name used for sending emails. (e.g., "LightNap Team")
