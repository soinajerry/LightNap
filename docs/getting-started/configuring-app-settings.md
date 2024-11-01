---
title: Configuring App Settings
layout: home
parent: Getting Started
nav_order: 200
---

All back-end configuration can be done from the `appsettings.json` in `LightNap.WebApi`.

{: .important}

In a production deployment it is preferable to define these settings in a secure place, like [Azure app service environment variables](https://learn.microsoft.com/en-us/azure/app-service/reference-app-settings).

### Database

- **DatabaseProvider**: The [database provider](../back-end/database-providers) to use.
  - `Sqlite` requires `ConnectionStrings.DefaultConnection` to be set.
  - `SqlServer` requires `ConnectionStrings.DefaultConnection` to be set.
  - `InMemory` uses the in-memory database and does not persist data between sessions.
- **ConnectionStrings.DefaultConnection**: The connection string for the database.

### ApplicationSettings

- **ApplicationSettings.AutomaticallyApplyEfMigrations**: `true` to automatically apply Entity Framework migrations.
  If this is set to false, EF migrations must be manually applied.
- **ApplicationSettings.LogOutInactiveDeviceDays**: The number of days of inactivity (no contact) before a device is logged out.
- **ApplicationSettings.RequireTwoFactorForNewUsers**: `true` to require two-factor email authentication for new users.
  It does not change existing users.
- **ApplicationSettings.SiteUrlRootForEmails**: The root URL used in emails. This should be the base URL where the Angular app
  is hosted from (e.g., `https://localhost:4200/` in development).
- **ApplicationSettings.UseSameSiteStrictCookies**: `true` to use SameSite strict cookies. Set this to `false` if debugging the
  front-end from a different URL root (like `http://localhost:4200/` for Angular).

### Administrators

If this section is configured then the specified accounts will be seeded with the provided credentials.

- **Administrators**: List of administrator accounts to seed.
  - **Email**: The email address of the administrator (e.g., `admin@admin.com`).
  - **UserName**: The username of the administrator (e.g., `admin`).
  - **Password**: The password of the administrator (e.g., `A2m!nPassword`). If this field is blank or missing then a random password
  will be generated and the user will need to reset their password via the site.

### Jwt

This section defines the configuration of the JWT tokens created for managing authentication. It will work out of the box as configured,
but it's critical that the `Key` value be changed for at least production environments.

- **Jwt.Key**: The secret key used for JWT token generation (e.g., "Any 32+ Character Key (Like A Guid)"). It should be different
  across different environments (development vs. production and so on).
- **Jwt.Issuer**: The issuer of the JWT token (e.g., `https://www.yourdomain.com`).
- **Jwt.Audience**: The audience of the JWT token (e.g., `https://www.yourdomain.com`).
- **Jwt.ExpirationMinutes**: The expiration time of the JWT token in minutes (e.g., `120`).

### Email

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
