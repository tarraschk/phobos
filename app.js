
function upload(response, postData) {
	console.log("Le gestionnaire 'upload' est appelé.");
	response.writeHead(200, {"Content-Type": "text/plain"});
	response.write("Vous avez envoyé : " + querystring.parse(postData).text);
response.end();
}
var gameport = 8080,
	phobos = {};
	server = true ; 
	firebaseRecover = false;
	http = require('http'),
	gameLoader = require('./loader.js'),
	
httpServer = http.createServer(function(request, response) {
	response.write('Phobos server launched');
	response.end();
});

io = require('socket.io').listen(httpServer);

httpServer.listen(gameport); 
gameLoader.loadCore();

require("./phobos.server.js");
require("./firebase.server.js");
