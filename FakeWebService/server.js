const express = require('express');
const fs = require('fs');
const https = require('https');
const dotenv = require('dotenv');

dotenv.config();

const app = express();
const port = process.env.PORT || 3000;

// Middleware to parse JSON bodies
app.use(express.json());

// Basic route
app.get('/', (req, res) => {
    res.send('Hello World: ' + new Date().toISOString());
});


const options = {
    key: fs.readFileSync(process.env.KEY_PEM),
    cert: fs.readFileSync(process.env.CERT_PEM)
};


// Start the server
https.createServer(options, app).listen(port, () => {
    console.log(`HTTPS Server is running on https://localhost:${port}`);
});
