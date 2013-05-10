(function (window) {

	Ship = function(params){
		this.initialize(params);
	}

	var s = Ship.prototype = new _.BitmapAnimation();

// static public properties:
	Ship.path = 'img/ship/';
	
// public properties:
	s.position = {x:null, y:null, rotation: 90};
	s.destination = {x:null, y:null};
	s.limitSpeed = 3;
	s.limitRotation;
	s.currentSpeed = 1 ; 
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
		debug ("New destination :  " + newDestination.x + " ; " + newDestination.y)
		s.destination = {
			x: newDestination.x,
			y: newDestination.y
		}
		var diffPosDest = this.getDiffDestinationPosition(); 
		s.destination.rotation = this.getDiffAngle(diffPosDest); 
		// console.log(s.destination.rotation);
		s.position.rotation = s.destination.rotation ;
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
		var dX = diffPosDest.dX;
		var dY = diffPosDest.dY;
		var diffAngle ; 
		var offset = 90 ; 
		if (dX > 0) 
			diffAngle = Math.asin(dY / Math.sqrt((dX * dX + dY * dY))) * (180 / Math.PI) - offset ; 
		else if (dX <= 0) 
			diffAngle = offset - Math.asin(dY / Math.sqrt((dX * dX + dY * dY))) * (180 / Math.PI);
		// console.log("diff Angle : ");
		//if (diffAngle < 0) diffAngle = - diffAngle ; 
		//else diffAngle += 180 ; 
		// console.log(diffAngle);

		return diffAngle;
	}

	s.behavior = function () {
		if (s.hasDestination) {
			var diffPosDest = this.getDiffDestinationPosition();
			if (Math.abs(diffPosDest.dX) < 5 && Math.abs(diffPosDest.dY) < 5) 
				s.stop() ; 
		}
		else {

		}
	}

	s.tickMovement = function () {
		//Throttle. 
		//s.position.rotation += 1 ;
		this.position.x += Math.sin((this.position.rotation)*(Math.PI/-180)) * this.currentSpeed;
		this.position.y += Math.cos((this.position.rotation)*(Math.PI/-180)) * this.currentSpeed;
	}

	s.rotationFrame = function() {
		this.gotoAndPlay("walk");
		// console.log(this.position.rotation % 360 ); 
		if (this.position.rotation % 360 > 0) 
			this.currentAnimationFrame = Math.abs((Math.round(((360 - this.position.rotation ) % 360) / 12)));
		else
			this.currentAnimationFrame = Math.abs((Math.round((this.position.rotation % 360) / 12)));

	}

	s.drawRender = function () {
		s.rotationFrame();
		//s.x = this.position.x - game._camera.x();
		//s.y = this.position.y - game._camera.y();
		var renderCoo = utils.absoluteToStd(this.position, game._camera._position);
		s.x = renderCoo.x;
		s.y = renderCoo.y;
		//s.isometricConversion(); 
		//s.x = this.position.x - game._camera.x();
		//s.y = this.position.y - game._camera.y();
	}

	s.tick = function (event) {
		this.behavior();
		this.tickMovement(); 
		this.drawRender();
	}

	s.load = function(shipData){
		// console.log(shipData);
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
			s.scaleX = 0.4;
			s.scaleY = 0.4; 
			s.name = shipData.name; 
			// console.log(s);
			cPlayground.addChild(s);
			cPlayground.update();//Create a Shape DisplayObject.
			// console.log("new ship")
		}
	}
	window.Ship = Ship;

}(window));
