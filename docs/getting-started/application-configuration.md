---
title: Application Configuration
layout: home
parent: Getting Started
nav_order: 200
---

All back-end configuration can be done from the `appsettings.json` in `LightNap.WebApi`.

{: .important}

In a production deployment it is preferable to define these settings in a secure place, like [Azure app service environment variables](https://learn.microsoft.com/en-us/azure/app-service/reference-app-settings).

## Database

See the options for configuring the [database provider](./database-providers) to use.

## Email

See the options for configuring the [email provider](./email-providers) to use.

## JWT

See the options for configuring the [JSON web tokens](./email-providers) used for site authentication.

## ApplicationSettings

See the options for configuring [application settings](./configuring-application-settings).

## Administrators

If this section is configured then the specified accounts will be seeded with the provided credentials.

- **Administrators**: List of administrator accounts to seed.
  - **Email**: The email address of the administrator (e.g., `admin@admin.com`).
  - **UserName**: The username of the administrator (e.g., `admin`).
  - **Password**: The password of the administrator (e.g., `A2m!nPassword`). If this field is blank or missing then a random password
  will be generated and the user will need to reset their password via the site.
