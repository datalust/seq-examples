using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

ActivitySource WeatherClientActivitySource = new("WeatherClient");

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource("WeatherClient")
    .ConfigureResource(r => r.AddService("weather"))
    .AddOtlpExporter(opt =>
    {
        opt.Endpoint = new Uri("http://localhost:5341/ingest/otlp/v1/traces");
        opt.Protocol = OtlpExportProtocol.HttpProtobuf;
        //opt.Headers = "X-Seq-ApiKey=abcde12345";
    })
    .AddConsoleExporter()
    .AddHttpClientInstrumentation()
    .Build();

if (args.Length != 1)
{
    Console.WriteLine("Usage: weather <POSTCODE>");
    return 1;
}

var postcode = args[0];

using var activity = WeatherClientActivitySource.StartActivity("Request weather for postcode {Postcode}");
activity?.SetTag("Postcode", postcode);

try
{
    var weatherClient = new HttpClient { BaseAddress = new("http://localhost:5133") };
    var forecast = await weatherClient.GetStringAsync(postcode);
    Console.WriteLine(forecast);

    return 0;
}
catch (Exception ex)
{
    //activity.Complete(LogEventLevel.Fatal, ex);
    return 1;
}

