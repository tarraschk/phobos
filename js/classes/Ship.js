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
ship.Container_initialize = ship.initialize;
	ship.initialize = function (shipData) {
		this.load(shipData);
	}

// public methods:
	ship.moveTo = function(destination) {

	}
	ship.stop = function() {

	}
	ship.rotate = function(params) {

	}
	ship.tick = function (event) {
	}

	ship.load = function(shipData){
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
			ship.x = shipData.x;
			ship.y = shipData.y;
			ship.name = shipData.name; 
			console.log(ship);
			playground.addChild(ship);
			playground.update();
			console.log("new ship");
		}
	}
	window.Ship = Ship;

}(window));