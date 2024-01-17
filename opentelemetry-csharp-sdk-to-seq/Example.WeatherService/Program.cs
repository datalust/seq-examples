using System.Diagnostics;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

ActivitySource ExampleWeatherServiceActivitySource = new("Example Weather Service");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddOpenTelemetry()
        .ConfigureResource(resource => resource.AddService(serviceName: builder.Environment.ApplicationName))
        .WithTracing(tracing =>
        {
            tracing.AddSource("Example Weather Service");
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddConsoleExporter();
            tracing.AddOtlpExporter(opt =>
            {
                opt.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/traces");
                opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                //opt.Headers = "X-Seq-ApiKey=abcde12345";
            });
        });

    builder.Services.AddLogging(logging => logging.AddOpenTelemetry(openTelemetryLoggerOptions =>  
    {  
        openTelemetryLoggerOptions.SetResourceBuilder(  
            ResourceBuilder.CreateEmpty()
                .AddService(serviceName: builder.Environment.ApplicationName)  
                .AddAttributes(new Dictionary<string, object>  
                {
                    // Add any desired resource attributes here
                    ["deployment.environment"] = "development"  
                }));
  
        // Some important options to improve data quality
        openTelemetryLoggerOptions.IncludeScopes = true;
        openTelemetryLoggerOptions.IncludeFormattedMessage = true;

        openTelemetryLoggerOptions.AddConsoleExporter();
        openTelemetryLoggerOptions.AddOtlpExporter(opt =>
        {
            opt.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/logs");
            opt.Protocol = OtlpExportProtocol.HttpProtobuf;
            //opt.Headers = "X-Seq-ApiKey=abcde12345";
        });
    }));

    var app = builder.Build();
    app.Logger.LogInformation("Starting {App}", builder.Environment.ApplicationName);

    var forecastByPostcode = Directory.GetFiles("./data")
        .ToDictionary(f => Path.GetFileNameWithoutExtension(f)!, f => File.ReadAllText(f).Trim());
    
    app.MapGet("/{postcode}", (string postcode) =>
    {
        using var activity = ExampleWeatherServiceActivitySource.StartActivity("Look up forecast for postcode {Postcode}");
        activity?.SetTag("Postcode", postcode);
        var forecast = forecastByPostcode[postcode];
        activity?.SetTag("Forecast", forecast);
        
        return forecast;
    });

    app.Run();

    return 0;
}
catch (Exception)
{
    return 1;
}

