(function (window) {

	Ship = function(params){
		this.initialize(params);
	}

	var s = Ship.prototype = new _.BitmapAnimation();

// static public properties:
	Ship.path = 'img/ship/';
	
// public properties:
	s.position = {x:null, y:null};
	s.destination = {x:null, y:null};
	s.limitSpeed;
	s.limitRotation;
	s.rotation;
	s.rotationSpeed;
	s.name;
// constructor:
	s.initialize = function (params) {
		this.name = params.name;
		this.setMapCoords({x: params.x, y: params.y});
		this.load(params);
	}

// public methods:
	
	s.moveTo = function (destination) {

	}

	s.rotate = function (rotation) {

	}

	s.throttleBrake = function (speed) {

	}

	s.stop = function () {

	}

	s.setLimitSpeed = function (newLimitSpeed) {

	}

	s.setDestination = function (newDestination) {

	}

	s.setRotationSpeed = function (newRotationSpeed) {

	}

	s.setName = function (newName) {

	}

	s.setMapCoords = function(newMapCoo){
		this.position.x = newMapCoo.x;
		this.position.y = newMapCoo.y;
	}

	s.tick = function (event) {
		//s.x += 5;
	}

	s.load = function(shipData){
		console.log(shipData);
		var imgShip = new Image(); 
		imgShip.src = Ship.path + shipData.src;
		var that = this;
		imgShip.onload = function() {
			var shipSpriteSheet = new _.SpriteSheet({
				// image to use
				images: [imgShip], 
				frames: {width: 120, height: 120, regX: 60, regY: 60, vX:0.5, currentAnimationFrame: 27}, 
				// width, height & registration point of each sprite
				animations: {    
					walk: [0, 30, "walk"]
				}
			});

			s.index = shipData.id; 
			s.image = imgShip;
			s.spriteSheet = shipSpriteSheet;
			s.gotoAndStop("walk");
			s.setMapCoords(shipData);
			s.x = shipData.x;
			s.y = shipData.y;
			s.name = shipData.name; 
			console.log(s);
			cPlayground.addChild(s);
			cPlayground.update();//Create a Shape DisplayObject.
			console.log("new ship")
		}
	}
	window.Ship = Ship;

}(window));