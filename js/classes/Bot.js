(function (window) {

	Bot = function(params){
	}

	var b = Bot.prototype = new Ship();
	
// static public properties:
	Bot.path = 'img/ship/';

// public properties:
	b.hasTarget = false;
// constructor:
	b.tick = function() {
		
	}

	b.checkForTarget = function(){

	}

// public methods:
	window.Bot = Bot;

}(window));