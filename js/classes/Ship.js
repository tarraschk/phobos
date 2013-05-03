(function (window) {

	Ship = function(){
		console.log();
		this.initialize();
	}


	var ship = Ship.prototype = new _.BitmapAnimation();

// static public properties:
	Ship.path = 'img/ships/';

// public properties:
	this.mapX = 0;
	this.mapY = 0;

// constructor:
ship.Container_initialize = ship.initialize;
	ship.initialize = function () {
		shipData = {id:1, x:0,y:35};
		console.log(shipData);
		var imgShip = new Image(); 
		//imgShip.src = "img/ship/Image_01.jpge39bff3d-384c-49fe-8b2c-45f2a5d42192Large-1.jpg";
		imgShip.src = "img/ship/spriteShip.png";
		var shipSpriteSheet = new _.SpriteSheet({
			// image to use
			images: [imgShip], 
			// width, height & registration point of each sprite
			frames: {width: 120, height: 120, regX: 60, regY: 60, vX:0.5, currentAnimationFrame: 27}, 
			animations: {    
				walk: [0, 30, "walk"]
			}
		});
		ship.index = shipData.id; 
		//this.shipImg = new createjs.Shape();
		ship.spriteSheet = shipSpriteSheet;
		ship.gotoAndStop("walk");
		//this.shipImg.graphics.beginFill("red").drawRect(-15, -15, 30, 30);
		//Set position of Shape instance.
		ship.mapX = shipData.x;
		ship.mapY = shipData.y;
		ship.x = 0;
		ship.y = 5;
		ship.name = shipData.name; 
		console.log(ship);
		playground.addChild(ship);	
	}

// public methods:

	ship.tick = function (event) {
	}

	ship.load = function(src){
	}
	window.Ship = Ship;

}(window));