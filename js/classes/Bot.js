(function (window) {

	Bot = function(params){
		this.initialize(params); 
	}

	var b = Bot.prototype = window.Ship.prototype;

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