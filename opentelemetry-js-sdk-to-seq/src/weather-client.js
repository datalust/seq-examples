const tracer = require('./server-telemetry')('example-http-client'); // must be required first!
const http = require('http');

if (process.argv.length != 3)
{
    console.log("Usage: node src/weather-client.js <POSTCODE>");
    process.exit(1);
}

const postcode = process.argv[2];

tracer.startActiveSpan('Request weather forecast for {postcode}', {
    attributes: {
        postcode
    }
}, (span) => {
    http.get(`http://localhost:8000/${postcode}`, res => {
        const data = [];
    
        res.on('data', chunk => {
            data.push(chunk);
            span.addEvent("Received data");
        });
    
        res.on('end', () => {
            const forecast = Buffer.concat(data).toString();
            console.log(forecast);
            span.end();
        });
    });
    
});




