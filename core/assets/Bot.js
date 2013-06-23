
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

	s.id;
	s.index ; 
	s.shared = {};
	s.local = {
		env: null,
	}
// constructor:
	s.initialize = function (params) {
		if (params) {
			this.id = params.id;
			this.index = this.id ; 
			this.shared = {
				position: {x:params.x, y:params.y, z:1, rotation: 90},
				initPosition: {x:params.x, y:params.y, z:params.z, rotation: 90},
				destination: {x:null, y:null},
				limitSpeed: 1.5,
				acceleration: 0.06 , 
				limitRotation:0,
				currentSpeed: 0 , 
				rotationSpeed: 3,
				hasDestination: false,
				weapons: new Weapon(2),
				hasTarget: false , 
				energy: 100,
				targetId: null,
				AIStopRange: 600 , 
				AIRange: 500,
				AI:"wait",
				name:null,
			}
			if (server) this.local.env = server;
			else this.local.env = client;
			// this.setMapCoords({x: params.x, y: params.y});
			this.load(params);
		}
	}

// public methods:
	
	s.moveTo = function (destination) {
		this.setHasTarget(false);
		this.setDestination({x:destination.x, y:destination.y});
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

	s.setHasDestination = function (newSetHasDestination) {
		this.shared.hasDestination = newSetHasDestination; 
	}

	s.setRotationSpeed = function (newRotationSpeed) {
		this.shared.rotationSpeed = newRotationSpeed;
	}

	s.setName = function (newName) {
		this.shared.name = newName;
	}

	s.setMapCoords = function(newMapCoo){
		this.shared.position.x = newMapCoo.x;
		this.shared.position.y = newMapCoo.y;
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

	s.idleMovement = function() {
		this.shared.currentSpeed = 0 ; 
	}
	
	s.setHasTarget = function(newHasTarget) {
		this.shared.hasTarget = newHasTarget;
	}
	s.setTargetId = function(newTargetId) {
		this.shared.targetId = newTargetId;
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

	s.moveToDestinationMovement = function() {
		var diffPosDest = this.getDiffDestinationPosition();
		if (Math.abs(diffPosDest.dX) != 0 && Math.abs(diffPosDest.dY) != 0) {
			this.shared.destination.rotation = this.getDiffAngle(diffPosDest); 
		} 
		if (Math.abs(diffPosDest.dRotation) > 2) {
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

	s.setEnergy = function(newEnergy) {
		this.shared.energy = newEnergy;
	}

	s.die = function() {
		debug("dead");
		this.shared.position.z = -1;
		this.local.env.getGame().switchPlayerToKilled(this);
		this.visible = false;
		return -1;
	}

	s.receiveDamage = function (power) {
		this.setEnergy(this.shared.energy - power);
		if (this.shared.energy <= 0) {
			return this.die(); 
		}
		else return this.shared.energy;
	}

	s.lookAt = function (coo) {
		var diffPosDest = this.getDiffDestinationPosition({ x:coo.x, y:coo.y}); 
		var destRotation = this.getDiffAngle(diffPosDest); 
		this.setDestination({ x:this.shared.position.x, y:this.shared.position.y, rotation: destRotation } );
	}

	s.shootAt = function(target, weapon) {
		weapon.doShoot(target);
		var attackResult = target.receiveDamage(weapon._power);
		return attackResult;
	}

	s.behavior = function () {
		var that = this ; 
		if (this.getHasTarget()) {
			var currentTarget = this.local.env.getGame()._shipsList[this.getTargetId()];
			var targetRange = utils.distance(currentTarget.shared, this.shared);
			if (targetRange <= this.getWeapons().getRange()) {
				this.lookAt(currentTarget.getPosition() );
				this.stop();
				if (this.getWeapons().isReady()) {
					this.shootAt(currentTarget, this.getWeapons()); 
				}
			}
			else {
				this.setDestination(currentTarget.getPosition());
			}
		}
		if (this.getHasDestination) {
			console.log("MOVE OUT");
			this.moveToDestinationMovement();
		}
		else {
			this.idleMovement() ; 
		}
	}
	
	s.setAI = function(newAI) {
		this.shared.AI = newAI;
	}
	
	s.botBehavior = function() {
		switch(this.shared.AI) {
			case "wait":
			var closeTarget = this.getCloseEnnemy();
			if (closeTarget) {
				if (utils.distance(closeTarget.shared, this.shared) < this.shared.AIRange && !this.shared.hasTarget) {
					this.setTargetId(closeTarget.id);
					this.setHasTarget(true);
					this.setDestination({ x:closeTarget.getPosition().x, y:closeTarget.getPosition().y} );
					this.setAI("attack");
				}
			}
			break;
			case "attack":
				if (this.shared.hasTarget) {
					var currentTarget = this.local.env.getGame()._shipsList[this.shared.targetId];
					var targetRange = utils.distance(currentTarget.shared, this.shared);
					if (targetRange >= this.AIStopRange || !utils.isSameZ(currentTarget,this)) {
						this.setTargetId(null);
						this.setHasTarget(false);
						this.setAI("backToPositionTrigger");
					}
				}
				else this.setAI("backToPositionTrigger");
			break;
			case "backToPositionTrigger":
				this.moveTo({x:this.getInitPosition().x,y:this.getInitPosition().y} );
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

		if (this.shared.position.rotation >= 180) this.getPosition().rotation = -180 + this.getPosition().rotation % 180 ;
		if (this.getPosition().rotation <= -180) this.getPosition().rotation = 180 - this.getPosition().rotation % 180 ;  
		//s.position.rotation = s.position.rotation % 360 ; 
		this.getPosition().x += Math.sin((this.getPosition().rotation)*(Math.PI/-180)) * this.getCurrentSpeed();
		this.getPosition().y += Math.cos((this.getPosition().rotation)*(Math.PI/-180)) * this.getCurrentSpeed();
	

	}

	s.rotationFrame = function() {
		if (this.getPosition().rotation % 360 > 0) 
			this.currentAnimationFrame = Math.abs((Math.round(((360 - this.getPosition().rotation ) % 360) / 12)));
		else
			this.currentAnimationFrame = Math.abs((Math.round((this.getPosition().rotation % 360) / 12)));

	}

	s.getInitPosition = function() {
		return (this.shared.initPosition);
	}

	s.getCurrentSpeed = function() {
		return (this.getShared().currentSpeed);
	}

	s.getShared = function() {
		return (this.shared);
	}

	s.getCloseEnnemy = function() {
		var minDistance = 999999999999999;
		var closeEnnemyKey = null;
		for (key in this.local.env.getShipsList()) {
			if (String((key)) === key && this.local.env.getShipsList().hasOwnProperty(key)) {
				if (utils.distance(this.local.env.getShipsList()[key].shared, this.shared) < minDistance && this.local.env.getGame()._shipsList[key].shared != this.shared) {
					minDistance = utils.distance(this.local.env.getGame()._shipsList[key].shared, this.shared);
					closeEnnemyKey = key;
				}
			}
		}
		return this.local.env.getGame()._shipsList[closeEnnemyKey];
	}

	s.getPosition = function() {
		return this.shared.position;
	}

	s.getWeapons = function() {
		return this.shared.weapons;
	}

	s.getHasTarget = function() {
		return this.shared.hasTarget;
	}

	s.getTargetId = function() {
		return this.shared.targetId;
	}

	s.getHasDestination = function() {
		return this.shared.hasDestination;
	}

	s.drawRender = function () {
		this.rotationFrame();
		//s.x = this.position.x - this.local.env.getGame()._camera.x();
		//s.y = this.position.y - this.local.env.getGame()._camera.y();
		var renderCoo = utils.absoluteToStd(this.shared.position, this.local.env.getGame()._camera._position);
		this.x = renderCoo.x;
		this.y = renderCoo.y;
		//s.isometricConversion(); 
		//s.x = this.position.x - this.local.env.getGame()._camera.x();
		//s.y = this.position.y - this.local.env.getGame()._camera.y();
	}
	
	
	s.weaponsTick = function() {
		this.getWeapons().tick();
	}
	
	s.tick = function (event) {
		this.weaponsTick() ; 
		this.botBehavior();
		this.behavior();
		this.tickMovement(); 
		if (!server)
			this.drawRender();
	}

	s.manageClick = function() {
		allowMoveClick = false ; 
		this.local.env.getGame()._playerShip.setTargetId(this.id);
		this.local.env.getGame()._playerShip.setHasTarget(true);
		this.local.env.getGame()._playerShip.setDestination({ x:this.position.x, y: this.position.y} );
	}

	s.load = function(shipData){
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
    phobos.Bot = Sh
}());
