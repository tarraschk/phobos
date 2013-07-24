/**

Server is the main entry of the game, once app.js has done it's job (loading assets and sockets). 
It initializes the game and manages the sockets entries. 
*/
phobos = this.phobos || {};


(function () {
	var Server = Class.create({

// constructor:

	initialize: function () { 
		this._universeGenerator = new phobos.UniverseGenerator();
		this._fake_latency = 0;
		this._local_time = 0;
		this._dt = new Date().getTime();
		this._dte = new Date().getTime();
		this._game ; 
		this._playerCount = -1 ; 
		this._users = []; 
		this._socketManager ;
    //a local queue of messages we delay if faking latency
		this._messages = [];
		this._universeGenerator ;
	},

	
// public methods:

	

	//Socket management


	emitSocket: function(message, messageData) {
		if (this._socketManager)
			this._socketManager.emit(message, messageData);
		else return -1; 
	},

	broadcastToAllSocket: function(message, messageData) {
		if (this._socketManager) {
			this._socketManager.emit(message, messageData);
			this._socketManager.broadcast.emit(message, messageData);
		}
		else return -1; 
	},

	// Message management

	pushMessage: function(client, message) {
		this._messages.push({message:message, client:client}); 
	},

	// Game generator

	startGame: function(universe) {
		this._game.startUpdate();
	},
	endGame: function(universe) {
		this._game.stopUpdate();
	},

	generateGame: function(universeToken) {
		var sector0 = this.getUniverseGenerator().generateSector(0, 11434);
		var sector1 = this.getUniverseGenerator().generateSector(1, 134);
		var generatedGame = new phobos.Game();
		generatedGame.loadSector(0, sector0); 
		generatedGame.loadSector(1, sector1); 
		this.setGame(generatedGame); 
	},

	//Player management


	addUser: function(user) {
		this._users[user.id] = user ; 
	},

	removeUser: function(user) {
		this._users[user.id] = null ; 
	},

	playerLogin: function(loginData) {
		var credentials = loginData.loginData;
		var socketData = loginData.socket ; 
		if (1) { // login check
			var shipData = this.getPlayerData(loginData);
			shipData.frame = this.getGameFrame(); 
			this.addUser(shipData); 
			return (this.playerJoinsGame(shipData, socketData)); 
		}
	},

	playerJoinsGame: function(shipData, socketData){
		if (this._game.playerJoin(shipData, false))
			return shipData;
		else return -1; 
	},

	//Misc

	log: function(log) {
		console.log(log);
	},


	getPlayerData: function(playerId) {
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
			image: { 
				src: 'Hercule/SpriteHercules.png', 
				animation: true, 
				spritesheet: { 
					frames: {width: 294, height: 266, regX: 293 / 2, regY: 266 / 2, vX:0.5, currentAnimationFrame: 15}, 
					animations: { walk: [0, 70, "walk"] },
				}, 
			},
			position: {x: Math.random() * 500, y: Math.random() * 500, z:1, rotation:0, sector:0 }, 
			cargo: {capacity:600, content:[]},
			name: "testeur" + this._playerCount, 
		})
	},

	loadSectorPlayers: function(socket, sector) {
		var sectorPlayers = this._game.getShipsList() ; 
		socket.emit('sectorPlayersLoaded', sectorPlayers);
	},

	loadSector: function(socket, sector) {
		// var sectorTiles = this._game.getTilesList() ; 
		// var sectorObjects = this._game.getObjectsList() ; 
		// sector = {objects: sectorObjects, tiles: sectorTiles}; 
		// console.log(this.sectors.sector1); 
		socket.emit('sectorLoaded', this.getSectorExport(sector));
	},




	getSyncDataSector: function(sector) {
		//Now only one sector. 
		//this.getGame().getSector(sector);
		var sector = this.getSectorExport(sector);

		return sector;
	},


	getSectorExport: function(sector) {
		var sharedData = this.getGame().exportSector(sector); //TO DO : ONLY FOR A SECTOR
		return (sharedData);
	},

	export: function(sector) {
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
	},

	//Setters and getters

	setSocketsManager: function(newSocketsManager) {
		this._socketManager = newSocketsManager; 
	},

	getUniverseGenerator: function() {
		return this._universeGenerator;
	},

	setGame: function(game) {
		this._game = game ; 
	},

	getUsers: function() {
		return this._users;
	},

	getGame: function() {
		return this._game; 
	},

	getGameFrame: function() {
		return this.getGame().getFrame();
	},
});
	phobos.Server = Server;

}());
