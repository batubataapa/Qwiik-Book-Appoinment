using System.Reflection;
using AutoMapper;
using BookAppoinment.Adapters;
using BookAppoinment.Adapters.Repositories;
using BookAppoinment.Adapters.Repositories.Interfaces;
using BookAppoinment.Domain;
using MediatR;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BookAppoinment;

public class Startup
{
    private readonly ILogger<Startup> _logger;
    protected IConfiguration Configuration { get; set; }

    // We default this to Development unless explicitly set elsewhere
    public string EnvironmentName { get; protected set; } = Environments.Development;

    public Startup(IConfiguration configuration, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        Configuration = configuration;
        EnvironmentName = env.EnvironmentName;
        _logger = logger;
        _logger.LogInformation("Environment set to {Environment}", EnvironmentName);
        _logger.LogInformation("DOTNET_ENVIRONMENT is {DOTNET_ENVIRONMENT}",
            Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Not set");
        _logger.LogInformation("ASPNETCORE_ENVIRONMENT  is {ASPNETCORE_ENVIRONMENT}",
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Not set");
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor contextAccessor)
    {
        EnvironmentName = env.EnvironmentName;

        // Enable swagger as required
        var isSwaggerEnabled = env.IsDevelopment() || env.EnvironmentName == "Test";
        if (isSwaggerEnabled)
            app.UseDeveloperExceptionPage()
                .UseSwagger()
                .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookAppoinment v1"));

        app.UseHttpsRedirection()
            .UseRouting()
            .UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials())
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints => endpoints.MapControllers());
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public virtual void ConfigureServices(IServiceCollection services)
    {
        _logger.LogInformation("Configuring Services");
        AddApi(services);
        AddAutoMapper(services);
        AddMediatr(services);
        AddPersistence(services);
        AddSwagger(services);
    }

    protected virtual void AddApi(IServiceCollection services)
    {
        _logger.LogInformation("Adding Api Services");
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
    }

    protected virtual void AddAutoMapper(IServiceCollection services)
    {
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
    }

    protected virtual void AddMediatr(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CommandHandler>());
    }

    protected virtual void AddPersistence(IServiceCollection services)
    {
        _logger.LogInformation("Adding Persistent Services");
        services.AddDbContext<QwiikDataContext, MySqlDbContext>();
        services.AddScoped<IAgencyRepository, AgencyRepository<QwiikDataContext>>();
        services.AddScoped<IBookingRepository, BookingRepository<QwiikDataContext>>();
    }

    protected virtual void AddSwagger(IServiceCollection services)
    {
        _logger.LogInformation("Adding Swagger Services");
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Version = "v1",
                Title = "Qwiik Case Work",
                Description = "An ASP.NET Core Web API for Qwiik Case Work Related to Book Appointment"
            });
            options.MapType<DateOnly>(() => new()
            {
                Type = "string",
                Example = new OpenApiString("2021-01-26")
            });
            options.AddSecurityDefinition("Bearer", new()
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new()
            {
                {
                    new()
                    {
                        Reference = new()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            options.EnableAnnotations();
        });
        services.AddSwaggerGenNewtonsoftSupport();
    }
}

