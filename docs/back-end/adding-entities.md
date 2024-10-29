---
title: "Adding Entities"
description: "A guide for extending the data model."
layout: home
parent: Back-End
nav_order: 3
---

LightNap makes use of Entity Framework (EF) for database access. The `ApplicationDbContext` and supporting entities are all kept in
the `Data` folder of the `LightNap.Core` project.

## Extending EF Models

Follow the common EF processes to ensure the changes are correctly integrated:

1. **Add New Entity Classes**: Create new classes in the `Data` folder that represent the new entities.
2. **Update DbContext**: Add `DbSet<TEntity>` properties to the `ApplicationDbContext` class for each new entity.
3. **Migrations**: Use EF Core migrations to update the database schema. Run `dotnet ef migrations add <MigrationName>` and `dotnet ef database update` to apply changes.
4. **Configure Relationships**: Define relationships and constraints using Fluent API or data annotations in your entity classes.

By following these steps, you can effectively extend the data model to meet your application's requirements.
