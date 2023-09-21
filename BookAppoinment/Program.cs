using Destructurama;
using LanguageExt;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Json;

namespace BookAppoinment;

public static class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(new JsonFormatter())
            .CreateBootstrapLogger();

        Log.Information("Starting up!");

        try
        {
            CreateHostBuilder(args).Build().Run();
            Log.Information("Stopped cleanly");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var hostEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";

        var logger = new SerilogLoggerFactory()
            .CreateLogger<Startup>();

        return Host.CreateDefaultBuilder(args)
            .UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()
                .Destructure.UsingAttributes()
                .Destructure.ByTransformingWhere<object>(t => t.GetInterfaces().Contains(typeof(IEither)), c => (c as IEither)!.MatchUntyped(
                    obj => new { IsRight = true, Value = obj },
                    obj => new { IsRight = false, Value = obj }))
                .Enrich.With(new LogMaskingEnricher())
                .WriteTo.Console(new JsonFormatter()))
            .ConfigureWebHostDefaults(webBuilder => webBuilder
                .UseStartup(ctx => new Startup(ctx.Configuration, ctx.HostingEnvironment, logger)));
    }
}

public class LogMaskingEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        try
        {
            // Masking Http Logging Request & Response Body, because can't masked in Endpoint Class
            if (logEvent.Properties.Keys.Contains("Body"))
            {
                var json = JsonConvert.DeserializeObject<string>(logEvent.Properties["Body"].ToString());
                if (json != "")
                {
                    var body = JObject.Parse(json!);
                    var mask = "*******";

                    // masking request data
                    if (body.ContainsKey("password")) body["password"] = mask;
                    if (body.ContainsKey("accessToken")) body["accessToken"] = mask;
                    if (body.ContainsKey("refreshToken")) body["password"] = mask;

                    // masking response data
                    if (body.ContainsKey("data") && body["data"]!.GetType() == typeof(JObject))
                    {
                        if (body["data"]!["token"] != null) body["data"]!["token"] = mask;
                        if (body["data"]!["refreshToken"] != null) body["data"]!["refreshToken"] = mask;
                        if (body["data"]!["refresh"] != null) body["data"]!["refreshToken"] = mask;
                    }
                    logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Body", JsonConvert.SerializeObject(body)));
                }
            }
        }
        catch (Exception ex)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("LogMaskingEnricherError", ex));
        }
    }
}