
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
	s.shared = {
		position: {x:null, y:null, rotation: 90},
		initPosition: {x:null, y:null, rotation: 90},
		destination: {x:null, y:null},
		limitSpeed: 1.5,
		acceleration: 0.06 , 
		limitRotation:0,
		currentSpeed: 0 , 
		rotationSpeed: 3,
		hasDestination: false,
		weapons: null,
		hasTarget: false , 
		energy: 100,
		targetId: null,
		name:null,
		dockingTarget:null,
	};
	s.local = {
		game: null,
	}
// constructor:
	s.initialize = function (params) {
		console.log("NEW SHIP");
		console.log(params);
		if (params) {
			this.id = params.id;
			this.shared.position = {x:null, y:null, z:1, rotation: 90};
			this.shared.initPosition = {x:this.shared.position.x, y:this.shared.position.y};
			this.shared.destination = {x:null, y:null};
			this.shared.limitSpeed = 3.5;
			this.shared.acceleration = 0.06 ; 
			this.shared.limitRotation;
			this.shared.weapons = new Weapon(this, 2);
			this.shared.currentSpeed = 0 ; 
			this.shared.rotationSpeed = 3;
			this.shared.hasDestination = false;
			this.shared.name = params.name;
			if (server) this.local.game = server;
			else this.local.game = client;
			this.setMapCoords(params.position);
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
		this.shared.dockingTarget = dockStation;
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
		console.log(newMapCoo);
		console.log(this.shared.position);
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
		this.shared.position.z = -1;
		this.local.game.getGame().switchPlayerToKilled(this);
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
		this.setDestination({ x:this.shared.position.x, y:this.shared.position.y, rotation: destRotation } );
	}

	s.shootAt = function(target, weapon) {
		console.log("shoot at " + target.name);
		weapon.doShoot(target);
		var attackResult = target.receiveDamage(weapon._power);
		return attackResult;
	}

	s.dockingMovement = function() {
		var dockPosition = {position: {x:this.shared.dockingTarget._mapX, y:this.shared.dockingTarget._mapY } }; 
		if (utils.distance(dockPosition, this) < 200) {
			this.doDock();
		}
	}

	s.doDock = function() {
		this.shared.position.z = this.shared.dockingTarget._mapZ;
		this.local.game.getGame().switchPlayerToStation(this);
		this.visible = false;
		debug("Docked !");
		ui.newStationElement();
	}

	s.behavior = function () {
		if (this.hasTarget) {
			var currentTarget = this.local.game.getGame()._shipsList[this.targetId];
			var targetRange = utils.distance(currentTarget, this);
			if (targetRange <= this.shared.weapons.getRange()) {
				this.lookAt({x:currentTarget.position.x, y:currentTarget.position.y} );
				this.stop();
				if (this.shared.weapons.isReady()) {
					var attackResult = this.shootAt(currentTarget, this.shared.weapons); 
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
		if (this.shared.weapons)
			this.shared.weapons.tick();
	}

	s.tickMovement = function () {
		//Throttle. 
		//s.position.rotation += 1 ;
		if (this.shared.position.rotation >= 180) this.shared.position.rotation = -180 + this.shared.position.rotation % 180 ;
		if (this.shared.position.rotation <= -180) this.shared.position.rotation = 180 - this.shared.position.rotation % 180 ;  
		//s.position.rotation = s.position.rotation % 360 ; 
		this.shared.position.x += Math.sin((this.shared.position.rotation)*(Math.PI/-180)) * this.shared.currentSpeed;
		this.shared.position.y += Math.cos((this.shared.position.rotation)*(Math.PI/-180)) * this.shared.currentSpeed;
	}

	s.rotationFrame = function() {
		// this.gotoAndPlay("walk");
		if (this.shared.position.rotation % 360 > 0) 
			this.currentAnimationFrame = Math.abs((Math.round(((360 - this.shared.position.rotation ) % 360) / 12)));
		else
			this.currentAnimationFrame = Math.abs((Math.round((this.shared.position.rotation % 360) / 12)));

	}

	s.getCloseEnnemy = function() {
		var minDistance = 999999999999999;
		var closeEnnemyKey = null;
		for (var j = 0 ; j < this.local.game.getGame()._shipsList.length ; j++) {
			if (utils.distance(this.local.game.getGame()._shipsList[j].shared, this) < minDistance && this.local.game.getGame()._shipsList[j].shared != this) {
				minDistance = utils.distance(this.local.game.getGame()._shipsList[j].shared, this);
				closeEnnemyKey = j;
			}
		}
		return this.local.game.getGame()._shipsList[closeEnnemyKey];
	}

	s.drawRender = function () {
		this.rotationFrame();
		var renderCoo = utils.absoluteToStd(this.shared.position, this.local.game.getGame().getCamera()._position);
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

			shipData.src  = "spriteShip.png";

			imgShip.src = Sh.path + shipData.src;
			var that = this;
			console.log("load a ship.");
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
