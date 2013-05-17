(function (window) {

	Bot = function(params){
		this.initialize(params);
	}

	var b = Bot.prototype = window.Ship.prototype;
	
// static public properties:
	Bot.path = 'img/ship/';

// public properties:

// constructor:


// public methods:
	b.botTick = b.tick;
	b.tick = function() {
		this.botTick();
	}

	window.Bot = Bot;

}(window));