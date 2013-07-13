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

s.fake_latency = 0;
s.local_time = 0;
s._dt = new Date().getTime();
s._dte = new Date().getTime();
s.universe ; 
s.playerCount = -1 ; 
s.users = []; 
s.socketManager ;

    //a local queue of messages we delay if faking latency
s.messages = [];
// constructor:

	s.initialize = function () { 

	    this.sectors = {sector1: []};


	}

	
// public methods:
	s.log = function() {

	}
	s.pushMessage = function(client, message) {
		this.messages.push({message:message, client:client}); 
	}
	s.onMessage = function() {
		
	}
	s.onInput = function() {
		
	}
	s.setSector = function(targetSector, newSector) {
		this.sectors.sector1 = newSector ; 
	}
	s.generateUniverse = function(universeToken) {
		sector = {
			objects:[

			{id:500,type:'Collectable', image: { src: 'Asteroid.png' },name: 'Minerai 1',position: {x: 500,y: 600, z:1, rotation: 10}, weight:10, dimensions: { width:218, height:181 } },
			{id:400,type:'Collectable', image: { src: 'Asteroid.png' },name: 'Minerai 1',position: {x: 200,y: 600, z:1, rotation: 130}, weight:10, dimensions: { width:218, height:181 } },
			{id:300,type:'Collectable', image: { src: 'Asteroid.png' },name: 'Minerai 1',position: {x: 800,y: 100, z:1, rotation: 530}, weight:10, dimensions: { width:218, height:181 } },
			{id:200,type:'Collectable', image: { src: 'Asteroid.png' },name: 'Minerai 1',position: {x: 1500,y: 1000, z:1, rotation: 30}, weight:10, dimensions: { width:218, height:181 } },
			{id:100,type:'Collectable', image: { src: 'Asteroid.png' },name: 'Minerai 1',position: {x: 200,y: 1600, z:1, rotation: 30}, weight:10, dimensions: { width:218, height:181 } },
			{id:0,type:'Station', image: { src: 'Anna Cruiser.png' },name: 'Station spatiale internationale',position: {x: 1500,y: 600, z:1},life: 150000, dimensions: { width:218, height:181 } },
			{id:1, type:'Station', image: { src: 'stationIso.png' },name: 'Station spatiale internationale',position: {x: 500,y: 500, z:1, rotation:0},life: 150000, dimensions: { width:218, height:181 } },
			{
				id:6, 
				type:'Bot', 
				src: 'Anna Cruiser.png',
				name: 'Station spatiale internationale',
				position: {x: 920,y: 500, z:1, rotation:-90},  
				initPosition: {x: 920,y: 500, z:1, rotation:-90},
				life: 150000, 
				width:1032, 
				height:620,
				destination: {x:null, y:null},
				limitSpeed: 1.5,
				acceleration: 0.06 , 
				limitRotation:0,
				currentSpeed: 0 , 
				rotationSpeed: 3,
				hasDestination: false,
				weapons: 2,
				hasTarget: false , 
				energy: 100,
				targetType: null,
				targetId: null,
				AIStopRange: 600 , 
				AIRange: 500,
				AI:"wait",
				name:null,
				type:"Bot",
			},
			{
				id:2, 
				type:'Bot', 
				src: 'Anna Cruiser.png',
				name: 'Station spatiale internationale',
				position: {x: 1520,y: 700, z:1, rotation:-90},  
				initPosition: {x: 1520,y: 500, z:1, rotation:-90},
				life: 150000, 
				width:1032, 
				height:620,
				destination: {x:null, y:null},
				limitSpeed: 1.5,
				acceleration: 0.06 , 
				limitRotation:0,
				currentSpeed: 0 , 
				rotationSpeed: 3,
				hasDestination: false,
				weapons: 2,
				hasTarget: false , 
				energy: 100,
				targetType: null,
				targetId: null,
				AIStopRange: 600 , 
				AIRange: 500,
				AI:"wait",
				name:null,
				type:"Bot",
			},
			{
				id:3, 
				type:'Bot', 
				src: 'Anna Cruiser.png',
				name: 'Station spatiale internationale',
				position: {x: 520,y: 200, z:1, rotation:-90},  
				initPosition: {x: -1520,y: 500, z:1, rotation:-90},
				life: 150000, 
				width:1032, 
				height:620,
				destination: {x:null, y:null},
				limitSpeed: 1.5,
				acceleration: 0.06, 
				limitRotation:0,
				currentSpeed: 0 , 
				rotationSpeed: 3,
				hasDestination: false,
				weapons: 2,
				hasTarget: false , 
				energy: 100,
				targetType: null,
				targetId: null,
				AIStopRange: 600 , 
				AIRange: 500,
				AI:"wait",
				name:null,
				type:"Bot",
			},
			],
			tiles:[
			{	id:1,position:{x:Math.random() * 2500,y:Math.random() * 2500, z: 1}, src:"iso-02-04.png",},
			]
		};
		for (var ll = 0 ; ll < 1; ll++) {
			sector.tiles[ll] = {	id:ll,position:{x:Math.random() * 2500,y:Math.random() * 2500, z: 1}, src:"iso-05-03.png",};
		}
		this.setSector(null, sector); 
		this.universe = new phobos.Game(universeToken);
		this.universe.loadSector(this.sectors.sector1); 
	}
	s.startUniverse = function(universe) {
		this.universe.startUpdate();
	}
	s.endUniverse = function(universe) {
		this.universe.stopUpdate();
	}
	s.loadSectorPlayers = function(socket, sector) {
		var sectorPlayers = this.universe.getShipsList() ; 
		socket.emit('sectorPlayersLoaded', sectorPlayers);
	}

	s.loadSector = function(socket, sector) {
		// var sectorTiles = this.universe.getTilesList() ; 
		// var sectorObjects = this.universe.getObjectsList() ; 
		// sector = {objects: sectorObjects, tiles: sectorTiles}; 
		// console.log(this.sectors.sector1); 
		socket.emit('sectorLoaded', this.getSectorExport(sector));
	}

	s.playerMove = function(playerId, moveData) {
		this.universe._shipsList[playerId].moveTo(moveData); 
	}

	s.createPingTimer = function() {

	        //Set a ping timer to 1 second, to maintain the ping/latency between
	        //client and server and calculated roughly how our connection is doing

	    setInterval(function(){

	        this.last_ping_time = new Date().getTime() - this.fakeLag;
	        this.socket.send('p.' + (this.lastPingTime) );

	    }.bind(this), 1000);
	    
	}; //s.createPingTimer

	s.createNewPlayer = function() {

	}

	s.setSocketsManager = function(newSocketsManager) {
		this.socketManager = newSocketsManager; 
	}

	s.emitSocket = function(message, messageData) {
		if (this.socketManager)
			this.socketManager.emit(message, messageData);
		else return -1; 
	}

	s.broadcastToAllSocket = function(message, messageData) {
		if (this.socketManager) {
			this.socketManager.emit(message, messageData);
			this.socketManager.broadcast.emit(message, messageData);
		}
		else return -1; 
	}

	s.getSyncDataSector = function(sector) {
		//Now only one sector. 
		//this.getGame().getSector(sector);
		var sector = this.getGame();
		var shipsList = sector.getShipsList();
		var objectsList = sector.getObjectsList();

		return {ships: shipsList, objects: objectsList};
	}

	

	s.getShipsList = function() {
		return (this.getGame()._shipsList);
	}

	s.getGame = function() {
		return this.universe; 
	}

	s.getGameFrame = function() {
		return this.getGame().getFrame();
	}

	s.getPlayerData = function(playerId) {
		this.playerCount++ ; 
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
			position: {x: Math.random() * 500, y: Math.random() * 500, z:1, rotation:0 }, 
			cargo: {capacity:500, content:[]},
			name: "testeur" + this.playerCount, 
		})
	}

	s.getUsers = function() {
		return this.users;
	}

	s.getSectorExport = function(sector) {
		var sharedData = this.getGame().getSharedData(); //TO DO : ONLY FOR A SECTOR
		return (sharedData);
	}

	s.getExport = function() {
		var sharedData = this.getGame().getSharedData();
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

	s.addUser = function(user) {
		this.users[user.id] = user ; 
	}

	s.removeUser = function(user) {
		this.users[user.id] = null ; 
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
		if (this.universe.playerJoin(shipData, false))
			return shipData;
		else return -1; 
	};
	phobos.Server = Server;

}());
