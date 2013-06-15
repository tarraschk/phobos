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
	
httpServer = http.createServer(function(req, res) {
	res.end("HelloWorld") ; 
}); 

var io = require('socket.io').listen(httpServer);

httpServer.listen(gameport); 
loadGameCore();

require("./phobos.server.js");

io.sockets.on('connection', function(socket) {
	socket.on('login', function(loginData){
		var loggedInShipData = server.playerLogin({socket: this,loginData: loginData}); 

		if (loggedInShipData) {	
			socket.emit('loggedIn', loggedInShipData);
  			socket.broadcast.emit('newPlayerLoggedIn', loggedInShipData);
		}
	});

	socket.on('playerMove', function(data){
		server.playerMove(data.player, data);
  		socket.broadcast.emit('playerMove', data);
	});
	socket.on('playerData', function (playerData) {

	});

	socket.on('loadPlayers', function() {
		server.loadSectorPlayers(this); 
	});
	socket.on('playerLogin', function(user) {

	}); 
	
	socket.on('tickPlayer', function(user) {
	}); 
	
}); 
