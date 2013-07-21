/**

Server is the main entry of the game, once app.js has done it's job (loading assets and sockets). 
It initializes the game and manages the sockets entries. 
*/
phobos = this.phobos || {};


(function () {

	function Server() {
		this.initialize();
	}
	var s = Server.prototype ;

s._fake_latency = 0;
s._local_time = 0;
s._dt = new Date().getTime();
s._dte = new Date().getTime();
s._game ; 
s._playerCount = -1 ; 
s._users = []; 
s._socketManager ;
s._universeGenerator ;

    //a local queue of messages we delay if faking latency
s.messages = [];
// constructor:

	s.initialize = function () { 
		this._universeGenerator = new phobos.UniverseGenerator();
	}

	
// public methods:

	

	//Socket management


	s.emitSocket = function(message, messageData) {
		if (this._socketManager)
			this._socketManager.emit(message, messageData);
		else return -1; 
	}

	s.broadcastToAllSocket = function(message, messageData) {
		if (this._socketManager) {
			this._socketManager.emit(message, messageData);
			this._socketManager.broadcast.emit(message, messageData);
		}
		else return -1; 
	}

	// Message management

	s.pushMessage = function(client, message) {
		this._messages.push({message:message, client:client}); 
	}

	// Game generator

	s.startGame = function(universe) {
		this._game.startUpdate();
	}
	s.endGame = function(universe) {
		this._game.stopUpdate();
	}

	s.generateGame = function(universeToken) {
		var sector0 = this.getUniverseGenerator().generateSector(0, 11434);
		var generatedGame = new phobos.Game();
		generatedGame.loadSector(0, sector0); 
		this.setGame(generatedGame); 
	}

	//Player management


	s.addUser = function(user) {
		this._users[user.id] = user ; 
	}

	s.removeUser = function(user) {
		this._users[user.id] = null ; 
	}

	s.playerLogin = function(loginData) {
		var credentials = loginData.loginData;
		var socketData = loginData.socket ; 
		if (1) { // login check
			var shipData = this.getPlayerData(loginData);
			shipData.frame = this.getGameFrame(); 
			this.addUser(shipData); 
			return (this.playerJoinsGame(shipData, socketData)); 
		}
	}

	s.playerJoinsGame = function(shipData, socketData){
		if (this._game.playerJoin(shipData, false))
			return shipData;
		else return -1; 
	};

	//Misc

	s.log = function(log) {
		console.log(log);
	}


	s.getPlayerData = function(playerId) {
		this._playerCount++ ; 
		return ({ 
			id: utils.generateId() ,
			destination: {x:null, y:null},
			limitSpeed: 5.5,
			acceleration: 0.13 , 
			limitRotation:0,
			weapons: 2,
			currentSpeed: 0 , 
			rotationSpeed: 6.5,
			hasDestination: false,
			hasTarget: false , 
			energy: 1000,
			targetType: null,
			targetId: null,
			status:"space",
			position: {x: Math.random() * 500, y: Math.random() * 500, z:1, rotation:0, sector:0 }, 
			cargo: {capacity:600, content:[]},
			name: "testeur" + this._playerCount, 
		})
	}

	s.loadSectorPlayers = function(socket, sector) {
		var sectorPlayers = this._game.getShipsList() ; 
		socket.emit('sectorPlayersLoaded', sectorPlayers);
	}

	s.loadSector = function(socket, sector) {
		// var sectorTiles = this._game.getTilesList() ; 
		// var sectorObjects = this._game.getObjectsList() ; 
		// sector = {objects: sectorObjects, tiles: sectorTiles}; 
		// console.log(this.sectors.sector1); 
		socket.emit('sectorLoaded', this.getSectorExport(sector));
	}




	s.getSyncDataSector = function(sector) {
		//Now only one sector. 
		//this.getGame().getSector(sector);
		var sector = this.getSectorExport(sector);

		return sector;
	}


	s.getSectorExport = function(sector) {
		var sharedData = this.getGame().exportSector(sector); //TO DO : ONLY FOR A SECTOR
		return (sharedData);
	}

	s.export = function(sector) {
		var sharedData = this.getGame().exportSector(sector);
		var users = this.getUsers();
		return (
		{
			server:{
					game: 
						{ 
							sector1: sharedData
						},
					users: users
			}
		});
	}

	//Setters and getters

	s.setSocketsManager = function(newSocketsManager) {
		this._socketManager = newSocketsManager; 
	}

	s.getUniverseGenerator = function() {
		return this._universeGenerator;
	}

	s.setGame = function(game) {
		this._game = game ; 
	}

	s.getUsers = function() {
		return this._users;
	}

	s.getGame = function() {
		return this._game; 
	}

	s.getGameFrame = function() {
		return this.getGame().getFrame();
	}

	phobos.Server = Server;

}());
