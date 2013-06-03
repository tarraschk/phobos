
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

    //a local queue of messages we delay if faking latency
s.messages = [];
// constructor:

	s.initialize = function () {
	}

	
// public methods:
	s.log = function() {

	}
	s.onMessage = function() {
		
	}
	s.onInput = function() {
		
	}
	s.startUniverse = function(universe) {
		
	}
	s.endUniverse = function(universe) {
		
	}
	phobos.Server = Server;

}());
