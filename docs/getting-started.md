---
title: "Getting Started"
description: "A guide to help you get started with the project."
layout: home
tags: ["setup", "installation"]
nav_order: 2
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

2. **Back-End Setup:**

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

    {: .important }
    The application will log the error and quit if anything in the startup or seeding process fails. This
    includes database migrations and user/role seeding. Please check the logs if a deployment fails to start.

3. **Front-End Setup:**

   - In a separate terminal, navigate to the `lightnap-ng` directory:

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
  - **Email**: `admin@admin.com`
  - **UserName**: `admin`
  - **Password**: `A2m!nPassword`

## Project Structure

Back-End

- `LightNap.Core`: .NET shared library for common server-side components.
- `LightNap.Core.Tests`: Test library for `LightNap.Core`.
- `LightNap.DataProviders.Sqlite`: SQLite data provider implementation including migrations and utilities.
- `LightNap.DataProviders.SqlServer`: SQL Server data provider implementation including migrations and utilities.
- `LightNap.MaintenanceService`: .NET Core console project to run maintenance tasks.
- `LightNap.WebApi`: .NET Core Web API project.

Front-End

- `lightnap-ng`: Angular project with PrimeNG components based on the [sakai-ng](https://github.com/primefaces/sakai-ng) template.

### Configuring The Back-End

All back-end configuration can be done from the `appsettings.json` in `LightNap.WebApi`.

#### Database

- **DatabaseProvider**: The database provider to use.
  - `Sqlite` requires `ConnectionStrings.DefaultConnection` to be set.
  - `SqlServer` requires `ConnectionStrings.DefaultConnection` to be set.
  - `InMemory` uses the in-memory database and does not persist data between sessions.
- **ConnectionStrings.DefaultConnection**: The connection string for the database.

#### ApplicationSettings

- **ApplicationSettings.AutomaticallyApplyEfMigrations**: `true` to automatically apply Entity Framework migrations.
  If this is set to false, EF migrations must be manually applied.
- **ApplicationSettings.LogOutInactiveDeviceDays**: The number of days of inactivity (no contact) before a device is logged out.
- **ApplicationSettings.RequireTwoFactorForNewUsers**: `true` to require two-factor email authentication for new users.
  It does not change existing users.
- **ApplicationSettings.SiteUrlRootForEmails**: The root URL used in emails. This should be the base URL where the Angular app
  is hosted from (e.g., `https://localhost:4200/` in development).
- **ApplicationSettings.UseSameSiteStrictCookies**: `true` to use SameSite strict cookies. Set this to `false` if debugging the
  front-end from a different URL root (like `http://localhost:4200/` for Angular).

#### Administrators

If this section is configured then the specified accounts will be seeded with the provided credentials.

- **Administrators**: List of administrator accounts to seed.
  - **Email**: The email address of the administrator (e.g., `admin@admin.com`).
  - **UserName**: The username of the administrator (e.g., `admin`).
  - **Password**: The password of the administrator (e.g., `A2m!nPassword`). If this field is blank or missing then a random password
  will be generated and the user will need to reset their password via the site.

#### Jwt

This section defines the configuration of the JWT tokens created for managing authentication. It will work out of the box as configured,
but it's critical that the `Key` value be changed for at least production environments.

- **Jwt.Key**: The secret key used for JWT token generation (e.g., "Any 32+ Character Key (Like A Guid)"). It should be different
  across different environments (development vs. production and so on).
- **Jwt.Issuer**: The issuer of the JWT token (e.g., `https://www.yourdomain.com`).
- **Jwt.Audience**: The audience of the JWT token (e.g., `https://www.yourdomain.com`).
- **Jwt.ExpirationMinutes**: The expiration time of the JWT token in minutes (e.g., `120`).

#### Email

- **EmailProvider**: The email provider to use.
  - `LogToConsole` logs messages to the console and is best suited for development.
  - `Smtp` requires the `Smtp` settings.
    - **Smtp.Host**: The SMTP server host. (e.g., "smtp.sendgrid.net")
    - **Smtp.Port**: The SMTP server port. (e.g., 587)
    - **Smtp.EnableSsl**: Boolean indicating whether to enable SSL for SMTP. (e.g., true)
    - **Smtp.User**: The SMTP user. (e.g., "apikey")
    - **Smtp.Password**: The SMTP password or API key. (e.g., "Your SendGrid API Key")
    - **Smtp.FromEmail**: The email address used for sending emails. (e.g., `hello@yourdomain.com`)
    - **Smtp.FromDisplayName**: The display name used for sending emails. (e.g., "LightNap Team")
