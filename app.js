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
	gameport = 4112,
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
	socket.on('connect', function(data){
		server.playerJoin({
			socket: this,
			name: data.name
		});
	});

	socket.on('move', function(data){
		console.log("Move player"); 
		console.log(data); 
		server.playerMove(data);
	});
	socket.on('playerData', function (playerData) {

	});

	socket.on('loadPlayers', function() {
	});
	socket.on('playerLogin', function(user) {

	}); 
	
	socket.on('tickPlayer', function(user) {
	}); 
	
}); 
