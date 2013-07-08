this.phobos = this.phobos || {};

(function () {

	Collectable = function(params){
		this.initialize(params);
	}

	if (server) var c = Collectable.prototype ;
	else var c = Collectable.prototype = new _.BitmapAnimation();

// static public properties:
	Station.path = 'img/objects/collectables/';
	
// public properties:
	s._id;
	s.shared = {};
	s.local = {};
// constructor:
	s.initialize = function (params) {
		this.id = params.id;
		this.index = params.id;
		this.shared = { 
			id: params.id,
			index: params.id,
			position: {x: params.position.x, y: params.position.y, z: params.position.z, rotation: params.position.rotation },
			type:"Collectable",
			name: params.name,
			actions: ["collect"],
			dimensions: params.dimensions,
			image: {
				src:params.image.src,
				dim:500 //To do
			}
		 };
		if (!server) this.load();
		if (server) this.local.env = server;
		else this.local.env = client;
	}

// public methods:
	c.getLifeInPercent = function(){
		return this._lifeLeft*100/this._life;
	}
	c.drawRender = function() {
		var renderCoo = utils.absoluteToStd({x:this.shared.position.x,y:this.shared.position.y}, this.local.env.getGame().getCamera()._position);
		this.x = renderCoo.x;
		this.y = renderCoo.y;
	}
	c.tick = function () {
		this.shared.position.x = this.shared.position.x + 0.01;
		if (!server) this.drawRender();
	}
	c.load = function(params){
		var imgShip = new Image(); 


		imgShip.src = Sh.path + params.src;
		var that = this;
		imgShip.onload = function() {
			var shipSpriteSheet = new _.SpriteSheet({
				// image to use
				images: [this], 
				frames: {width: 293, height: 266, regX: 293 / 2, regY: 266 / 2, vX:0.5, currentAnimationFrame: 15}, 
				// width, height & registration point of each sprite
				animations: {    
					walk: [0, 71, "walk"]
				}
			});
			//that.image = this;
			that.spriteSheet = shipSpriteSheet;
			that.gotoAndStop("walk");
			cPlayground.addChild(that);
			cPlayground.update();//Create a Shape DisplayObject.
		}
	}
	c.getShared = function() {
		return this.shared;
	}
	phobos.Station = Station;

}());
