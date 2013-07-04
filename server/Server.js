
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
		this.show_help = false;             //Whether or not to draw the help text
	    this.naive_approach = false;        //Whether or not to use the naive approach
	    this.show_server_pos = false;       //Whether or not to show the server position
	    this.show_dest_pos = false;         //Whether or not to show the interpolation goal
	    this.client_predict = true;         //Whether or not the client is predicting input
	    this.input_seq = 0;                 //When predicting client inputs, we store the last input as a sequence number
	    this.client_smoothing = true;       //Whether or not the client side prediction tries to smooth things out
	    this.client_smooth = 25;            //amount of smoothing to apply to client update dest

	    this.net_latency = 0.001;           //the latency between the client and the server (ping/2)
	    this.net_ping = 0.001;              //The round trip time from here to the server,and back
	    this.lastPingTime = 0.0;        //The time we last sent a ping
	    this.fakeLag = 0;                //If we are simulating lag, this applies only to the input client (not others)
	    this.fake_lag_time = 0;

	    this.net_offset = 100;              //100 ms latency between server and client interpolation for other clients
	    this.buffer_size = 2;               //The size of the server history to keep for rewinding/interpolating.
	    this.target_time = 0.01;            //the time where we want to be in the server timeline
	    this.oldest_tick = 0.01;            //the last time tick we have available in the buffer

	    this.client_time = 0.01;            //Our local 'clock' based on server time - client interpolation(net_offset).
	    this.server_time = 0.01;            //The time the server reported it was at, last we heard from it
	    
	    this.dt = 0.016;                    //The time that the last frame took to run
	    this.fps = 0;                       //The current instantaneous fps (1/this.dt)
	    this.fps_avg_count = 0;             //The number of samples we have taken for fps_avg
	    this.fps_avg = 0;                   //The current average fps displayed in the debug UI
	    this.fps_avg_acc = 0;    

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
			{id:0,type:'Station', image: { src: 'Anna-Cruiser.png' },name: 'Station spatiale internationale',position: {x: 1500,y: 600},life: 150000, width:218, height:181},
			{id:1, type:'Station', image: { src: 'Anna Cruiser.png' },name: 'Station spatiale internationale',position: {x: 500,y: 500, z:1, rotation:0},life: 150000, width:1032, height:620},
			{
				id:6, 
				type:'Bot', 
				src: 'Anna Cruiser.png',
				name: 'Station spatiale internationale',
				position: {x: -1520,y: 500, z:1, rotation:-90},  
				initPosition: {x: -1520,y: 500, z:1, rotation:-90},
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
				initPosition: {x: -1520,y: 500, z:1, rotation:-90},
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
			],
			tiles:{	id:1,x:Math.random() * 2500,y:Math.random() * 2500,	src:"iso-02-04.png",},
		};
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
			limitSpeed: 4.5,
			acceleration: 0.06 , 
			limitRotation:0,
			weapons: 2,
			currentSpeed: 0 , 
			rotationSpeed: 4,
			hasDestination: false,
			hasTarget: false , 
			energy: 1000,
			targetType: null,
			targetId: null,
			status:"space",
			position: {x: Math.random() * 500, y: Math.random() * 500, z:1, rotation:0 }, 
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
