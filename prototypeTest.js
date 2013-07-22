

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
			console.log(x + " ; " + y);
		},

	});


	phobos.SpaceObject = SpaceObject;

// 	Weapon = function(owner, weaponId){
// 		this.initialize(owner, weaponId);
// 	}

// 	var w = Weapon.prototype ;

// // static public properties:
// 	Weapon.path = 'img/objects/stations/';
	
// // public properties:
// 	w._power = null // dommages que la station peut causer quand elle attaque;
// 	w._id = null;
// 	w._weaponId = null ; 
// 	w._range = 350 ; 
// 	w._ready = false;
// 	w._cooldown = null;
// // constructor:
// 	w.initialize = function (weaponId) {
// 		this._id = utils.generateId();
// 		this._power = 20;
// 		this._weaponId = weaponId ;
// 		this._cooldown = new phobos.Cooldown();
// 	}

// // public methods:
// 	w.tick = function (event) {
// 		this._cooldown.try();
// 		this.setReady(this._cooldown._ready);
// 		// if (Math.random() < 0.05) this._ready = true;
// 		// else this._ready = false ; 
// 	}

// 	w.doShoot = function(target, shooterPos) {
// 		if (!server)
// 			client.getGame()._gameGraphics.drawLaser(shooterPos, target, this._weaponId);
// 		this._ready = false ; 
// 		this._cooldown.start();
// 	}
	
// 	w.getRange = function() {
// 		return this._range ; 
// 	}

// 	w.isReady = function() {
// 		return this._ready;
// 	}

// 	w.setReady = function (newReady) {
// 		this._ready = newReady; 
// 	}

}());


var larme = new phobos.SpaceObject(5); 