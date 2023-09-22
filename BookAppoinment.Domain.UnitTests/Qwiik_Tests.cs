using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit.Abstractions;
using BookAppoinment.Adapters;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using BookAppoinment.Adapters.Repositories;
using BookAppoinment.Adapters.Repositories.Interfaces;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace BookAppoinment.Domain.UnitTests;

public class Qwiik_Tests
{
    protected IServiceScope _scope;
    protected QwiikDataContext _dbContext;
    protected IMapper _mapper;


    protected Qwiik_Tests(ITestOutputHelper output)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        var databaseName = Guid.NewGuid().ToString();
        var config = new ConfigurationBuilder().AddInMemoryCollection(
            new Dictionary<string, string>
            {
                { "ApiDatabase", databaseName }
            }
        ).Build();

        var services = ConfigureServices(new ServiceCollection(), config);
        var provider = services.BuildServiceProvider();
        _scope = provider.CreateScope();

        _dbContext = _scope.ServiceProvider.GetRequiredService<QwiikDataContext>();
        _mapper = _scope.ServiceProvider.GetRequiredService<IMapper>();
    }

    public static IServiceCollection ConfigureServices(IServiceCollection services, IConfiguration config)
    {
        services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = true,
                        OverrideSpecifiedNames = true
                    }
                };
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
        services.AddHttpContextAccessor();

        services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All;
        });

        foreach (var file in Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "BookAppoinment*.dll", SearchOption.AllDirectories))
        {
            string[] checkMapper = { "BookAppoinment.Domain.dll", "BookAppoinment.dll" };
            if (checkMapper.Any(file.Contains))
            {
                var asm = Assembly.Load(Path.GetFileNameWithoutExtension(file));
                var types = asm.GetTypes();
                if (types.Any(t => t.IsSubclassOf(typeof(Profile)) && !t.IsAbstract))
                {
                    services.AddAutoMapper(asm);
                }
            }
        }

        services.AddSingleton(config);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CommandHandler>());

        services.AddDbContext<QwiikDataContext, InMemoryDataContext>();
        services.AddScoped<IAgencyRepository, AgencyRepository<QwiikDataContext>>();
        services.AddScoped<IBookingRepository, BookingRepository<QwiikDataContext>>();

        services.AddFluentMigratorCore()
            .ConfigureRunner(r => r.AddSQLite()
                .WithGlobalConnectionString(config["ApiDatabase"])
                .ScanIn(typeof(Migrations.Databases.Database).Assembly).For.Migrations());

        return services;

    }
}
