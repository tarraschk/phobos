(function (window) {
	console.log(Ship)
	Bot = function(params){
		this.initialize(params);
	}

	var b = Bot.prototype = new Ship();
	
// static public properties:
	Bot.path = 'img/ship/';

// public properties:
	b.hasTarget = false;
// constructor:
	b.Container_initialize = b.initialize;
	b.initialize = function (params) {
		this.Container_initialize(params);
	}
	b.checkForTarget = function(){

	}

// public methods:
	window.Bot = Bot;

}(window));