this.phobos = this.phobos || {};

(function () {

	Collectable = function(params){
		this.initialize(params);
	}

	if (server) var c = Collectable.prototype ;
	else var c = Collectable.prototype = new _.BitmapAnimation();

// static public properties:
	Collectable.path = 'img/objects/collectables/';
	
// public properties:
	c._id;
	c.shared = {};
	c.local = {};
// constructor:
	c.initialize = function (params) {
		console.log("NEW COLLECTABLE");
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
		if (!server) this.load(params);
		if (server) this.local.env = server;
		else this.local.env = client;
	}

// public methods:
	c.getLifeInPercent = function(){
		return this._lifeLeft*100/this._life;
	}

	c.rotationFrame = function() {
		// this.gotoAndPlay("walk");
		if (this.getPosition().rotation % 360 > 0) 
			this.currentAnimationFrame = Math.abs((Math.round(((360 - this.getPosition().rotation ) % 360) / 5)));
		else
			this.currentAnimationFrame = Math.abs((Math.round((this.getPosition().rotation % 360) / 5)));

	}

	c.drawRender = function() {

		this.rotationFrame();
		var renderCoo = utils.absoluteToStd({x:this.shared.position.x,y:this.shared.position.y}, this.local.env.getGame().getCamera()._position);
		this.x = renderCoo.x;
		this.y = renderCoo.y;
	}

	c.getPosition = function() {
		return this.shared.position;
	}

	c.tick = function () {
		this.shared.position.rotation = this.shared.position.rotation + 1.5;
		if (!server) this.drawRender();
	}
	c.load = function(params){
		var imgC = new Image(); 


		imgC.src = Collectable.path + params.image.src;
		var that = this;
		imgC.onload = function() {
			var shipSpriteSheet = new _.SpriteSheet({
				// image to use
				images: [this], 
				frames: {width: 294, height: 218, regX: 293 / 2, regY: 218 / 2, vX:0.5, currentAnimationFrame: 15}, 
				// width, height & registration point of each sprite
				animations: {    
					walk: [0, 70, "walk"]
				}
			});
			//that.image = this;
			that.scaleX = 0.2;
			that.scaleY = 0.2;
			that.spriteSheet = shipSpriteSheet;
			that.gotoAndStop("walk");
			console.log("WAAAALK");
			cPlayground.addChild(that);
			cPlayground.update();//Create a Shape DisplayObject.
		}
	}
	c.getShared = function() {
		return this.shared;
	}
	phobos.Collectable = Collectable;

}());
