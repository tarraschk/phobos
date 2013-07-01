
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
	c.lastPingTime ; 
	c.pingTime ; 
	c.syncTime = 5000 ; 
	// constructor:

	c.initialize = function () { 
		c.playerId = utils.generateId(); 
	}

	
// public methods:
	c.log = function() {

	}
	c.onMessage = function() {
		
	}
	c.onInput = function() {
		
	}

	c.socketEmit = function(message, data) {
		socket.emit(message, data);
	}

	// Link with server methods 
	c.loadServerPlayer = function(player) {
		socket.emit('loadPlayers', player);
	}
	c.loadSectorData = function(player) {
		socket.emit('loadSector', player);
	}

	c.loadSector = function(sector) {
		this.game.loadSector(sector); 
	}

	c.loadSectorPlayers = function(playersData) {
		console.log("loading sector playerz");
		console.log(playersData);
		for (key in playersData) {
			if (String((key)) === key && playersData.hasOwnProperty(key)) {
				if (playersData[key].index == playersData[key].id) {
					console.log(key);
					console.log("player id: " + this.playerId);
					if (key != this.playerId) //Must be other players, not main player already loaded
					{
						var player = playersData[key].shared
						player.id = playersData[key].id;
						this.playerJoinGame(player, false); 
					}
				}
			}
		}
	}

	c.loginToServer = function() {
		socket.emit('login', {name: this.playerId });

	}
	// General methods 
	
	c.onPlayerMove = function(playerMoveData) {
		this.game._shipsList[playerMoveData.player].shared.position.x += 5 ; 
		if (this.game._shipsList[playerMoveData.player])
			this.game._shipsList[playerMoveData.player].moveTo({x:playerMoveData.x, y:playerMoveData.y});
	}

	c.onPlayerDock = function(dock) {
		console.log("Docking")
		console.log(dock);
		this.getGame().getShipsList()[dock.player.id].dockTo(dock.station);
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
	c.playerJoinGame  = function(player, mainPlayer) {
		this.game.playerJoin(player, mainPlayer); 
		if (mainPlayer) this.initMouseClick() ;
	}


	c.initMouseClick = function() {
		var that = this ; 
		$(document).on('click', function(e){
			if (allowMoveClick) {
				that.inputPlayer("mouse1InSpace", e);
			}
		});
	}

	c.inputPlayer = function(command, input) {
				switch(this.getGame().getMainPlayerStatus()) {
					case "space":

						switch (command) {

						case "mouse1InSpace":

							console.log(allowMoveClick);
							console.log("Move to !");
							var gameCam = this.game.getCamera();
							var cooClick = utils.cameraToAbsolute({	x:input.clientX, y:input.clientY}, gameCam._position);

							var cooClick2 = utils.stdToAbsolute({	x:input.clientX, y:input.clientY}, gameCam._position);
							
							// this.game._playerShip.moveTo({x:cooClick2.x, y:cooClick2.y});
				        	socket.emit('playerMove', {player: this.game._playerShip.id, x:cooClick2.x, y:cooClick2.y});

						break;
						case "mouse2InSpace":

						break;
						case "mouse1Object":

							var object = input.targObject;
							console.log("STATION !");
							console.log(object);
							allowMoveClick = false ; 
							debug('arrimage '+object._name);
							client.socketEmit('playerDockTo', {player:client.getGame().getPlayerShip().getShared(), station:object.getShared()});

						break;
					}

					break;
					case "docked":
						console.log("Docked !");
					break;
					case "killed":
					break;
		}
	}

	c.loadGameData = function() {
		this.loadServerPlayer() ; 
		this.loadSectorData() ; 
		this.game = new phobos.Game();
	}

	c.setBotBehavior = function(newBotBehavior, bot, data) {
		this.game.getObjectsList()[bot.id].setBotBehavior(newBotBehavior, data);
	}

	c.setGameFrame = function(frame) {
		this.game.setFrame(frame);
	}

	c.startGame = function() {
		this.game.startUpdate();
	}
	c.endUniverse = function(universe) {
		this.game.stopUpdate();
	}

	c.getShipsList = function() {
		return (this.getGame()._shipsList);
	}

	c.getGame = function() {
		return this.game ;
	}
	c.setPlayerId = function(newPlayerId) {
		this.playerId = newPlayerId ; 
	}

	c.onPong = function(pongTime) {
		this.pongTime = new Date().getTime()
		var pingResult = (this.pongTime - this.lastPingTime) / 2; //split : this time is the go and return time, we just want half of it
		$("#ping").html(pingResult); 
	}

	c.createPingTimer = function() {
	        //Set a ping timer to 1 second, to maintain the ping/latency between
	        //client and server and calculated roughly how our connection is doing

	    setInterval(function(){
	        this.lastPingTime = new Date().getTime() //- this.fakeLag;
	        socket.emit('ping', { pingTime:(this.lastPingTime) } );

	    }.bind(this), 2000);
	    
	}; //s.createPingTimer

	c.diffShip = function(shipServ, shipClient, nowServ, nowClient) {
		var positionServ = shipServ.shared.position;
		var positionClient = shipClient.shared.position;
		var dPos = utils.getDiffPosition(positionServ, positionClient);
		var dT = nowClient - nowServ;

	}

	c.sync = function(frameServer, serverData) {
		var frameClient = this.getGame().getFrame();

		servShips = serverData.ships;
		servObjects = serverData.objects;


		for (key in servShips) {
			if (String((key)) === key && servShips.hasOwnProperty(key)) {
				if (servShips[key]) {
					if (servShips[key].index == servShips[key].id) {
						this.diffShip(servShips[key], this.getGame().getShipsList()[key], frameServer, frameClient);
						this.getGame().getShipsList()[key].shared.position = servShips[key].shared.position;
					}
				}
			}
		}
		for (key in servObjects) {
			if (String((key)) === key && servObjects.hasOwnProperty(key)) {
				if (servObjects[key]) {
					if (servObjects[key].index == servObjects[key].id) {
					// servObjects[key].tick();
					}
				}
			}
		}
	}

	c.createServerLoop = function() {
		setInterval(function(){
	        socket.emit('sync', { player:this.getGame().getPlayerShip().getShared() } );

	    }.bind(this), this.syncTime);
	}

	phobos.Client = Client;

}());
