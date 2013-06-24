

utils = new phobos.Utils();

server = new phobos.Server();

server.generateUniverse(1);
server.startUniverse();


io.sockets.on('connection', function(client) {

	server.setSocketsManager(client);

	client.on('login', function(loginData){
		var loggedInShipData = server.playerLogin({client: this,loginData: loginData}); 

		if (loggedInShipData) {	
			client.emit('loggedIn', loggedInShipData);
  			client.broadcast.emit('newPlayerLoggedIn', loggedInShipData);
		}
	});

	client.on('playerMove', function(data){
		server.playerMove(data.player, data);
  		client.broadcast.emit('playerMove', data);
  		client.emit('playerMove', data);
	});
	client.on('playerData', function (playerData) {

	});

	client.on('loadPlayers', function() {
		server.loadSectorPlayers(this); 
	});
	client.on('loadSector', function() {
		console.log(":: Loading sector :: "); 
		server.loadSector(this); 
	});
	client.on('playerLogin', function(user) {

	}); 
	
	client.on('tickPlayer', function(user) {
	}); 

	client.on('ping', function() {
		client.emit('pong'); 
	}); 
	
}); 
