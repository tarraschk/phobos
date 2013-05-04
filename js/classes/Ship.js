(function (window) {

	Ship = function(shipData){
		this.initialize(shipData);
	}


	var ship = Ship.prototype = new _.BitmapAnimation();

// static public properties:
	Ship.path = 'img/ship/';

// public properties:
	ship.id = null;
	ship.name = "caca";
	ship.mapX = 0;
	ship.mapY = 0;
	ship.speed = {x:0, y:0};
	ship.limitSpeed = 0;
	ship.rotation = 0;		
	ship.rotationSpeed = 0;
	ship.destination = null;		


// constructor:
	ship.initialize = function (shipData) {
		console.log("init");
		this.load(shipData);
	}

// public methods:
	ship.moveTo = function(destination) {

	}
	ship.stop = function() {

	}
	ship.rotate = function(rotation) {
	}

	ship.throttle = function (speed) {
	}

	ship.tick = function (event) {
		ship.mapX += 1;
		console.log(ship);
	}

	ship.draw = function (event) {
	}

	ship.load = function(shipData){
		console.log(shipData);
		var imgShip = new Image(); 
		//imgShip.src = "img/ship/Image_01.jpge39bff3d-384c-49fe-8b2c-45f2a5d42192Large-1.jpg";
		imgShip.src = Ship.path + shipData.src;

		var that = this;
		imgShip.onload = function() {
			var shipSpriteSheet = new _.SpriteSheet({
				// image to use
				images: [imgShip], 
				// width, height & registration point of each sprite
				frames: {width: 120, height: 120, regX: 60, regY: 60, vX:0.5, currentAnimationFrame: 27}, 
				animations: {    
					walk: [0, 30, "walk"]
				}
			});
			ship.index = shipData.id; 
			ship.spriteSheet = shipSpriteSheet;
			ship.gotoAndStop("walk");
			ship.mapX = shipData.x;
			ship.mapY = shipData.y;
			ship.x = 50//shipData.x;
			ship.y = 70//shipData.y;
			ship.name = shipData.name; 
			console.log(ship);
			cPlayground.addChild(ship);
			cPlayground.update();//Create a Shape DisplayObject.
			console.log("new ship");
		}
	}
	window.Ship = Ship;
}(window));