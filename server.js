/*  Copyright (c) 2013 Raphaël Sfeir
    
    written by : http://raphaelsfeir.fr
    written for : http://phobosproject.com

    Usage : node app.js
*/

var http = require('http'),
	express = require('express');
    io              = require('socket.io'),
    UUID            = require('node-uuid'),

    gameport        = process.env.PORT || 4112;

app = express();


/* Express server set up. */


        //Tell the server to listen for incoming connections
    app.listen( gameport );
    console.log('\t :: Express :: Listening on port ' + gameport );

    app.get( '/', function( req, res ){
        res.sendfile( __dirname + '/index.html' );
    });


        //This handler will listen for requests on /*, any file from the root of our server.
        //See expressjs documentation for more info on routing.

    app.get( '/*' , function( req, res, next ) {

            //This is the current file they have requested
        var file = req.params[0];

            //For debugging, we can track what files are requested.
        console.log('\t :: Express :: file requested : ' + file);

            //Send the requesting client the file.
        res.sendfile( __dirname + '/' + file );

    }); //app.get *