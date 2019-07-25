var express = require('express');
var app = express();

// headers für die requests
app.use(function(req, response, next) {

    response.header("Access-Control-Allow-Credentials", "true");
    response.header("Access-Control-Allow-Origin", "*");
    response.header("Access-Control-Allow-Methods", "GET, OPTIONS");
    response.header("Access-Control-Allow-Headers","Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
  
    next();
});

// deklaration dass es ein static server wird und von wo er die dateien abrufen soll
app.use(express.static(__dirname + '/bilder'));

// process.env.PORT ist für das deployen auf Heroku bzw auf anderen platformen gedacht sonst  port 8000
var port = process.env.PORT || 8000
    
// server wird hier gestartet
app.listen(port, (req, res) => {
    console.log("Static server has been started");
    
 } );