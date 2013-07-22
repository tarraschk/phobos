

var gameport = 8080,
	phobos = {};
	server = true ; 
	firebaseRecover = false;
	http = require('http'),
	prototype = require('prototype');
	
httpServer = http.createServer(function(request, response) {
	response.write('Phobos server launched');
	response.end();
});

io = require('socket.io').listen(httpServer);

httpServer.listen(gameport); 

var Person = Class.create({
  initialize: function(name) {
    this.name = name;
  },
  say: function(message) {
    return this.name + ': ' + message;
  }
});
    
// when subclassing, specify the class you want to inherit from
var Pirate = Class.create(Person, {
  // redefine the speak method
  say: function($super, message) {
    return $super(message) + ', yarr!';
  }
});
var paul = new Person('Paco');
console.log(paul.say('caca'));
var john = new Pirate('Long John');
console.log(john.say('ahoy matey'));
// -> "Long John: ahoy matey, yarr!"




this.phobos = this.phobos || {};


(function () {

	var SpaceObject = Class.create({
		initialize: function(params) {
			this.x = params.x;
			this.y = params.y;
		},

		tick: function() {
			this.x += 5; 
			this.y += 1.5;
			this.draw();
		},

		draw: function() {
			console.log("DRAW");
			console.log(this.x + " ; " + this.y);
		},

	});


	phobos.SpaceObject = SpaceObject;

	var Ship = Class.create(SpaceObject, {
		initialize: function($super, params) {
			$super(params);
			this.name = params.name
		},

		tick: function($super) {
			$super();
			console.log("I AM A SHIP !");
			this.x += 5; 
		},

	});


	phobos.SpaceObject = SpaceObject;
	phobos.Ship = Ship;


}());


var larme = new phobos.SpaceObject({x:-50, y:50}); 
var larme2 = new phobos.SpaceObject({x:-150, y:200}); 
var ship = new phobos.Ship({x:-250000, y:300}); 

larme.tick();
larme.tick();
larme.tick();
larme2.tick();
ship.tick();