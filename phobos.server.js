utils = new phobos.Utils();

server = new phobos.Server();

server.generateGame(1);
server.startGame();


io.sockets.on('connection', function(client) {

	server.setSocketsManager(client);

	/* Credentials actions, player joins or out */

	client.on('login', function(loginData){
		var loggedInShipData = server.playerLogin({client: this,loginData: loginData}); 

		if (loggedInShipData) {	
			client.emit('loggedIn', loggedInShipData);
  			client.broadcast.emit('newPlayerLoggedIn', loggedInShipData);
		}
	});

	/* Player actions in the game */


	client.on('playerMove', function(data){
		server.getGame().getShipsList()[data.player].moveTo(data);
  		client.broadcast.emit('playerMove', data);
  		client.emit('playerMove', data);
	});

	client.on('playerAttack', function(data) {
		server.getGame().playerAttack(data.player, data.target); 
		server.broadcastToAllSocket('playerAttack', data);
	});

	client.on('playerCollects', function(data) {
		server.getGame().playerCollects(data.player, data.collectable); 
		server.broadcastToAllSocket('playerCollects', data);
	});

	client.on('playerDockTo', function(data){
		console.log("DOCK TO !");
		console.log(data);
		var playerDocking = data.player;
		var station = data.station;
		server.getGame().getShipsList()[playerDocking.id].dockTo(station);
		// server.playerMove(data.player, data);
  		client.broadcast.emit('playerDockTo', data);
  		client.emit('playerDockTo', data);
	});

	/* Player request for data in the game */


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


	/* Synchronisation and client-server binding  */

	client.on('ping', function() {
		client.emit('pong'); 
	}); 
	
	client.on('sync', function(player) {
		var syncData = server.getSyncDataSector(player.position);
		var frameSend = server.getGameFrame();
		client.emit('sync', { frame:frameSend, data: syncData }); 
	});
 }); 
