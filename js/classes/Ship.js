(function (window) {

	Ship = function(params){
		this.initialize(params);
	}

	var s = Ship.prototype = new _.BitmapAnimation();

// static public properties:
	Ship.path = 'img/ship/';
	
// public properties:
	s.position = {x:null, y:null, rotation: 0};
	s.destination = {x:null, y:null};
	s.limitSpeed = 3;
	s.limitRotation;
	s.currentSpeed = 0 ; 
	s.rotationSpeed;
	s.hasDestination = false;
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
		s.currentSpeed = 0 ; 
		s.destination = null ; 
		s.setHasDestination(false);
	}

	s.setLimitSpeed = function (newLimitSpeed) {

	}

	s.setDestination = function (newDestination) {
		console.log (newDestination.x);
		console.log (newDestination.y);  
		s.destination = {
			x: newDestination.x,
			y: newDestination.y
		}
		var diffPosDest = this.getDiffDestinationPosition(); 
		s.destination.rotation = this.getDiffAngle(diffPosDest); 
		s.position.rotation = s.destination.rotation;
		s.setHasDestination(true); 
		s.currentSpeed = s.limitSpeed ; 
	}
	s.setHasDestination = function (newSetHasDestination) {
		s.hasDestination = newSetHasDestination; 
	}

	s.setRotationSpeed = function (newRotationSpeed) {
		s.rotationSpeed = newRotationSpeed;
	}

	s.setName = function (newName) {
		s.name = newName;
	}

	s.setMapCoords = function(newMapCoo){
		this.position.x = newMapCoo.x;
		this.position.y = newMapCoo.y;
	}

	s.getDiffDestinationPosition = function() {
		return ({dX : (s.destination.x - s.position.x), dY : (s.destination.y - s.position.y)});
	}

	s.getDiffAngle = function(diffPosDest) {
		console.log(diffPosDest);
		var dX = diffPosDest.dX;
		var dY = diffPosDest.dY;
		var diffAngle ; 
		var offset = 90 ; 
		if (dX > 0) 
			diffAngle = Math.asin(dY / Math.sqrt((dX * dX + dY * dY))) * (180 / Math.PI) - offset ; 
		else if (dX <= 0) 
			diffAngle = offset - Math.asin(dY / Math.sqrt((dX * dX + dY * dY))) * (180 / Math.PI);
		console.log(diffAngle);
		return diffAngle;
	}

	s.behavior = function () {
		if (s.hasDestination) {
			var diffPosDest = this.getDiffDestinationPosition();
			if (Math.abs(diffPosDest.dX) < 1 && Math.abs(diffPosDest.dY) < 1) 
				s.stop() ; 
		}
		else {

		}
	}

	s.tickMovement = function () {
		//Throttle. 
		this.position.x += Math.sin((this.position.rotation)*(Math.PI/-180)) * this.currentSpeed;
		this.position.y += Math.cos((this.position.rotation)*(Math.PI/-180)) * this.currentSpeed;
	}

	s.drawRender = function () {
		s.x = this.position.x - game._camera.x();
		s.y = this.position.y - game._camera.y();
		//s.x = this.position.x - game._camera.x();
		//s.y = this.position.y - game._camera.y();
	}

	s.tick = function (event) {
		this.behavior();
		this.tickMovement(); 
		this.drawRender();
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