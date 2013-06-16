function loadGameCore() {
	console.log("::Loading game Core... ::");
	loadGameAssets();
	loadGameUtils();
}
function loadGameAssets() {
	console.log("::Loading game Assets... ::");
	var gameAssetsDir = "./" + coreDir + dirSep + assetsDir + dirSep;
	var serverAssetsDir = "./" + serverDir + dirSep ;
	require(serverAssetsDir + "Server.js");
	require(gameAssetsDir + "Game.js");
	require(gameAssetsDir + "Ship.js");
	require(gameAssetsDir + "Bot.js");
	require(gameAssetsDir + "Station.js");
	require(gameAssetsDir + "Cooldown.js");
	require(gameAssetsDir + "Weapon.js");
}
function loadGameUtils() {
	console.log("::Loading game Utils... ::");
	var gameUtilsDir = "./" + coreDir + dirSep + utilsDir + dirSep;
	require(gameUtilsDir + "Utils.js");
}

var http = require('http'),
	gameport = 8080,
	coreDir = "core",
	dirSep = "/",
	utilsDir = "utils",
	serverDir = "server",
	assetsDir = "assets";
	phobos = {};
	server = true ; 
	
httpServer = http.createServer(function(request, response) {
  response.writeHead(200, {"Content-Type": "text/plain"});
  // write some content to the browser that your user will see
  response.write('Phobos server launched');

  // close the response
  response.end();
}); 

var io = require('socket.io').listen(httpServer);

httpServer.listen(gameport); 
loadGameCore();

require("./phobos.server.js");

io.sockets.on('connection', function(client) {
	client.on('login', function(loginData){
		var loggedInShipData = server.playerLogin({client: this,loginData: loginData}); 

		if (loggedInShipData) {	
			client.emit('loggedIn', loggedInShipData);
  			client.broadcast.emit('newPlayerLoggedIn', loggedInShipData);
		}
	});

	client.on('playerMove', function(data){
		server.playerMove(data.player, data);
  		client.broadcast.emit('playerMove', data);
	});
	client.on('playerData', function (playerData) {

	});

	client.on('loadPlayers', function() {
		server.loadSectorPlayers(this); 
	});
	client.on('playerLogin', function(user) {

	}); 
	
	client.on('tickPlayer', function(user) {
	}); 
	
}); 
