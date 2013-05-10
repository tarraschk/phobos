(function (window) {

	Bot = function(params){
	}

	var b = Bot.prototype = new Ship(params);
	
// static public properties:
	Bot.path = 'img/ship/';

// public properties:
	b.hasTarget = false;
// constructor:
	b.checkForTarget = function(){

	}

// public methods:
	window.Bot = Bot;

}(window));