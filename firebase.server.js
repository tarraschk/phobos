var Firebase = require('firebase');
var myRootRef = new Firebase('https://phobosdb.firebaseIO.com/');
// myRootRef.set({server:{game: {sector1:"data", sector2: "coucou"} } });

setInterval(function(){
	var sharedData = server.getGame().getSharedData();
	myRootRef.server( set.getGame().getSharedData() );

}.bind(this), 100);