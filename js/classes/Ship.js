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
	s.limitSpeed = 4;
	s.acceleration = 0.05 ; 
	s.limitRotation;
	s.currentSpeed = 0 ; 
	s.rotationSpeed = 3;
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
		s.position.rotation += rotation;
	}

	s.throttleBrake = function (speed) {
		if (speed < 0) 
		{
			//Brake
			this.currentSpeed = ((this.currentSpeed + speed < 0) ? 0 : this.currentSpeed + speed) ; 
		}
		else 
		{
			//Throttle
			this.currentSpeed = ((this.currentSpeed + speed > this.limitSpeed) ? this.limitSpeed : this.currentSpeed + speed) ; 
		}
	}

	s.stop = function () {
		this.currentSpeed = 0 ; 
		this.destination = null ; 
		this.setHasDestination(false);
	}

	s.setLimitSpeed = function (newLimitSpeed) {
		this.limitSpeed = newLimitSpeed ; 
	}

	s.setDestination = function (newDestination) { 
		debug ("New destination :  " + newDestination.x + " ; " + newDestination.y)
		s.destination = {
			x: newDestination.x,
			y: newDestination.y
		}
		var diffPosDest = this.getDiffDestinationPosition(); 
		s.destination.rotation = this.getDiffAngle(diffPosDest); 
		//s.position.rotation = s.destination.rotation ;
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
		return ({dX : (s.destination.x - s.position.x), dY : (s.destination.y - s.position.y), dRotation: (s.destination.rotation % 360 - s.position.rotation % 360)});
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
		//if (diffAngle < 0) diffAngle = - diffAngle ; 
		//else diffAngle += 180 ; 

		return diffAngle;
	}

	s.rotateToDestination = function(diffPosDest) {
		if (diffPosDest.dRotation > 0) {
			if (Math.abs(diffPosDest.dRotation) > 180) {
				s.rotate(-s.rotationSpeed);
			}
			else s.rotate(s.rotationSpeed);
		}
		else {
			if (Math.abs(diffPosDest.dRotation) > 180) {
				s.rotate(s.rotationSpeed);
			}
			else s.rotate(-s.rotationSpeed);
		}
	}

	s.idleBehavior = function() {
		this.currentSpeed = 0 ; 
	}

	s.moveToDestinationBehavior = function() {
		var diffPosDest = this.getDiffDestinationPosition();
		s.destination.rotation = this.getDiffAngle(diffPosDest); 
		if (Math.abs(diffPosDest.dRotation) > 2) {
			this.rotateToDestination(diffPosDest);
			if (Math.abs(diffPosDest.dX) < 250 && Math.abs(diffPosDest.dY) < 250) //If target is very close, we brake.
				this.throttleBrake(-this.acceleration) ; 
			else 
				this.throttleBrake(this.acceleration); 
		}
		else {
			if (this.currentSpeed < this.limitSpeed)
			{
				this.throttleBrake(this.acceleration) ; 
			}
			this.position.rotation = this.destination.rotation ; 
		}
		if (Math.abs(diffPosDest.dX) < 5 && Math.abs(diffPosDest.dY) < 5) 
			s.stop() ; 
	}

	s.behavior = function () {
		if (s.hasDestination) {
			this.moveToDestinationBehavior();
		}
		else {
			this.idleBehavior() ; 
		}
	}

	s.tickMovement = function () {
		//Throttle. 
		//s.position.rotation += 1 ;
		if (s.position.rotation >= 180) s.position.rotation = -180 + s.position.rotation % 180 ;
		if (s.position.rotation <= -180) s.position.rotation = 180 - s.position.rotation % 180 ;  
		//s.position.rotation = s.position.rotation % 360 ; 
		this.position.x += Math.sin((this.position.rotation)*(Math.PI/-180)) * this.currentSpeed;
		this.position.y += Math.cos((this.position.rotation)*(Math.PI/-180)) * this.currentSpeed;
	}

	s.rotationFrame = function() {
		this.gotoAndPlay("walk");
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
			cPlayground.addChild(s);
			cPlayground.update();//Create a Shape DisplayObject.
		}
	}
	window.Ship = Ship;

}(window));
