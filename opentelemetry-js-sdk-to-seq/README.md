OpenTelemetry JavaScript SDK to Seq Demo
========================================

> NOTE: The OpenTelemetry JavaScript SDK does not work with Node EcmaScript modules. You must use the old style CommonJS modules.

A simple client-server application that demonstrates using the [OpenTelemetry JavaScript SDK](https://github.com/open-telemetry/opentelemetry-js) to send logs and traces to [Seq](https://datalust.co/seq).

Original example from https://github.com/serilog-tracing/serilog-tracing/tree/dev/example.

Usage
=====

Start the server:

```
> node src/weather-server.js
```

Then run the client:

```
> node src/weather-client.js 4061
```

