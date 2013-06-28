var Firebase = require('firebase');
var myRootRef = new Firebase('https://phobosdb.firebaseIO.com/');
myRootRef.set({server:{game: {sector1:"data", sector2: "coucou"} } });

console.log(server.getGame().getSharedData());	
setInterval(function(){
	console.log(server.getGame().getSharedData());	
	var sharedData = server.getGame().getSharedData();
	myRootRef.set({server:sharedData });

}.bind(this), 1500);