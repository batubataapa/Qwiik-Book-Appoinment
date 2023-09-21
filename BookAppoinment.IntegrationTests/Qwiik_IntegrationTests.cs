using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using BookAppoinment;
using LanguageExt;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Xunit.Abstractions;

namespace BookAppoinment.IntegrationTests;

public class Qwiik_IntegrationTests : IDisposable
{
    protected readonly ITestOutputHelper _output;
    protected readonly HttpClient _sut;
    protected JsonSerializerSettings _serializerSettings;
    protected IServiceProvider Scope { get; set; }

    public Qwiik_IntegrationTests(ITestOutputHelper output)
    {
        _output = output;
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        var jsonOptions = new JsonSerializerOptions();
        jsonOptions.Converters.Add(new JsonStringEnumConverter());
        jsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        jsonOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

        _serializerSettings = new JsonSerializerSettings();
        _serializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        _serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        _serializerSettings.NullValueHandling = NullValueHandling.Ignore;

        var testServer = new TestServer(new WebHostBuilder()
            .UseEnvironment("Test")
            .UseSerilog((_, config) => config
                .MinimumLevel.Verbose()
                .Destructure.ByTransformingWhere<object>(t => t.GetInterfaces().Contains(typeof(IEither)), c => (c as IEither)!.MatchUntyped(
                    obj => new { IsRight = true, Value = obj },
                    obj => new { IsRight = false, Value = obj }))
                .Enrich.WithCorrelationId()
                .WriteTo.TestOutput(output))
            .UseStartup<TestStartup>());
        _sut = testServer.CreateClient();

        Scope = testServer.Host.Services;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _sut.Dispose();
        }
    }

}

public class TestStartup : Startup
{
    public TestStartup(IConfiguration configuration, IWebHostEnvironment env, ILogger<TestStartup> logger) : base(configuration, env, logger)
    {
        Configuration = new ConfigurationBuilder()
            .AddConfiguration(Configuration)
            .AddInMemoryCollection(
                new Dictionary<string, string>
                {
                })
            .Build();
    }

    public override void ConfigureServices(IServiceCollection services)
    {
        // Start with the base registrations
        base.ConfigureServices(services);

        services.AddSingleton(sp => sp);

        services.AddSingleton(Configuration);
        services.AddHttpContextAccessor();
    }

    protected override void AddApi(IServiceCollection services)
    {
        base.AddApi(services);

        services.AddControllers().AddApplicationPart(typeof(Startup).Assembly);
    }
}
