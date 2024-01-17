import logging
from opentelemetry import trace
from opentelemetry.exporter.otlp.proto.http.trace_exporter import OTLPSpanExporter
from opentelemetry.exporter.otlp.proto.http._log_exporter import OTLPLogExporter
from opentelemetry._logs import set_logger_provider
from opentelemetry.sdk._logs import LoggerProvider, LoggingHandler
from opentelemetry.sdk._logs.export import BatchLogRecordProcessor
from opentelemetry.sdk.trace import TracerProvider
from opentelemetry.sdk.trace.export import BatchSpanProcessor
from opentelemetry.sdk.resources import SERVICE_NAME, Resource

# Service name is required for most backends
resource = Resource(attributes={
    SERVICE_NAME: "weather"
})

# configure logging
logger_provider = LoggerProvider(resource=resource)
set_logger_provider(logger_provider)
logger_provider.add_log_record_processor(BatchLogRecordProcessor(OTLPLogExporter(endpoint="http://localhost:5341/ingest/otlp/v1/logs")))
handler = LoggingHandler(level=logging.NOTSET, logger_provider=logger_provider)
logging.getLogger().addHandler(handler)

# configure tracing
traceProvider = TracerProvider(resource=resource)
processor = BatchSpanProcessor(OTLPSpanExporter(endpoint="http://localhost:5341/ingest/otlp/v1/traces"))
traceProvider.add_span_processor(processor)
trace.set_tracer_provider(traceProvider)

tracer = trace.get_tracer("weather.tracer")

logger = logging.getLogger(__name__)

with tracer.start_as_current_span("this is a span") as span:
    logger.warning("The weather forecast is %s", "Overcast, 24°C")
    span.set_attribute("parent-attribute", 5)
    with tracer.start_as_current_span("child span") as cspan:
        cspan.set_attribute("child-attribute", 42)

logger_provider.shutdown()
traceProvider.shutdown()
