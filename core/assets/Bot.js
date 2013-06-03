
this.phobos = this.phobos || {};

(function () {

	var Sh = function(params){
		this.initialize(params);
	}

	var s = Sh.prototype ;

// static public properties:
	Sh.path = 'img/ship/';
	
// public properties:
	s.position = {x:null, y:null, rotation: 90};
	s.initPosition = {x:null, y:null, rotation: 90};
	s.destination = {x:null, y:null};
	s.limitSpeed = 1.5;
	s.acceleration = 0.06 ; 
	s.limitRotation;
	s.currentSpeed = 0 ; 
	s.rotationSpeed = 3;
	s.hasDestination = false;
	s.weapons = null;
	s.hasTarget = false ; 
	s.energy = 100;
	s.targetId = null;
	s.AI = "wait";
	s.AIStopRange = 600 ; 
	s.AIRange = 500;
	s.name;
// constructor:
	s.initialize = function (params) {
		if (params) {

			this.position = {x:null, y:null, z:1, rotation: 90};
			this.setMapCoords({x: params.x, y: params.y});
			this.initPosition = {x:this.position.x, y:this.position.y};
			this.destination = {x:null, y:null};
			this.limitSpeed = 3.5;
			this.acceleration = 0.06 ; 
			this.limitRotation;
			this.weapons = new Weapon(this, 2);
			this.currentSpeed = 0 ; 
			this.rotationSpeed = 3;
			this.id = params.id;
			this.hasDestination = false;
			this.name = params.name;
			console.log("init : ");
			console.log(params);
			this.load(params);
		}
	}

// public methods:
	
	s.moveTo = function (destination) {
		this.setHasTarget(false);
		this.setDestination({x:destination.x, y:destination.y});
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

	s.idleMovement = function() {
		this.currentSpeed = 0 ; 
	}
	
	s.setHasTarget = function(newHasTarget) {
		this.hasTarget = newHasTarget;
	}
	s.setTargetId = function(newTargetId) {
		this.targetId = newTargetId;
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

	s.moveToDestinationMovement = function() {
		var diffPosDest = this.getDiffDestinationPosition();
		if (Math.abs(diffPosDest.dX) != 0 && Math.abs(diffPosDest.dY) != 0) {
			this.destination.rotation = this.getDiffAngle(diffPosDest); 
		} 
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
		if (Math.abs(diffPosDest.dX) < 5 && Math.abs(diffPosDest.dY) < 5 && Math.abs(diffPosDest.dRotation) == 0) {
			this.stop() ; 
		}
	}

	s.setEnergy = function(newEnergy) {
		this.energy = newEnergy;
	}

	s.die = function() {
		debug("dead");
		this.position.z = -1;
		game.switchPlayerToKilled(this);
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

	s.behavior = function () {
		var that = this ; 
		if (this.hasTarget) {
			var currentTarget = game._shipsList[this.targetId];
			var targetRange = utils.distance(currentTarget, this);
			if (targetRange <= this.weapons.getRange()) {
				this.lookAt({x:currentTarget.position.x, y:currentTarget.position.y} );
				this.stop();
				if (this.weapons.isReady()) {
					this.shootAt(currentTarget, this.weapons); 
				}
			}
			else {
				this.setDestination({ x:currentTarget.position.x, y:currentTarget.position.y} );
			}
		}
		if (this.hasDestination) {
			this.moveToDestinationMovement();
		}
		else {
			this.idleMovement() ; 
		}
	}
	
	s.setAI = function(newAI) {
		this.AI = newAI;
	}
	
	s.botBehavior = function() {
		switch(this.AI) {
			case "wait":
			var closeTarget = this.getCloseEnnemy();
			if (closeTarget) {
				if (utils.distance(closeTarget, this) < this.AIRange && !this.hasTarget) {
					this.setTargetId(closeTarget.id);
					this.setHasTarget(true);
					this.setDestination({ x:closeTarget.position.x, y:closeTarget.position.y} );
					this.setAI("attack");
				}
			}
			break;
			case "attack":
				if (this.hasTarget) {
					console.log(game._shipsList);
					var currentTarget = game._shipsList[this.targetId];
					console.log("target " + currentTarget.name);
					var targetRange = utils.distance(currentTarget, this);
					if (targetRange >= this.AIStopRange || !utils.isSameZ(currentTarget,this)) {
						this.setTargetId(null);
						this.setHasTarget(false);
						this.setAI("backToPositionTrigger");
					}
				}
				else this.setAI("backToPositionTrigger");
			break;
			case "backToPositionTrigger":
				this.moveTo({x:this.initPosition.x,y:this.initPosition.y} );
				this.setAI("backToPosition")
			break;
			case "backToPosition":
				//Trigger, all is ok.
			break;
			default:
			break;
		}
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
		if (this.position.rotation % 360 > 0) 
			this.currentAnimationFrame = Math.abs((Math.round(((360 - this.position.rotation ) % 360) / 12)));
		else
			this.currentAnimationFrame = Math.abs((Math.round((this.position.rotation % 360) / 12)));

	}

	s.getCloseEnnemy = function() {
		var minDistance = 999999999999999;
		var closeEnnemyKey = null;
		for (var j = 0 ; j < game._shipsList.length ; j++) {
			if (utils.distance(game._shipsList[j], this) < minDistance && game._shipsList[j] != this) {
				minDistance = utils.distance(game._shipsList[j], this);
				closeEnnemyKey = j;
			}
		}
		return game._shipsList[closeEnnemyKey];
	}

	s.drawRender = function () {
		this.rotationFrame();
		//s.x = this.position.x - game._camera.x();
		//s.y = this.position.y - game._camera.y();
		var renderCoo = utils.absoluteToStd(this.position, game._camera._position);
		this.x = renderCoo.x;
		this.y = renderCoo.y;
		//s.isometricConversion(); 
		//s.x = this.position.x - game._camera.x();
		//s.y = this.position.y - game._camera.y();
	}
	
	
	s.weaponsTick = function() {
		this.weapons.tick();
	}
	
	s.tick = function (event) {
		this.weaponsTick() ; 
		this.botBehavior();
		this.behavior();
		this.tickMovement(); 
		// this.drawRender();
	}

	s.manageClick = function() {
		allowMoveClick = false ; 
		game._playerShip.setTargetId(this.id);
		game._playerShip.setHasTarget(true);
		game._playerShip.setDestination({ x:this.position.x, y: this.position.y} );
	}

	s.load = function(shipData){
		this.index = shipData.id; 
		this.setMapCoords(shipData);
		this.name = shipData.name; 
	}
    phobos.Bot = Sh
}());
