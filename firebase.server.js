var Firebase = require('firebase');
var phobosAlpha = new Firebase('https://phobosalpha.firebaseio.com/');
phobosAlpha.set({server:{game: {sector1:"data", sector2: "coucou"} } });

setInterval(function(){
	var sharedData = server.getGame().getSharedData();

	phobosAlpha.set( {server:{game: { sector1: sharedData}}} );

}.bind(this), 500);