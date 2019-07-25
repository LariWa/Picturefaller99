const express = require('express');
const mysql = require('mysql');
var bodyParser = require("body-parser");
var fs = require('fs');


// Create connection MySQL
const db = mysql.createConnection({
    host     : '141.45.146.52',
    port: '14731',
    user     : 'dreamdiver',
    password : 'hLQnvR3yVaA6ZB56',
    database : 'Dream_Diver'
    
});

// Connect
db.connect((err) => {
    if(err){
        throw err;
    }
    console.log('mysql connected');
});

const app = express();

app.set('view engine', 'ejs');

//install bodyPArser
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

//Zulassung von CORS für den Webbrowser um daten zu schicken
app.use(function(req, response, next) {

    response.header("Access-Control-Allow-Credentials", "true");
    response.header("Access-Control-Allow-Origin", "*");
    response.header("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
    response.header("Access-Control-Allow-Headers", "content-type, Accept, X-Access-Token, X-Application-Name, X-Request-Sent-Time");
  
    next();
});


//CREATE TIME TABLE
app.get('/createTableTime', (req,res) => {
    let sql = 'CREATE TABLE time (' + 
                'id int AUTO_INCREMENT ,' + 
                'time float ,' +
                'sortQuality INT,' +
                'dim INT,' +
                'gameID VARCHAR(255),' +
                'PRIMARY KEY (id))';
    db.query(sql, (err, result) => {
        if (err) throw err;
        console.log(result);
        res.send('time table created.....');
    });
});

// CREATE HIGHSCORE TABLE
app.get('/createTableHighscore', (req,res) => {
    let sql = 'CREATE TABLE highscore (' +
                'id int AUTO_INCREMENT,' +
                'name VARCHAR(255),' + 
                'score INT,' +
                'PRIMARY KEY (id))';
    db.query(sql, (err, result) => {
        if (err) throw err;
        console.log(result);
        res.send('highscore table created.....');
    });
});

//INSERT INTO time POST REQUEST with BodyParser and JSON
app.post('/addtimerihno', (req, res) =>{


    let time = req.body.time;
    let sortQuality = req.body.sortQuality;
    let dim = req.body.dim;
    let gameID = req.body.gameID;
    

    console.log("body", req.body);
    let sql = 'INSERT INTO time SET time= ?, sortQuality = ?, dim = ?, gameID = ? '; // ? is place holder for query(48)
    let query = db.query(sql,[time,sortQuality,dim,gameID],(err, result) =>{
        if (err) console.log(err);
        console.log(result);
        res.send('Added time over POST');
    });
});



// Insert score over POST request
app.post('/addscore', (req, res) =>{
    let name = req.body.name
    let score = req.body.score
    
    console.log("body", req.body)
    let sql = 'INSERT INTO highscore SET name= ?, score = ? '; 
    let query = db.query(sql,[name,score],(err, result) =>{
        if (err) console.log(err);
        console.log(result);
        res.send('Added score over POST');
    });
});


//highscore visualisierung
app.get('/globalhighscore', (req,res)=>{
    var data = fs.readFileSync('score.json');
    var JSONScore = JSON.parse(data);

    let sql = 'SELECT * FROM highscore ORDER BY score DESC LIMIT 10;'
    let query = db.query(sql, (err,row)=> {
        
        var dataJSON = JSON.stringify(row, null , 2); // safe data from DB to JSON
        fs.writeFile('score.json', dataJSON ,finished);

       function finished(err){
           
       }

        res.render('highscore', {
            datenbank: JSONScore,
            highScore: [JSONScore.score],
            message:"Show all scores here",
        });
    });
});


// AUSWERTUNG von den gesammelten Daten und visualiserung unter views/auswertung.ejs
app.get('/auswertung', (req, res) => {
   
    let sql = 'SELECT * FROM time'
    let query = db.query(sql, (err, row) => {
        auswertung(row);
    });


function auswertung(data) {
    var sixDim = [];
    var sevenDim = [];
    var eightDim = [];
    var nineDim = [];
    var tenDim = [];
    var elevenDim = [];
    var twelveDim = [];
    var thirteenDim = [];
    for (let i in data) {
        switch (data[i].dim) {
            case 6: sixDim.push(data[i]);   break;
            case 7: sevenDim.push(data[i]); break;
            case 8: eightDim.push(data[i]); break;
            case 9: nineDim.push(data[i]);  break;
            case 10: tenDim.push(data[i]);  break;
            case 11: elevenDim.push(data[i]); break;
            case 12: twelveDim.push(data[i]); break;
            case 13: thirteenDim.push(data[i]); break;
        }
    }

    var dims = [sixDim, sevenDim, eightDim, nineDim, tenDim, elevenDim, twelveDim, thirteenDim];

    var results = [];//auswertung[dim6, dim7, dim8... dim13]
    var results3 = [];
    for (let i in dims) {
        if (dims[i].length > 0) {
            // console.log(dims[i]);
            var sortedAlg = sortAlgorithmen(dims[i]);
            var pairs = getPairs(sortedAlg[0], sortedAlg[1]);
            var rel = calculateReleation(pairs[0], pairs[1]);
            results[i] = rel;
            console.log("Dimension " + dims[i][0].dim + ": " + rel);
            results3.push(rel);
            console.log("DIMENSIONEN VON ist " + dims[i][0].dim);
            console.log("Kehrwert: " + 1/rel); 
        }
    }
    console.log("ERSTE ARRAY" + results3[0]);
    res.render('auswertung', {
        results3 : results3,     
        dims : dims   
    });
    
}

function sortAlgorithmen(data) {
    var alg0 = [];
    var alg1 = [];
    for (let i in data) {
        if (data[i].sortQuality == 0)
            alg0.push(data[i]);
        else
            alg1.push(data[i]);
    }
    return [alg0, alg1];
}

function getPairs(alg0, alg1) {
    var p1 = [];
    var p2 = [];
    for (var i in alg0) {
        var tempP1 = alg0[i].gameID;
        for (let j = 0; j < alg1.length; j++) {
            if (tempP1 == alg1[j].gameID) {
                p1.push(alg0[i].time);
                p2.push(alg1[j].time);
                if (j - 1 < 0)
                    alg1.shift();
                else
                    alg1.splice(j - 1, 1);
               
                break;
            }
        }
    }
    return [p1, p2];

}

function calculateReleation(p0, p1) {
    console.log(p0);
    console.log(p1);
    if (p0.length > 0 && p1.length > 0) {
        var sum  =0;
        for(let i in p0){
            sum+= p0[i]/p1[i];
        }
        return Math.round((1/p0.length)*sum * 100) / 100;
    }
    else return 0;
}

function calculateRel2(p0, p1) {
    if (p0.length > 0 && p1.length > 0) {
        var rel0 = 0;
        var rel1 = 0;
        for (let i in p0) {
            if (p0[i] != 0 && p1[i] != 0) {
                rel0 += p0[i] / (p0[i] + p1[i]);
                rel1 += p1[i] / (p0[i] + p1[i]);
            }
        }
        return [rel0 / p0.length, rel1 / p1.length]
    }
}
});

app.listen(process.env.PORT || 3000, (req, res) => {

    console.log("Server started on " + 3000);
});

