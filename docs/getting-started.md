---
title: "Getting Started"
description: "A guide to help you get started with the project."
tags: ["setup", "installation"]
---

## Prerequisites

- .NET Core SDK
- Node.js and npm
- Angular CLI

## Installation

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

   - Run the application:

     ```bash
     dotnet run --project LightNap.WebApi
     ```

3. **Frontend Setup:**

   - In a separate console, navigate to the `lightnap-ng` directory:

     ```bash
     cd src/lightnap-ng
     ```

   - Install Angular dependencies:

     ```bash
     npm install
     ```

   - Run the Angular application:

     ```bash
     ng serve
     ```

## Usage

- Access the application at `http://localhost:4200`.
- A default administrator account is created with:
    - **Email**: admin@admin.com
    - **UserName**: admin
    - **Password**: A2m!nPassword 

## Project Structure

- `lightnap-ng`: Angular project with PrimeNG components based on the [sakai-ng](https://github.com/primefaces/sakai-ng) template.
- `LightNap.Core`: .NET shared library for common server-side components.
- `LightNap.DataProviders.Sqlite`: SQLite data provider implementation including migrations and utilities.
- `LightNap.DataProviders.SqlServer`: SQL Server data provider implementation including migrations and utilities.
- `LightNap.MaintenanceService`: .NET Core console project to run maintenance tasks.
- `LightNap.WebApi`: .NET Core Web API project.

### Configuring LightNap.WebApi

- **appsettings.json**: Configuration settings for the Web API.

#### DatabaseProvider
- **DatabaseProvider**: The database provider to use. ("Sqlite" or "SqlServer")

#### ConnectionStrings

- **ConnectionStrings.DefaultConnection**: The connection string for the database.

#### ApplicationSettings

- **ApplicationSettings.AutomaticallyApplyEfMigrations**: true to automatically apply EF migrations.
- **ApplicationSettings.RequireTwoFactorForNewUsers**: true to require two-factor authentication for new users.
- **ApplicationSettings.SiteUrlRootForEmails**: The root URL used in emails. (e.g., "<https://localhost:4200/>")
- **ApplicationSettings.UseSameSiteStrictCookies**: true to use SameSite strict cookies. (e.g., false)

#### Administrators

- **Administrators**: List of administrator accounts to seed.
  - **Email**: The email address of the administrator. (e.g., "<admin@admin.com>")
  - **UserName**: The username of the administrator. (e.g., "admin")
  - **Password**: The password of the administrator. (e.g., "A2m!nPassword")

#### Jwt

- **Jwt.Key**: The secret key used for JWT token generation. (e.g., "Any 32+ Character Key (Like A Guid)+BitRequirement")
- **Jwt.Issuer**: The issuer of the JWT token. (e.g., "https://www.yourdomain.com")
- **Jwt.Audience**: The audience of the JWT token. (e.g., "https://www.yourdomain.com")
- **Jwt.ExpirationMinutes**: The expiration time of the JWT token in minutes. (e.g., 120)

#### Smtp

- **Smtp.Host**: The SMTP server host. (e.g., "smtp.sendgrid.net")
- **Smtp.Port**: The SMTP server port. (e.g., 587)
- **Smtp.EnableSsl**: Boolean indicating whether to enable SSL for SMTP. (e.g., true)
- **Smtp.User**: The SMTP user. (e.g., "apikey")
- **Smtp.Password**: The SMTP password or API key. (e.g., "Your SendGrid API Key")
- **Smtp.FromEmail**: The email address used for sending emails. (e.g., "hello@yourdomain.com")
- **Smtp.FromDisplayName**: The display name used for sending emails. (e.g., "LightNap Team")
