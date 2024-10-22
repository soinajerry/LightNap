# SQL Server Data Provider

The `LightNap.DataProviders.SqlServer` project contains the migrations for a SQL Server database. This project should never need to be manually edited as the migrations are handled by the `dotnet ef migrations` commands.

## Configuration

To use SQL Server:

- Set the `DatabaseProvider` setting to `SqlServer`.
- Set the `DefaultConnection` connections string.

For example, in `LightNap.WebApi` this can be added in `Program.cs`:

```csharp
builder.Services.AddLightNapSqlServer(builder.Configuration);
```

## Migrations

Use the Entity Framework Core tools to create and apply migrations. Entities are defined in the `LightNap.Core` project.

To add a migration, use the following command from the `/src` folder:

```bash
dotnet ef migrations add <MigrationName> --context ApplicationDbContext --project LightNap.DataProviders.SqlServer --startup-project LightNap.WebApi
```

Similarly, you can remove the most recent migration using the following command:

```bash
dotnet ef migrations remove --project LightNap.DataProviders.SqlServer --startup-project LightNap.WebApi
```

To apply changes to the database, use the following command:

```bash
dotnet ef database update --project LightNap.DataProviders.SqlServer --startup-project LightNap.WebApi
```

Note that the `LightNap.WebApi` project also offers automatic migrations by setting the
`SiteSettings.AutomaticallyApplyEfMigrations` to true.

### Pre-Initial Deployment Changes

Prior to deploying a database you may want to regenerate the entire schema to include your initial changes as a single InitialCreate migration.

To do this, use the following commands:

```bash
dotnet ef database drop --project LightNap.DataProviders.SqlServer --startup-project LightNap.WebApi
dotnet ef migrations remove --project LightNap.DataProviders.SqlServer --startup-project LightNap.WebApi
dotnet ef migrations add InitialCreate --context ApplicationDbContext --project LightNap.DataProviders.SqlServer --startup-project LightNap.WebApi
dotnet ef database update --project LightNap.DataProviders.SqlServer --startup-project LightNap.WebApi
```

Note that the `drop` command above will request confirmation to avoid inadvertently dropping the wrong database.