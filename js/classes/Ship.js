(function (window) {

	Ship = function(params){
		this.initialize(params);
	}

	var s = Ship.prototype = new _.BitmapAnimation();

// static public properties:
	Ship.path = 'img/ship/';
	
// public properties:
	s.mapX;
	s.mapY;
	s.name;
// constructor:
	s.initialize = function (params) {
		this.name = params.name;
		this.setMapCoords({x: params.x, y: params.y});
		this.load(params);
	}

// public methods:
	s.setMapCoords = function(params){
		this.mapX = params.x;
		this.mapY = params.y;
	}
	s.tick = function (event) {

	}

	s.load = function(shipData){
		console.log(shipData);
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
			s.mapX = shipData.x;
			s.mapY = shipData.y;
			s.x = shipData.x;
			s.y = shipData.y;
			s.name = shipData.name; 
			console.log(s);
			cPlayground.addChild(s);
			cPlayground.update();//Create a Shape DisplayObject.
			console.log("new ship")
		}
	}
	window.Ship = Ship;

}(window));
