
phobos = this.phobos || {};


(function () {

	function Client() {
		this.initialize();
	}
	var c = Client.prototype ;
	
	c.game ; 

	    //a local queue of messages we delay if faking latency
	c.messages = [];
	c.playerId ; 
	// constructor:

	c.initialize = function () { 
		c.playerId = utils.generateId(); 
		console.log("this client :" + c.playerId);
	}

	
// public methods:
	c.log = function() {

	}
	c.onMessage = function() {
		
	}
	c.onInput = function() {
		
	}

	// Link with server methods 
	c.loadServerPlayer = function(player) {
		socket.emit('loadPlayers', player);
	}
	c.loadSectorPlayers = function(playersData) {

		for (key in playersData) {
			if (String((key)) === key && playersData.hasOwnProperty(key)) {
				if (playersData[key].index == playersData[key].id) {
					if (key != this.playerId) //Must be other players, not main player already loaded
						this.playerJoinGame(playersData[key], false); 
					else console.log("THIS IS THE PLAYER"); 
				}
			}
		}
	}

	c.loginToServer = function() {
		socket.emit('login', {name: this.playerId });

	}
	// General methods 
	
	c.onPlayerMove = function(playerMoveData) {
		console.log(playerMoveData);
		console.log(this.game._shipsList);
		if (this.game._shipsList[playerMoveData.player])
			this.game._shipsList[playerMoveData.player].moveTo({x:playerMoveData.x, y:playerMoveData.y});
	}

	/* A player joined the game.
	This player IS the current client's player. */
	c.mainPlayerLogged = function(playerData)  {
		this.setPlayerId(playerData.id); 
		this.playerJoinGame(playerData, true); 
	}

	/* A player joined the game.
	This player isn't the current client's player. */
	c.newPlayerLogged = function(playerData)  {
		this.playerJoinGame(playerData, false); 
	}

	/* Adds a player to the client's game. */
	c.playerJoinGame  = function(playerData, mainPlayer) {
		this.game.playerJoin(playerData, mainPlayer); 
		if (mainPlayer) this.initMouseClick() ;
	}

	c.initMouseClick = function() {
		var that = this ; 
		$(document).on('click', function(e){
			if (allowMoveClick) {
				console.log("click"); 
				var gameCam = that.game.getCamera();
				var cooClick = utils.cameraToAbsolute({	x:e.clientX, y:e.clientY}, gameCam._position);

				var cooClick2 = utils.stdToAbsolute({	x:e.clientX, y:e.clientY}, gameCam._position);
				console.log(that.game._playerShip); 
				that.game._playerShip.moveTo({x:cooClick2.x, y:cooClick2.y});
	        	socket.emit('playerMove', {player: that.game._playerShip.id, x:cooClick2.x, y:cooClick2.y});
			}
		});
	}

	c.loadGameData = function() {
		this.loadServerPlayer() ; 
		this.game = new phobos.Game();
	}
	c.startGame = function() {
		this.game.startUpdate();
	}
	c.endUniverse = function(universe) {
		this.game.stopUpdate();
	}

	c.getGame = function() {
		return this.game ;
	}
	c.setPlayerId = function(newPlayerId) {
		this.playerId = newPlayerId ; 
	}

	c.createPingTimer = function() {

	        //Set a ping timer to 1 second, to maintain the ping/latency between
	        //client and server and calculated roughly how our connection is doing

	    setInterval(function(){

	        this.last_ping_time = new Date().getTime() - this.fakeLag;
	        this.socket.send('p.' + (this.lastPingTime) );

	    }.bind(this), 1000);
	    
	}; //s.createPingTimer


	phobos.Client = Client;

}());
