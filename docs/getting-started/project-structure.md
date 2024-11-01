---
title: Understanding the Project Structure
layout: home
parent: Getting Started
nav_order: 300
---

Back-End

- `LightNap.Core`: .NET shared library for common server-side components.
- `LightNap.Core.Tests`: Test library for `LightNap.Core`.
- `LightNap.DataProviders.Sqlite`: SQLite data provider implementation including migrations and utilities.
- `LightNap.DataProviders.SqlServer`: SQL Server data provider implementation including migrations and utilities.
- `LightNap.MaintenanceService`: .NET Core console project to run maintenance tasks.
- `LightNap.WebApi`: .NET Core Web API project.

Front-End

- `lightnap-ng`: Angular project with PrimeNG components based on the [sakai-ng](https://github.com/primefaces/sakai-ng) template.
