
this.phobos = this.phobos || {};


(function () {

	var Ship = Class.create(phobos.SpaceObject, {

// static public properties:
	

// constructor:
	initialize: function ($super, params) {
		this.local = {};
		if (server) { 
			// this.local.env = server;
			// this.local = {
			// 	env: server,
			// }
		}
		else { 
			this.local = {
				env: client,
				isPlayerShip: false,
				diffDrawCooForCamera: {x: 0, y:0},
				drawCoo : {x:null, y: null},
			}

			if (this.isPlayerShip) {
				this.local.isPlayerShip = true;;
			}
		}

		$super(params);

		this.shared = {
			id: params.id,
			position: {x:params.position.x, y:params.position.y, z:params.position.z, rotation: params.position.rotation, sector: params.position.sector},
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
			cargo: params.cargo,
			status:"space",
		}
		console.log("loaded ship");
		console.log(this);
	},

// public methods:

	moveTo: function (destination) {
		if (this.shared.status != "docked") {
			this.setHasTarget(false);
			this.cancelDock() ; 
			this.setDestination({x:destination.x, y:destination.y});
		}
	},

	dockTo: function(dockStation) {
		var newDestination = {
			x: dockStation.position.x + dockStation.dimensions.width / 2, 
			y: dockStation.position.y + dockStation.dimensions.height / 2
		}
		this.moveTo(newDestination);
		this.shared.dockingTarget = dockStation;
	},

	cancelDock: function() {
		this.shared.dockingTarget = null;
	},

	rotate: function (rotation) {
		this.shared.position.rotation += rotation;
	},

	throttleBrake: function (speed) {
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
	},

	stop: function () {
		this.shared.currentSpeed = 0 ; 
		this.shared.destination.x = this.shared.position.x ; 
		this.shared.destination.y = this.shared.position.y;
	},

	setLimitSpeed: function (newLimitSpeed) {
		this.shared.limitSpeed = newLimitSpeed ; 
	},

	setDestination: function (newDestination) { 
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
	},

	setHasDestination: function (newSetHasDestination) {
		this.shared.hasDestination = newSetHasDestination; 
	},

	setRotationSpeed: function (newRotationSpeed) {
		this.shared.rotationSpeed = newRotationSpeed;
	},

	setName: function (newName) {
		this.name = newName;
	},

	setMapCoords: function(newMapCoo){
		this.shared.position.x = newMapCoo.x;
		this.shared.position.y = newMapCoo.y;
	},

	setTargetType: function(newTargetType) {
		this.shared.targetType = newTargetType;
	},

	getDiffDestinationPosition: function(destination) {
		if (!destination) destination = this.shared.destination ; 
		return ({dX : (destination.x - this.shared.position.x), dY : (destination.y - this.shared.position.y), dRotation: (destination.rotation % 360 - this.shared.position.rotation % 360)});
	},

	getDiffAngle: function(diffPosDest) {
		var dX = diffPosDest.dX;
		var dY = diffPosDest.dY;
		var diffAngle ; 
		var offset = 90 ; 
		if (dX > 0) 
			diffAngle = Math.asin(dY / Math.sqrt((dX * dX + dY * dY))) * (180 / Math.PI) - offset ; 
		else if (dX <= 0) 
			diffAngle = offset - Math.asin(dY / Math.sqrt((dX * dX + dY * dY))) * (180 / Math.PI);
		return diffAngle;
	},

	rotateToDestination: function(diffPosDest) {
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
	},

	idleBehavior: function() {
		this.shared.currentSpeed = 0 ; 
	},

	moveToDestinationMovement: function() {
		var diffPosDest = this.getDiffDestinationPosition();
		if (Math.abs(diffPosDest.dX) != 0 && Math.abs(diffPosDest.dY) != 0) {
			this.shared.destination.rotation = this.getDiffAngle(diffPosDest); 
		} 
		if (Math.abs(diffPosDest.dRotation) > this.getRotationSpeed()) {
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
	},


	setHasTarget: function(newHasTarget) {
		this.shared.hasTarget = newHasTarget;
	},
	setTargetId: function(newTargetId) {
		this.shared.targetId = newTargetId;
	},

	setEnergy: function(newEnergy) {
		this.shared.energy = newEnergy;
	},

	die: function() {
		this.shared.position.z = -1;
		this.local.env.getGame().switchPlayerToKilled(this);
		this.visible = false;
		return -1;
	},

	receiveDamage: function (power) {
		this.setEnergy(this.shared.energy - power);
		if (this.isPlayerShip()) {
			if (!this.local.env.getGame().getCamera().getVibration() && Math.random() < 0.5)
				this.local.env.getGame().getCamera().setVibration(true);
		}
		if (this.getEnergy() <= 0) {
			return this.die(); 
		}
		else return this.energy;
	},

	lookAt: function (coo) {
		var diffPosDest = this.getDiffDestinationPosition({ x:coo.x, y:coo.y}); 
		var destRotation = this.getDiffAngle(diffPosDest); 
		this.setDestination({ x:this.shared.position.x, y:this.shared.position.y, rotation: destRotation } );
	},

	shootAt: function(target, weapon) {
		weapon.doShoot(target, this.getPositionDraw());
		var attackResult = target.receiveDamage(weapon._power);


		return attackResult;
	},

	dockingMovement: function() {
		var dockPosition = {
			position: {
			x: this.shared.dockingTarget.position.x + this.shared.dockingTarget.dimensions.width / 2, 
			y: this.shared.dockingTarget.position.y + this.shared.dockingTarget.dimensions.height / 2
			}
		}
		if (utils.distance(dockPosition, this.getShared()) < 100) {
			this.doDock();
		}
	},

	doDock: function() {
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
	},

	attackBehavior: function(target) {
		var targetRange = utils.distance(target, this);
		if (targetRange <= this.shared.weapons.getRange()) {
			this.lookAt({x:target.getPosition().x, y:target.getPosition().y} );
			this.stop();
			if (this.shared.weapons.isReady()) {
				var attackResult = this.shootAt(target, this.shared.weapons); 
				if (attackResult == -1) {
					this.setHasTarget(false) ;
					this.setTargetId (null) ;  
					this.stop();
				}
			}
		}
		else {
			this.setDestination({ x:target.getPosition().x, y:target.getPosition().y} );
		}
	},

	collect: function(collectable) {
		this.getCargo().capacity -= collectable.shared.weight;
		this.local.env.getGame().switchObjectToCargo(this.getShared(), collectable);
		this.getCargo().content[collectable.id] = collectable.getShared();
	},

	collectBehavior: function(collectable) {
		var targetRange = utils.distance(collectable, this);
		if (targetRange <= 100) {
			this.lookAt({x:collectable.getPosition().x, y:collectable.getPosition().y} );
			this.stop();
			if (this.getCargo().capacity >= collectable.shared.weight) {
				//Pick up item
				this.collect(collectable);
			}
		}
		else {
			this.setDestination({ x:collectable.getPosition().x, y:collectable.getPosition().y} );
		}
	},

	behavior: function () {
		if (this.getHasTarget()) {
			var targetType = this.getTargetType();
			if (targetType == "ship") 
				var currentTarget = this.getSectorShip(this.getTargetId());
			else if (targetType == "bot" || targetType == "collectable") 
				var currentTarget = this.local.env.getGame().getObjects()[this.getTargetId()];

			if (currentTarget) {
				if (targetType == "bot" || targetType == "ship") { //Attack behavior
					this.attackBehavior(currentTarget);
				}
				else if (targetType == "collectable") { //Pickup collectable
					this.collectBehavior(currentTarget);
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
	},

	weaponsTick: function() {
		this.getWeapons().tick();
	},

	tickMovement: function () {
		//Throttle. 
		//s.position.rotation += 1 ;
		if (this.getPosition().rotation >= 180) this.getPosition().rotation = -180 + this.getPosition().rotation % 180 ;
		if (this.getPosition().rotation <= -180) this.getPosition().rotation = 180 - this.getPosition().rotation % 180 ;  
		//s.position.rotation = s.position.rotation % 360 ; 
		this.getPosition().x += Math.sin((this.getPosition().rotation)*(Math.PI/-180)) * this.shared.currentSpeed;
		this.getPosition().y += Math.cos((this.getPosition().rotation)*(Math.PI/-180)) * this.shared.currentSpeed;
	},

	rotationFrame: function() {
		// this.gotoAndPlay("walk");
		if (this.getPosition().rotation % 360 > 0) 
			this.currentAnimationFrame = Math.abs((Math.round(((360 - this.getPosition().rotation ) % 360) / 5)));
		else
			this.currentAnimationFrame = Math.abs((Math.round((this.getPosition().rotation % 360) / 5)));

	},

	getCloseEnnemy: function() {
		var minDistance = 999999999999999;
		var closeEnnemyKey = null;
		for (key in this.getSectorShips()) {
			if (String((key)) === key && this.getSectorShips().hasOwnProperty(key)) {
				if (utils.distance(this.getSectorShip(key).shared, this.shared) < minDistance && this.getSectorShip(key).shared != this.shared) {
					minDistance = utils.distance(this.getSectorShip(key).shared, this.shared);
					closeEnnemyKey = key;
				}
			}
		}
		return this.getSectorShip(closeEnnemyKey);
	},
	isPlayerShip: function() {
		return (!server && client.getGame().getPlayerShip().id == this.id);
	},

	drawRender: function () {
		this.rotationFrame();

		if (this.local.env.getGame().getCamera().getCenteredOnPlayer())
		{
			var renderCoo = utils.stdToIsometricScreen(this.shared.position);

			this.local.drawCoo.x = renderCoo.x;
			this.local.drawCoo.y = renderCoo.y;

			this.x = renderCoo.x;
			this.y = renderCoo.y;

			this.x -= this.local.env.getGame().getCamera()._position.x;
			this.y -= this.local.env.getGame().getCamera()._position.y;	
		}
		else {
			var renderCoo = utils.absoluteToStd(this.shared.position, this.local.env.getGame().getCamera()._position);

			this.x = renderCoo.x;
			this.y = renderCoo.y;

		}
		// if (this.isPlayerShip()) {
		// 	// this.local.diffDrawCooForCamera.dX = this.x - renderCoo.x;
		// 	// this.local.diffDrawCooForCamera.dY = this.y - renderCoo.y;
		// 	var renderCoo = utils.absoluteToStd(this.shared.position, this.local.env.getGame().getCamera()._position);
		// }
		// else 
		// 	


		// this.x = renderCoo.x;
		// this.y = renderCoo.y;
	},

	/**
	*	Returns data for this object that is shared within the whole network. 
	*	Use this to send this object via a socket or to the database. 
	*/
	getExport: function() {
		return ({
			id: params.id,
			position: this.position,
			destination: this.destination,
			limitSpeed: this.limitSpeed,
			acceleration:this.acceleration , 
			limitRotation:this.limitRotation,
			weapons: this.weapons.getExport(),
			currentSpeed: this.currentSpeed , 
			rotationSpeed: this.rotationSpeed,
			hasDestination: this.hasDestination,
			name: this.name,
			hasTarget: this.hasTarget , 
			energy: this.energy,
			targetType: this.targetType,
			targetId: this.targetId,
			status:this.status,
		})
	},

	tick: function (event) {
		this.shared.weapons.tick() ; 
		this.behavior();
		this.tickMovement(); 
		if (!server)
			this.drawRender();
	},

	// load: function(shipData){
	// 	this.index = shipData.id; 
	// 	if (!server) {
	// 		var imgShip = new Image(); 

	// 		shipData.src  = "Hercule/SpriteHercules.png";

	// 		imgShip.src = Sh.path + shipData.src;
	// 		var that = this;
	// 		imgShip.onload: function() {
	// 			var shipSpriteSheet = new _.SpriteSheet({
	// 				// image to use
	// 				images: [this], 
	// 				frames: {width: 294, height: 266, regX: 293 / 2, regY: 266 / 2, vX:0.5, currentAnimationFrame: 15}, 
	// 				// width, height & registration point of each sprite
	// 				animations: {    
	// 					walk: [0, 70, "walk"]
	// 				}
	// 			});
	// 			that.index = shipData.id; 
	// 			//that.image = this;
	// 			that.spriteSheet = shipSpriteSheet;
	// 			that.gotoAndStop("walk");
	// 			that.scaleX = 0.45;
	// 			that.scaleY = 0.45; 
	// 			that.name = shipData.name; 
	// 			cPlayground.addChild(that);
	// 			cPlayground.update();//Create a Shape DisplayObject.
	// 		}
	// 	}
	// }


	getWeapons: function() {
		return this.shared.weapons;
	},

	getShared: function() {
		return this.shared;
	},

	getEnergy: function() {
		return this.shared.energy;
	},

	getId: function() {
		return this.id;
	},

	getPosition: function() {
		return this.shared.position;
	},

	getTargetId: function() {
		return this.shared.targetId;
	},
	
	getHasTarget: function() {
		return this.shared.hasTarget;
	},

	getStatus: function() {
		return this.shared.status;
	},

	getTargetType: function() {
		return this.shared.targetType;
	},

	getDockingTarget: function() {
		return this.shared.dockingTarget;
	},

	getSectorShip: function(shipId) {
		return this.getSectorShips()[shipId];
	},

	getSectorShips: function() {
		return this.local.env.getGame().getUniverse[this.getSector()].ships;
	},

	getSector: function() {
		return this.getPosition().sector;
	},
	
	getPositionDraw: function() {
		return {x:this.x, y:this.y};
	},

	getCargo: function() {
		return this.shared.cargo;
	},

	getRotationSpeed: function() {
		return this.shared.rotationSpeed;
	},

	});
	phobos.Ship = Ship;

}());


