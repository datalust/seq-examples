const tracer = require('./server-telemetry')('example-http-server'); // must be required first!
const http = require('http');
const fs = require('fs');
const path = require('path');
const url = require('url');

const host = 'localhost';
const port = 8000;

const server = http.createServer((req, res) => {
    tracer.startActiveSpan('Creating forecast for {postcode} - {forecast}', (span) => {
        const data = readData();
        const postcode = url.parse(req.url, true).path.slice(1);
        span.setAttribute("postcode", postcode);
        const forecast = data[postcode];
        span.setAttribute("forecast", forecast);
        res.writeHead(200);
        res.end(forecast ?? `Unknown postcode ${postcode}`);
        span.end(); // without this it doesn't work
    });
});

server.listen(port, host, () => {
    console.log(`Server is running on http://${host}:${port}`);
});

function readData() {
    // must be `startActiveSpan` (not `startSpan`) to set parent span of child spans (trace context)
    return tracer.startActiveSpan('readData', (span) => {

        const paths = fs.readdirSync(path.join(__dirname, '..', 'data'));
        const data = Object.fromEntries(
            paths.map((p) => [
                p.split('.')[0], 
                fs.readFileSync(path.join(__dirname, '..', 'data', p), 'utf8')
            ])
        );
        span.addEvent("Found {count} postcodes", {
            count: Object.keys(data).length,
        });
        span.end();
        return data;
    });
}