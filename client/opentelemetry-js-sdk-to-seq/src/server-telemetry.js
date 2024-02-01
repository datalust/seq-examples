const opentelemetry = require('@opentelemetry/api');
const { NodeTracerProvider } = require('@opentelemetry/sdk-trace-node');
const { ConsoleSpanExporter, SimpleSpanProcessor } = require('@opentelemetry/sdk-trace-base');
const { registerInstrumentations } = require('@opentelemetry/instrumentation');
const { Resource } = require('@opentelemetry/resources');
const { SemanticResourceAttributes } = require('@opentelemetry/semantic-conventions');
const { HttpInstrumentation } = require('@opentelemetry/instrumentation-http');
const { getNodeAutoInstrumentations } = require('@opentelemetry/auto-instrumentations-node');
const { OTLPTraceExporter } = require('@opentelemetry/exporter-trace-otlp-proto');

module.exports = (serviceName) => {
  const provider = new NodeTracerProvider({
    resource: new Resource({
      [SemanticResourceAttributes.SERVICE_NAME]: serviceName,
    }),
  });

  const otlpExporter = new OTLPTraceExporter({
    url: 'http://localhost:5341/ingest/otlp/v1/traces',
    // headers: {
    //   "X-Seq-ApiKey": "abcde12345"
    // },
  });

  provider.addSpanProcessor(new SimpleSpanProcessor(new ConsoleSpanExporter()));
  provider.addSpanProcessor(new SimpleSpanProcessor(otlpExporter));
  provider.register();

  registerInstrumentations({
    instrumentations: [
      new HttpInstrumentation(),
      getNodeAutoInstrumentations(),
    ],
  });

  return opentelemetry.trace.getTracer('weather-example-tracer');
};
