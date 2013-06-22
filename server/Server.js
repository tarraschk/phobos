
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
	s.generateUniverse = function(universeToken) {
		sector = {
			objects:[{id:0,type:'Station', src: 'stationIso.png',name: 'Station spatiale internationale',x: 0,y: 0,life: 150000},{id:1, type:'Station', src: 'stationIso.png',name: 'Station spatiale internationale',x: 0,y: 0,life: 150000},
			{id:3, type:'Bot', src: 'stationIso.png',name: 'Station spatiale internationale',x: 0,y: 0,life: 150000}],
			tiles:{	id:1,x:Math.random() * 2500,y:Math.random() * 2500,	src:"iso-02-04.png",},
		};

		this.universe = new phobos.Game(universeToken);
		this.universe.loadSector(sector); 
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
		var sectorTiles = this.universe.getTilesList() ; 
		var sectorObjects = this.universe.getObjectsList() ; 
		sector = {objects: sectorObjects, tiles: sectorTiles}; 
		console.log(sector); 
		// socket.emit('sectorLoaded', sectorObjects);
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

	s.getPlayerData = function(playerId) {
		this.playerCount++ ; 
		return ({ position: {x: Math.random() * 500, y: Math.random() * 500}, name: playerId.name, id: this.playerCount })
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
