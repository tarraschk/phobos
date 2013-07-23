function loadCore() {
	var coreDir = "core",
	dirSep = "/",
	utilsDir = "utils";
	serverDir = "server",
	assetsDir = "assets";
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
		require(serverAssetsDir + "UniverseGenerator.js");
		require(serverAssetsDir + "DBHandler.js");
		require(gameAssetsDir + "Game.js");
		require(gameAssetsDir + "SpaceObject.js");
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
	 console.log("Le gestionnaire 'upload' est appel��.");
	 response.writeHead(200, {"Content-Type": "text/plain"});
	 response.write("Vous avez envoy�� : " + querystring.parse(postData).text);
	 response.end();
	}

	/* Game core loading */

	loadGameCore();
}

exports.loadCore = loadCore;