
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
	s.id ;
	s.shared = {};
	s.local = {
		env: null,
	}
// constructor:
	s.initialize = function (params) {
		if (params) {
			this.id = params.id;
			this.shared = {
				id: params.id,
				position: {x:params.position.x, y:params.position.y, z:params.position.z, rotation: params.position.rotation},
				destination: {x:null, y:null},
				limitSpeed: 4.5,
				acceleration: 0.06 , 
				limitRotation:0,
				weapons: new Weapon(2),
				currentSpeed: 0 , 
				rotationSpeed: 4,
				hasDestination: false,
				name: params.name,
				hasTarget: false , 
				energy: 1000,
				targetType: null,
				targetId: null,
				status:"space",
			}
			if (server) this.local.env = server;
			else this.local.env = client;
			this.load(params);
		}
	}

// public methods:

	s.moveTo = function (destination) {
		if (this.shared.status != "docked") {
			this.setHasTarget(false);
			this.cancelDock() ; 
			this.setDestination({x:destination.x, y:destination.y});
		}
	}

	s.dockTo = function(dockStation) {
		var newDestination = {
			x: dockStation.position.x + dockStation.dimensions.w / 2, 
			y: dockStation.position.y + dockStation.dimensions.h / 2
		}
		this.moveTo(newDestination);
		this.shared.dockingTarget = dockStation;
	}

	s.cancelDock = function() {
		this.shared.dockingTarget = null;
	}

	s.rotate = function (rotation) {
		this.shared.position.rotation += rotation;
	}

	s.throttleBrake = function (speed) {
		if (speed < 0) 
		{
			//Brake
			this.shared.currentSpeed = ((this.shared.currentSpeed + speed < 0) ? 0 : this.shared.currentSpeed + speed) ; 
		}
		else 
		{
			//Throttle
			this.shared.currentSpeed = ((this.shared.currentSpeed + speed > this.shared.limitSpeed) ? this.shared.limitSpeed : this.shared.currentSpeed + speed) ; 
		}
	}

	s.stop = function () {
		this.shared.currentSpeed = 0 ; 
		this.shared.destination.x = this.shared.position.x ; 
		this.shared.destination.y = this.shared.position.y;
	}

	s.setLimitSpeed = function (newLimitSpeed) {
		this.shared.limitSpeed = newLimitSpeed ; 
	}

	s.setDestination = function (newDestination) { 
		if (newDestination.x != this.shared.position.x && newDestination.y != this.shared.position.y) {
			this.shared.destination.x = newDestination.x;
			this.shared.destination.y = newDestination.y;
			if (newDestination.rotation) this.shared.destination.rotation = newDestination.rotation ; 
			var diffPosDest = this.getDiffDestinationPosition(); 
			this.shared.destination.rotation = this.getDiffAngle(diffPosDest); 
			//s.position.rotation = s.destination.rotation ;
			this.setHasDestination(true); 
		}
		else {
			if (newDestination.rotation) this.shared.destination.rotation = newDestination.rotation ; 
			this.setHasDestination(true); 

		}
	}

	s.setHasDestination = function (newSetHasDestination) {
		this.shared.hasDestination = newSetHasDestination; 
	}

	s.setRotationSpeed = function (newRotationSpeed) {
		this.shared.rotationSpeed = newRotationSpeed;
	}

	s.setName = function (newName) {
		this.name = newName;
	}

	s.setMapCoords = function(newMapCoo){
		this.shared.position.x = newMapCoo.x;
		this.shared.position.y = newMapCoo.y;
	}

	s.setTargetType = function(newTargetType) {
		this.shared.targetType = newTargetType;
	}

	s.getDiffDestinationPosition = function(destination) {
		if (!destination) destination = this.shared.destination ; 
		return ({dX : (destination.x - this.shared.position.x), dY : (destination.y - this.shared.position.y), dRotation: (destination.rotation % 360 - this.shared.position.rotation % 360)});
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
				this.rotate(-this.shared.rotationSpeed);
			}
			else this.rotate(this.shared.rotationSpeed);
		}
		else {
			if (Math.abs(diffPosDest.dRotation) > 180) {
				this.rotate(this.shared.rotationSpeed);
			}
			else this.rotate(-this.shared.rotationSpeed);
		}
	}

	s.idleBehavior = function() {
		this.shared.currentSpeed = 0 ; 
	}

	s.moveToDestinationMovement = function() {
		var diffPosDest = this.getDiffDestinationPosition();
		if (Math.abs(diffPosDest.dX) != 0 && Math.abs(diffPosDest.dY) != 0) {
			this.shared.destination.rotation = this.getDiffAngle(diffPosDest); 
		} 
		if (Math.abs(diffPosDest.dRotation) > 3) {
			this.rotateToDestination(diffPosDest);
			if (Math.abs(diffPosDest.dX) < 250 && Math.abs(diffPosDest.dY) < 250) //If target is very close, we brake.
				this.throttleBrake(-this.shared.acceleration) ; 
			else 
				this.throttleBrake(this.shared.acceleration); 
		}
		else {
			if (this.shared.currentSpeed < this.shared.limitSpeed)
			{
				this.throttleBrake(this.shared.acceleration) ; 
			}
			this.shared.position.rotation = this.shared.destination.rotation ; 
		}
		if (Math.abs(diffPosDest.dX) < 5 && Math.abs(diffPosDest.dY) < 5 && Math.abs(diffPosDest.dRotation) == 0) {
			this.stop() ; 
		}
	}


	s.setHasTarget = function(newHasTarget) {
		this.shared.hasTarget = newHasTarget;
	}
	s.setTargetId = function(newTargetId) {
		this.shared.targetId = newTargetId;
	}

	s.setEnergy = function(newEnergy) {
		this.shared.energy = newEnergy;
	}

	s.die = function() {
		this.shared.position.z = -1;
		this.local.env.getGame().switchPlayerToKilled(this);
		this.visible = false;
		return -1;
	}

	s.receiveDamage = function (power) {
		this.setEnergy(this.shared.energy - power);
		if (this.getEnergy() <= 0) {
			return this.die(); 
		}
		else return this.energy;
	}

	s.lookAt = function (coo) {
		var diffPosDest = this.getDiffDestinationPosition({ x:coo.x, y:coo.y}); 
		var destRotation = this.getDiffAngle(diffPosDest); 
		this.setDestination({ x:this.shared.position.x, y:this.shared.position.y, rotation: destRotation } );
	}

	s.shootAt = function(target, weapon) {
		weapon.doShoot(target, this.getPositionDraw());
		var attackResult = target.receiveDamage(weapon._power);
		return attackResult;
	}

	s.dockingMovement = function() {
		var dockPosition = {
			position: {
			x: this.getDockingTarget().position.x + this.getDockingTarget().dimensions.w / 2, 
			y: this.getDockingTarget().position.y + this.getDockingTarget().dimensions.h / 2
			}
		}

		if (utils.distance(dockPosition, this.getShared()) < 100) {
			this.doDock();
		}
	}

	s.doDock = function() {
		this.shared.position.z = 5//this.shared.dockingTarget._mapZ;
		this.shared.status = "docked" ; 
		this.local.env.getGame().switchPlayerToStation(this);
		this.visible = false;
		this.stop();
		if (!server) {
			ui.newStationElement();
			if (this.local.env.getGame().getPlayerShip().getId() == this.getId()) 
				allowMoveClick = false ; 
		}
	}

	s.behavior = function () {
		if (this.getHasTarget()) {
			if (this.getTargetType() == "ship") 
				var currentTarget = this.local.env.getGame()._shipsList[this.getTargetId()];
			else if (this.getTargetType() == "bot") 
				var currentTarget = this.local.env.getGame()._objectsList[this.getTargetId()];
			if (currentTarget) {
				var targetRange = utils.distance(currentTarget, this);
				if (targetRange <= this.shared.weapons.getRange()) {
					this.lookAt({x:currentTarget.getPosition().x, y:currentTarget.getPosition().y} );
					this.stop();
					if (this.shared.weapons.isReady()) {
						var attackResult = this.shootAt(currentTarget, this.shared.weapons); 
						if (attackResult == -1) {
							this.setHasTarget(false) ;
							this.setTargetId (null) ;  
							this.stop();
						}
					}
				}
				else {
					this.setDestination({ x:currentTarget.getPosition().x, y:currentTarget.getPosition().y} );
				}
			}
		}
		if (this.shared.dockingTarget) {
			this.dockingMovement();
		}
		if (this.shared.hasDestination) {
			this.moveToDestinationMovement();
		}
		else {
			this.idleBehavior() ; 
		}
	}

	s.weaponsTick = function() {
		this.getWeapons().tick();
	}

	s.tickMovement = function () {
		//Throttle. 
		//s.position.rotation += 1 ;
		if (this.getPosition().rotation >= 180) this.getPosition().rotation = -180 + this.getPosition().rotation % 180 ;
		if (this.getPosition().rotation <= -180) this.getPosition().rotation = 180 - this.getPosition().rotation % 180 ;  
		//s.position.rotation = s.position.rotation % 360 ; 
		this.getPosition().x += Math.sin((this.getPosition().rotation)*(Math.PI/-180)) * this.shared.currentSpeed;
		this.getPosition().y += Math.cos((this.getPosition().rotation)*(Math.PI/-180)) * this.shared.currentSpeed;
	}

	s.rotationFrame = function() {
		// this.gotoAndPlay("walk");
		if (this.getPosition().rotation % 360 > 0) 
			this.currentAnimationFrame = Math.abs((Math.round(((360 - this.getPosition().rotation ) % 360) / 5)));
		else
			this.currentAnimationFrame = Math.abs((Math.round((this.getPosition().rotation % 360) / 5)));

	}

	s.getCloseEnnemy = function() {
		var minDistance = 999999999999999;
		var closeEnnemyKey = null;
		for (var j = 0 ; j < this.local.env.getGame()._shipsList.length ; j++) {
			if (utils.distance(this.local.env.getGame()._shipsList[j].shared, this) < minDistance && this.local.env.getGame()._shipsList[j].shared != this) {
				minDistance = utils.distance(this.local.env.getGame()._shipsList[j].shared, this);
				closeEnnemyKey = j;
			}
		}
		return this.local.env.getGame()._shipsList[closeEnnemyKey];
	}

	s.getWeapons = function() {
		return this.shared.weapons;
	}

	s.getShared = function() {
		return this.shared;
	}

	s.getEnergy = function() {
		return this.shared.energy;
	}

	s.getId = function() {
		return this.id;
	}

	s.getPosition = function() {
		return this.shared.position;
	}

	s.getTargetId = function() {
		return this.shared.targetId;
	}
	
	s.getHasTarget = function() {
		return this.shared.hasTarget;
	}

	s.getStatus = function() {
		return this.shared.status;
	}

	s.getTargetType = function() {
		return this.shared.targetType;
	}

	s.getDockingTarget = function() {
		return this.getShared().dockingTarget;
	}

	s.getPositionDraw = function() {
		return {x:this.x, y:this.y};
	}

	s.drawRender = function () {
		this.rotationFrame();
		var renderCoo = utils.absoluteToStd(this.shared.position, this.local.env.getGame().getCamera()._position);
		this.x = renderCoo.x;
		this.y = renderCoo.y;
	}

	s.tick = function (event) {
		this.shared.weapons.tick() ; 
		this.behavior();
		this.tickMovement(); 
		if (!server)
			this.drawRender();
	}

	s.load = function(shipData){
		this.index = shipData.id; 
		if (!server) {
			var imgShip = new Image(); 

			shipData.src  = "Mantis1.png";

			imgShip.src = Sh.path + shipData.src;
			var that = this;
			imgShip.onload = function() {
				var shipSpriteSheet = new _.SpriteSheet({
					// image to use
					images: [this], 
					frames: {width: 340, height: 263, regX: 340 / 2, regY: 263 / 2, vX:0.5, currentAnimationFrame: 15}, 
					// width, height & registration point of each sprite
					animations: {    
						walk: [0, 71, "walk"]
					}
				});
				that.index = shipData.id; 
				//that.image = this;
				that.spriteSheet = shipSpriteSheet;
				that.gotoAndStop("walk");
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
