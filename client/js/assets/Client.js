
phobos = this.phobos || {};


(function () {

	function Client() {
		this.initialize();
	}
	var c = Client.prototype ;
	
	c.game ; 

	    //a local queue of messages we delay if faking latency
	c.messages = [];
	// constructor:

	c.initialize = function () { 
	}

	
// public methods:
	c.log = function() {

	}
	c.onMessage = function() {
		
	}
	c.onInput = function() {
		
	}

	c.connectToServer = function() {

		socket.emit('connect', {name: 'wam'});
		
	}

	c.playerJoin  = function(playerData) {
		var that = this ; 
		this.game.playerJoin(playerData, true); 

		$(document).on('click', function(e){
			if (allowMoveClick) {
				var gameCam = that.game.getCamera();
				var cooClick = utils.cameraToAbsolute({	x:e.clientX, y:e.clientY}, gameCam._position);

				var cooClick2 = utils.stdToAbsolute({	x:e.clientX, y:e.clientY}, gameCam._position);
				that.game._playerShip.moveTo({x:cooClick2.x, y:cooClick2.y});

	        	socket.emit('move', {player: 0, x:cooClick2.x, y:cooClick2.y});
			}
		});
	}
	c.loadGameData = function() {
		this.game = new phobos.Game();
	}
	c.startGame = function() {
		this.game.startUpdate();
	}
	c.endUniverse = function(universe) {
		this.game.stopUpdate();
	}

	c.loadServerPlayer = function(playerLocation) {

	}

	c.getGame = function() {
		return this.game ;
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
