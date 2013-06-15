
this.phobos = this.phobos || {};

(function () {

	var Sh = function(params){
		this.initialize(params);
	}
	if (server) var s = Sh.prototype ;
	else var s = Sh.prototype = new _.BitmapAnimation();

// static public properties:
	Sh.path = 'img/ship/';

// public properties:
	s.position       = {x:0, y:0, rotation: 90};
	s.destination    = {x:null, y:null};
	s.limitSpeed ;
	s.acceleration ; 
	s.limitRotation;
	s.currentSpeed ; 
	s.rotationSpeed ;
	s.hasDestination = false;
	s.weapons        = null;
	s.hasTarget      = false ; 
	s.energy ;
	s.targetId       = null;
	s.dockingTarget  = null ;
	s.name;
// constructor:
	s.initialize = function (params) {
		if (params) {
			this.acceleration   = params.acceleration || 0.06 ;
			this.currentSpeed   = params.currentSpeed || 0 ; 
			this.destination    = params.destination || {x:5, y:5};
			this.energy         = params.energy || 500;
			this.hasDestination = params.hasDestination || false;
			this.id             = params.id;
			this.limitSpeed     = params.limitSpeed || 3.5;
			this.name           = params.name;
			this.position       = {x:0, y:0, z: 1, rotation: 90};
			this.rotationSpeed  = params.rotationSpeed || 6;
			// this.weapons        = params.weapon || new phobos.Weapon(1);
			// this.setMapCoords({x: params.position.x, y: params.position.y});
			this.limitRotation;
			this.load(params);
		}
	}

// public methods:

	s.moveTo = function (destination) {
		this.setHasTarget(false);
		this.setDestination({x:destination.x, y:destination.y});
	}

	s.dockTo = function(dockStation) {
		// var newDestination = {
		// 	x: dockStation._mapX + dockStation.image.width / 2, 
		// 	y: dockStation._mapY + dockStation.image.height / 2
		// }
		var newDestination = {
			x: dockStation._mapX + 150, 
			y: dockStation._mapY - 70
		}
		this.moveTo(newDestination);
		this.dockingTarget = dockStation;
	}

	s.rotate = function (rotation) {
		this.position.rotation += rotation;
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
		this.destination.x = this.position.x ; 
		this.destination.y = this.position.y;
	}

	s.setLimitSpeed = function (newLimitSpeed) {
		this.limitSpeed = newLimitSpeed ; 
	}

	s.setDestination = function (newDestination) { 
		if (newDestination.x != this.position.x && newDestination.y != this.position.y) {
			this.destination.x = newDestination.x;
			this.destination.y = newDestination.y;
			if (newDestination.rotation) this.destination.rotation = newDestination.rotation ; 
			var diffPosDest = this.getDiffDestinationPosition(); 
			this.destination.rotation = this.getDiffAngle(diffPosDest); 
			//s.position.rotation = s.destination.rotation ;
			this.setHasDestination(true); 
		}
		else {
			if (newDestination.rotation) this.destination.rotation = newDestination.rotation ; 
			this.setHasDestination(true); 

		}
	}

	s.setHasDestination = function (newSetHasDestination) {
		this.hasDestination = newSetHasDestination; 
	}

	s.setRotationSpeed = function (newRotationSpeed) {
		this.rotationSpeed = newRotationSpeed;
	}

	s.setName = function (newName) {
		this.name = newName;
	}

	s.setMapCoords = function(newMapCoo){
		this.position.x = newMapCoo.x;
		this.position.y = newMapCoo.y;
	}

	s.getDiffDestinationPosition = function(destination) {
		if (!destination) destination = this.destination ; 
		return ({dX : (destination.x - this.position.x), dY : (destination.y - this.position.y), dRotation: (destination.rotation % 360 - this.position.rotation % 360)});
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
		return diffAngle;
	}

	s.rotateToDestination = function(diffPosDest) {
		if (diffPosDest.dRotation > 0) {
			if (Math.abs(diffPosDest.dRotation) > 180) {
				this.rotate(-this.rotationSpeed);
			}
			else this.rotate(this.rotationSpeed);
		}
		else {
			if (Math.abs(diffPosDest.dRotation) > 180) {
				this.rotate(this.rotationSpeed);
			}
			else this.rotate(-this.rotationSpeed);
		}
	}

	s.idleBehavior = function() {
		this.currentSpeed = 0 ; 
	}

	s.moveToDestinationMovement = function() {
		var diffPosDest = this.getDiffDestinationPosition();
		if (Math.abs(diffPosDest.dX) != 0 && Math.abs(diffPosDest.dY) != 0) {
			this.destination.rotation = this.getDiffAngle(diffPosDest); 
		} 
		if (Math.abs(diffPosDest.dRotation) > 3) {
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
		if (Math.abs(diffPosDest.dX) < 5 && Math.abs(diffPosDest.dY) < 5 && Math.abs(diffPosDest.dRotation) == 0) {
			this.stop() ; 
		}
	}


	s.setHasTarget = function(newHasTarget) {
		this.hasTarget = newHasTarget;
	}
	s.setTargetId = function(newTargetId) {
		this.targetId = newTargetId;
	}

	s.setEnergy = function(newEnergy) {
		this.energy = newEnergy;
	}

	s.die = function() {
		debug("dead");
		this.position.z = -1;
		server.universe.switchPlayerToKilled(this);
		this.visible = false;
		return -1;
	}

	s.receiveDamage = function (power) {
		this.setEnergy(this.energy - power);
		if (this.energy <= 0) {
			return this.die(); 
		}
		else return this.energy;
	}

	s.lookAt = function (coo) {
		var diffPosDest = this.getDiffDestinationPosition({ x:coo.x, y:coo.y}); 
		var destRotation = this.getDiffAngle(diffPosDest); 
		this.setDestination({ x:this.position.x, y:this.position.y, rotation: destRotation } );
	}

	s.shootAt = function(target, weapon) {
		console.log("shoot at " + target.name);
		weapon.doShoot(target);
		var attackResult = target.receiveDamage(weapon._power);
		return attackResult;
	}

	s.dockingMovement = function() {
		var dockPosition = {position: {x:this.dockingTarget._mapX, y:this.dockingTarget._mapY } }; 
		if (utils.distance(dockPosition, this) < 200) {
			this.doDock();
		}
	}

	s.doDock = function() {
		this.position.z = this.dockingTarget._mapZ;
		server.universe.switchPlayerToStation(this);
		this.visible = false;
		debug("Docked !");
		ui.newStationElement();
	}

	s.behavior = function () {
		if (this.hasTarget) {
			var currentTarget = server.universe._shipsList[this.targetId];
			var targetRange = utils.distance(currentTarget, this);
			if (targetRange <= this.weapons.getRange()) {
				this.lookAt({x:currentTarget.position.x, y:currentTarget.position.y} );
				this.stop();
				if (this.weapons.isReady()) {
					var attackResult = this.shootAt(currentTarget, this.weapons); 
					if (attackResult == -1) {
						debug("You killed it ! ");
						this.setHasTarget(false) ;
						this.setTargetId (null) ;  
						this.stop();
					}
					else debug ("Energy left " + attackResult);
				}
			}
			else {
				this.setDestination({ x:currentTarget.position.x, y:currentTarget.position.y} );
			}
		}
		if (this.dockingTarget) {
			this.dockingMovement();
		}
		if (this.hasDestination) {
			this.moveToDestinationMovement();
		}
		else {
			this.idleBehavior() ; 
		}
	}

	s.weaponsTick = function() {
		if (this.weapons)
			this.weapons.tick();
	}

	s.tickMovement = function () {
		//Throttle. 
		//s.position.rotation += 1 ;
		if (this.position.rotation >= 180) this.position.rotation = -180 + this.position.rotation % 180 ;
		if (this.position.rotation <= -180) this.position.rotation = 180 - this.position.rotation % 180 ;  
		//s.position.rotation = s.position.rotation % 360 ; 
		this.position.x += Math.sin((this.position.rotation)*(Math.PI/-180)) * this.currentSpeed;
		this.position.y += Math.cos((this.position.rotation)*(Math.PI/-180)) * this.currentSpeed;
	}

	s.rotationFrame = function() {
		// this.gotoAndPlay("walk");
		if (this.position.rotation % 360 > 0) 
			this.currentAnimationFrame = Math.abs((Math.round(((360 - this.position.rotation ) % 360) / 12)));
		else
			this.currentAnimationFrame = Math.abs((Math.round((this.position.rotation % 360) / 12)));

	}

	s.getCloseEnnemy = function() {
		var minDistance = 999999999999999;
		var closeEnnemyKey = null;
		for (var j = 0 ; j < server.universe._shipsList.length ; j++) {
			if (utils.distance(server.universe._shipsList[j], this) < minDistance && server.universe._shipsList[j] != this) {
				minDistance = utils.distance(server.universe._shipsList[j], this);
				closeEnnemyKey = j;
			}
		}
		return server.universe._shipsList[closeEnnemyKey];
	}

	s.drawRender = function () {
		this.rotationFrame();
		var renderCoo = utils.absoluteToStd(this.position, client.getGame().getCamera()._position);
		this.x = renderCoo.x;
		this.y = renderCoo.y;
	}

	s.tick = function (event) {
		this.weaponsTick() ; 
		this.behavior();
		this.tickMovement(); 
		if (!server)
			this.drawRender();
	}

	s.load = function(shipData){
		this.index = shipData.id; 
		if (!server) {
			var imgShip = new Image(); 

			shipData.src  = "spriteShip.png";

			imgShip.src = Sh.path + shipData.src;
			var that = this;
			imgShip.onload = function() {
				var shipSpriteSheet = new _.SpriteSheet({
					// image to use
					images: [this], 
					frames: {width: 120, height: 120, regX: 60, regY: 60, vX:0.5, currentAnimationFrame: 27}, 
					// width, height & registration point of each sprite
					animations: {    
						walk: [0, 30, "walk"]
					}
				});
				that.index = shipData.id; 
				//that.image = this;
				that.spriteSheet = shipSpriteSheet;
				that.gotoAndStop("walk");
				that.x = shipData.x;
				that.y = shipData.y;
				that.scaleX = 0.4;
				that.scaleY = 0.4; 
				that.name = shipData.name; 
				cPlayground.addChild(that);
				cPlayground.update();//Create a Shape DisplayObject.

			}
		}
	}


    phobos.Ship = Sh
}());
