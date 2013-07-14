
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
				id: params.id,
				position: {x:params.position.x, y:params.position.y, z:params.position.z, rotation: params.position.rotation},
				initPosition: {x:params.position.x, y:params.position.y, z:params.position.z, rotation: params.position.rotation},
				destination: params.destination,
				limitSpeed: params.limitSpeed,
				acceleration:params.acceleration , 
				limitRotation:params.limitRotation,
				weapons: new phobos.Weapon(params.weapons),
				currentSpeed: params.currentSpeed , 
				rotationSpeed: params.rotationSpeed,
				hasDestination: params.hasDestination,
				name: params.name,
				hasTarget: params.hasTarget , 
				energy: params.energy,
				targetType: params.targetType,
				targetId: params.targetId,
				AIStopRange: params.AIStopRange , 
				AIRange: params.AIRange,
				AI:params.AI,
				type:params.type,

			}
			if (server) this.local.env = server;
			else this.local.env = client;
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

	s.idleBehavior = function() {
		this.shared.currentSpeed = 0 ; 
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
		this.shared.position.z = -1;
		this.local.env.getGame().switchObjectToDestroyed(this);
		this.visible = false;
		return -1;
	}

	s.receiveDamage = function (power) {
		this.setEnergy(this.shared.energy - power);
		if (this.getEnergy() <= 0) {
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
		weapon.doShoot(target, this.getPositionDraw());
		var attackResult = target.receiveDamage(weapon._power);
		return attackResult;
	}

	s.behavior = function () {
		if (this.getHasTarget()) {
			var currentTarget = this.local.env.getGame()._shipsList[this.getTargetId()];
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
	
	s.setAI = function(newAI) {
		this.shared.AI = newAI;
	}

	s.setBotBehavior = function (newBotBehavior, miscData) {
		if (server)  {
			server.broadcastToAllSocket('setBotBehavior', {newBehavior:newBotBehavior, bot:this.shared, data:miscData});
		}
		switch(newBotBehavior) {
			case "wait":
			break;
			case "attack":
				var closeTarget = miscData;
				this.setTargetId(closeTarget.id);
				this.setHasTarget(true);
				this.setDestination({ x:closeTarget.shared.position.x, y:closeTarget.shared.position.y} );
				this.setAI("attack");
			break;
			case "backToPosition":
				this.moveTo({x:this.getInitPosition().x,y:this.getInitPosition().y} );
				this.setTargetId(null);
				this.setHasTarget(false);
				this.setAI("wait")
			break;
		}
	}
	
	s.botBehavior = function() {
		switch(this.shared.AI) {
			case "wait":
			var closeTarget = this.getCloseEnnemy();
			if (closeTarget) {
				if (utils.distance(closeTarget.shared, this.shared) < this.shared.AIRange && !this.shared.hasTarget) {
					if (server) this.setBotBehavior("attack", closeTarget);
				}
			}
			break;
			case "attack":
				if (this.shared.hasTarget) {
					var currentTarget = this.local.env.getGame()._shipsList[this.shared.targetId];
					if (currentTarget) {
						var targetRange = utils.distance(currentTarget.shared, this.shared);
						if (targetRange >= this.shared.AIStopRange || !utils.isSameZ(currentTarget,this)) {
							if (server) this.setBotBehavior("backToPosition");
						}
					}
					else this.setBotBehavior("backToPosition");
				}
				else this.setAI("backToPositionTrigger");
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
			this.currentAnimationFrame = Math.abs((Math.round(((360 - this.getPosition().rotation ) % 360) / 5)));
		else
			this.currentAnimationFrame = Math.abs((Math.round((this.getPosition().rotation % 360) / 5)));

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
		for (key in this.local.env.getGame().getShipsList()) {
			if (String((key)) === key && this.local.env.getGame().getShipsList().hasOwnProperty(key)) {
				if (utils.distance(this.local.env.getGame().getShipsList()[key].shared, this.shared) < minDistance && this.local.env.getGame()._shipsList[key].shared != this.shared) {
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

	s.getPositionDraw = function() {
		return {x:this.x, y:this.y};
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

	s.getEnergy = function() {
		return this.shared.energy;
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

			shipData.src  = "Brood/BroodSprite.png";

			imgShip.src = Sh.path + shipData.src;
			var that = this;
			imgShip.onload = function() {
				var shipSpriteSheet = new _.SpriteSheet({
					// image to use
					images: [this], 
					frames: {width: 294, height: 266, regX: 294 / 2, regY: 266 / 2, vX:0.5, currentAnimationFrame: 15}, 
					// width, height & registration point of each sprite
					animations: {    
						walk: [0, 71, "walk"]
					}
				});
				that.spriteSheet = shipSpriteSheet;
				that.gotoAndStop("walk");
				that.x = shipData.x;
				that.y = shipData.y;
				that.scaleX = 0.4;
				that.scaleY = 0.4; 
				that.name = shipData.name; 
				cPlayground.addChild(that);
				cPlayground.update();
				
				that.addEventListener("mouseover", function(e) {
					ui.showEntityInfos(that);
				});
				that.addEventListener("mouseout", function(e) {
					ui.hideEntityInfos(that);
				});
				that.addEventListener("click", function(e){
					client.inputPlayer("mouse1TargetBot",{ click:e, target: that});
				});

			}
		}
	}
    phobos.Bot = Sh
}());
