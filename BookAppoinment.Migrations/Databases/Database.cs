using System;
using FluentMigrator.Runner;

namespace BookAppoinment.Migrations.Databases;

public class Database
{
    public static void RunMigrations(string mode, long version)
    {
        var serviceProvider = CreateServices();

        // Put the database update into a scope to ensure
        // that all resources will be disposed.
        using var scope = serviceProvider.CreateScope();
        RunMigrations(scope.ServiceProvider, mode, version);
    }

    /// <summary>
    /// Configure the dependency injection services
    /// </summary>
    private static IServiceProvider CreateServices()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        var configuration = builder.Build();

        return new ServiceCollection()
            // Add common FluentMigrator services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                // Add SqlServer support to FluentMigrator
                .AddMySql5()
                // Set the connection string
                .WithGlobalConnectionString(configuration.GetSection("ConnectionStrings").GetValue<string>("ApiDatabase"))
                // Define the assembly containing the migrations
                .ScanIn(typeof(Database).Assembly).For.Migrations().For.EmbeddedResources())
            // Enable logging to console in the FluentMigrator way
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            // Build the service provider
            .BuildServiceProvider(false);
    }

    /// <summary>
    /// Update the database
    /// </summary>
    private static void RunMigrations(IServiceProvider serviceProvider, string mode, long version)
    {
        // Instantiate the runner
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        // Execute the migrations
        switch (mode)
        {
            case "up":
                runner.MigrateUp();
                break;
            case "down":
                runner.MigrateDown(version);
                break;
        }

    }
}