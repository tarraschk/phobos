var Firebase = require('firebase');
var phobosAlpha = new Firebase('https://phobosalpha.firebaseio.com/');
// phobosAlpha.set({server:{game: {sector1:"data", sector2: "coucou"} } });


function startGameUpdate() {
	setInterval(function(){
		var sharedData = server.getGame().getSharedData();
		phobosAlpha.set( {server:{game: { sector1: sharedData}}} );

	}.bind(this), 500);
}

function recoverGame() {
	var phobosAlphaGame = new Firebase('https://phobosalpha.firebaseio.com/server/');
	phobosAlphaGame.on('value', function(server) {
		console.log("Server recovered");
		console.log(server.val());

	});

}

function startNewGame() {

}
if (firebaseRecover)
	recoverGame();
else 
	startGameUpdate();