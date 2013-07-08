function loadGameCore() {
	console.log(":: Welcome ! ::");
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
	require(gameAssetsDir + "Collectable.js");
	require(gameAssetsDir + "Station.js");
	require(gameAssetsDir + "Cooldown.js");
	require(gameAssetsDir + "Weapon.js");
	require(gameAssetsDir + "Tile.js");
}
function loadGameUtils() {
	console.log("::Loading game Utils... ::");
	var gameUtilsDir = "./" + coreDir + dirSep + utilsDir + dirSep;
	require(gameUtilsDir + "Utils.js");
}
function upload(response, postData) {
  console.log("Le gestionnaire 'upload' est appelé.");
  response.writeHead(200, {"Content-Type": "text/plain"});
  response.write("Vous avez envoyé : " + querystring.parse(postData).text);
  response.end();
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
	firebaseRecover = false;
	
httpServer = http.createServer(function(request, response) {
	response.write('Phobos server launched');
	response.end();
}); 

io = require('socket.io').listen(httpServer);

httpServer.listen(gameport); 
loadGameCore(); 

require("./phobos.server.js");
require("./firebase.server.js");
