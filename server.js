var http = require('http'); 
var players = []; 
var playersCount = 0;
var asteroidImg = "img/tiles/iso-02-00.png";
var spaceData = [{x:15, y:15, z:1, imgSrc:"img/tiles/iso-02-00.png"}, {x:215, y:215, z: 1, imgSrc:"" + asteroidImg + ""}];
var spaceObjects = [{x:255, y:15, z:1, imgSrc:"img/objects/stationIso.png"}];

for (var k = 0 ; k < 1000 ; k ++) {
	spaceData[k] = {
		x:Math.random() * 3500 - 1500,
		y:Math.random() * 3500 - 1500,
		imgSrc:asteroidImg,
	}
}


httpServer = http.createServer(function(req, res) {
	res.end("HelloWorld") ; 
}); 

var io = require('socket.io').listen(httpServer);

httpServer.listen(1337); 

function spawnPlayer(userName) {
	var newPlayer = {
		name: userName,
		x: Math.random() * 250 + 150,
		y: Math.random() * 250 + 100, 
		o: 0,
		id: playersCount 
	}
	console.log('new player spawned : ');
	console.log(newPlayer);
	return newPlayer;
}


io.sockets.on('connection', function(socket) {
	console.log('Nouveau utilisateur') ; 

	socket.on('playerData', function (playerData) {
		console.log("Player data received");
		console.log(playerData);
		socket.broadcast.emit('getInfoFromServer', playerData);
	})

	socket.on('loadPlayers', function() {
		socket.emit('playersLoaded', players);
	});
	socket.on('playerLogin', function(user) {
		var newPlayer = spawnPlayer(user); 
		console.log('new player: ');
		console.log(spaceData); 
		socket.emit('loadSpace', spaceData);
		socket.emit('loadObjects', spaceObjects); 
		socket.emit('playerLogged', newPlayer, players); 
		socket.broadcast.emit('newPlayer', newPlayer);
		players[playersCount] = newPlayer;
		playersCount++;
	}); 
	
	socket.on('tickPlayer', function(user) {
		players[user.index] = user;
	}); 
	
}); 