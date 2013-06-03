function loadGameCore() {
	console.log("::Loading game Core... ::");
	loadGameAssets();
	loadGameUtils();
}
function loadGameAssets() {
	console.log("::Loading game Assets... ::");
	var gameAssetsDir = "./" + coreDir + dirSep + assetsDir + dirSep;
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
	assetsDir = "assets";

httpServer = http.createServer(function(req, res) {
	res.end("HelloWorld") ; 
}); 

var io = require('socket.io').listen(httpServer);

httpServer.listen(gameport); 
loadGameCore();


utils = new phobos.Utils();
game = new phobos.Game();

io.sockets.on('connection', function(socket) {

	socket.on('playerData', function (playerData) {
	})

	socket.on('loadPlayers', function() {
	});
	socket.on('playerLogin', function(user) {
	}); 
	
	socket.on('tickPlayer', function(user) {
	}); 
	
}); 