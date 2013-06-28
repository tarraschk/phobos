var Firebase = require('firebase');
var myRootRef = new Firebase('https://phobosdb.firebaseIO.com/');
// myRootRef.set({server:{game: {sector1:"data", sector2: "coucou"} } });

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

	myRootRef.set( server.getGame().getSharedData() );

}.bind(this), 100);