
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
	c.generateGame = function() {
		this.game = new phobos.Game();
	}
	c.startGame = function() {
		this.game.startGraphicsUpdate();
	}
	c.endUniverse = function(universe) {
		this.game.stopGraphicsUpdate();
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
