

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

var Cooldown = Class.create({

	initialize: function () {
		this._time = 750;
		this._date = new Date();
		this._ready = false ; 
	},

// public methods:

	try: function () {
		var current = new Date();
		var interval = new Date();
		interval.setTime(current.getTime() - this._date.getTime()); 
		if (interval.getMilliseconds() >= this._time) {
			this._ready = true ; 
		}
	},

	start: function() {
		this._date = new Date();
		this._ready = false ; 
	},

	tick: function() {
		if (!this._ready) {
			this.try();
		}
	},
});
	phobos.Cooldown = Cooldown;

}());



(function () {

var Weapon = Class.create({
		initialize: function(weaponId) {
			this._id = weaponId;
			this._power = 20;
			this._range = 350;
			this._weaponId = weaponId ;
			this._cooldown = new phobos.Cooldown();
		},

		tick: function(event) {
			// this._cooldown.try();
			// this.setReady(this._cooldown._ready);
			// if (Math.random() < 0.05) this._ready = true;
			// else this._ready = false ; 
		},

		doShoot: function(target, shooterPos) {
			if (!server)
				client.getGame()._gameGraphics.drawLaser(shooterPos, target, this._weaponId);
			this._ready = false ; 
			this._cooldown.start();
		},
		
		getRange: function() {
			return this._range ; 
		},

		isReady: function() {
			return this._ready;
		},

		setReady: function (newReady) {
			this._ready = newReady; 
		},

	});

	phobos.Weapon = Weapon;
}());

var theweap = new phobos.Weapon ; 
console.log(theweap.getRange());

// (function () {

// 	var SpaceObject = Class.create({
// 		initialize: function(params) {
// 			this.x = params.x;
// 			this.y = params.y;
// 		},

// 		tick: function() {
// 			this.x += 5; 
// 			this.y += 1.5;
// 			this.draw();
// 		},

// 		draw: function() {
// 			console.log("DRAW");
// 			console.log(this.x + " ; " + this.y);
// 		},

// 	});


// 	phobos.SpaceObject = SpaceObject;

// 	var Ship = Class.create(SpaceObject, {
// 		initialize: function($super, params) {
// 			$super(params);
// 			this.name = params.name
// 		},

// 		tick: function($super) {
// 			$super();
// 			console.log("I AM A SHIP " + this.name);
// 			this.x += 5; 
// 		},

// 	});

// 	var Bot = Class.create(Ship, {
// 		initialize: function($super, params) {
// 			$super(params);
// 			this.IA = params.IA;
// 			this.name = params.name;
// 			console.log("bot initialized")
// 		},

// 		tick: function($super) {
// 			console.log("bot");
// 			$super();
// 			console.log("I AM A BOT !" + this.IA);
// 			this.x += 5; 
// 		},

// 	});


// 	phobos.SpaceObject = SpaceObject;
// 	phobos.Ship = Ship;
// 	phobos.Bot = Bot;


// }());


// var larme = new phobos.SpaceObject({x:-50, y:50}); 
// var larme2 = new phobos.SpaceObject({x:-150, y:200}); 
// var ship = new phobos.Ship({x:-250000, y:300, name:"tristan"}); 
// var bot = new phobos.Bot({x:-6551250000, y:300, name:"lebot", IA:"caca"}); 

// larme.tick();
// larme.tick();
// larme.tick();
// larme2.tick();
// ship.tick();
// bot.tick();