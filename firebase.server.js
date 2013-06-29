var Firebase = require('firebase');
var myRootRef = new Firebase('https://phobosalpha.firebaseio.com/');
myRootRef.set({server:{game: {sector1:"data", sector2: "coucou"} } });

setInterval(function(){
	var sharedData = server.getGame().getSharedData();
	console.log("SHAREDDATA");
	console.log("SHAREDDATA");
	console.log("SHAREDDATA");
	console.log("SHAREDDATA");
	console.log("SHAREDDATA");
	console.log(sharedData);	
	console.log("SHAREDDATA");
	console.log("SHAREDDATA");
	console.log("SHAREDDATA");

	myRootRef.set( {server:{game:sharedData}} );

}.bind(this), 2000);