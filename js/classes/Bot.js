(function (window) {

	Bot = function(params){
		this.initialize(params); 
	}

	var b = Bot.prototype = window.Ship.prototype;

// static public properties:
	Bot.path = 'img/ship/';

// public properties:
	b.position = {x:null, y:null, rotation: 90};
	b.destination = {x:null, y:null};
	b.limitSpeed = 5.5;
	b.acceleration = 0.08 ; 
	b.limitRotation;
	b.currentSpeed = 0 ; 
	b.rotationSpeed = 3;
	b.hasDestination = false;
	b.name;
// constructor:


// public methods:
	window.Bot = Bot;

}(window));