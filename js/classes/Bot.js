(function (window) {

	Bot = function(params){
		console.log(Bot); 

		console.log(Bot.prototype); 
		this.initialize() ; 
	}

	var b = Bot.prototype = new Ship();
	console.log(Bot.prototype); 

// static public properties:
	Bot.path = 'img/ship/';
	
// public properties:
	b.position = {x:null, y:null, rotation: 90};
	b.destination = {x:null, y:null};
	b.limitSpeed = 5.5;
	b.acceleration = 0.08 ; 
	b.limitRotation;
	b.currentSpeed = 0 ; 
	s.rotationSpeed = 3;
	b.hasDestination = false;
	b.name;
// constructor:

// public methods:
	window.Bot = Bot;

}(window));
