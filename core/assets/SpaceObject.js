this.phobos = this.phobos || {};


(function () {

	var SpaceObject = Class.create({

// static public properties:
	

// constructor:
	initialize: function (params) {
		console.log("load space object");
		console.log(params);
		this.id = params.id;
		this.index = params.id;
		this._position = {
			x: params.position.x,
			y: params.position.y,
			z: params.position.z,
			rotation: params.position.rotation,
			sector: params.position.sector,
		}
		if (!server) {
			if (params.image.animation) {
				console.log("load animation !!");
				console.log(params.image);
				this.sprite = new _.BitmapAnimation();
				this.sprite.isAnimation = true; 
				this.loadAnimation(params.image.src, params.image.spritesheet);
			}
			else {
				this.sprite = new _.Bitmap()
				this.sprite.isAnimation = false; 
				this.load(params.image.src);
			}
		}
	},

// public methods:
	tick: function (event) {
		if (!server) this.drawRender();
	},


	rotationFrame: function() {
		// this.gotoAndPlay("walk");
		console.log("rotation frame");
		console.log(this.getPosition().rotation);
		console.log(this.getSprite().currentAnimationFrame);
		if (this.getPosition().rotation % 360 > 0) 
			this.getSprite().currentAnimationFrame = Math.abs((Math.round(((360 - this.getPosition().rotation ) % 360) / 5)));
		else
			this.getSprite().currentAnimationFrame = Math.abs((Math.round((this.getPosition().rotation % 360) / 5)));

	},

	drawAnimation: function() {
		this.rotationFrame();
	},

	drawRender: function() {
		if (this.getisAnimation()) 
			this.drawAnimation();
		var renderCoo = utils.absoluteToStd({x:this.getPosition().x,y:this.getPosition().x}, client.getGame().getCamera().getPosition());
		this.sprite.x = renderCoo.x;
		this.sprite.y = renderCoo.y;
	},


	load: function(src){
		var imgSprite = new Image(); 
		var objSprite = this.sprite
		imgSprite.src = this.path+src; 
		imgSprite.onload = function() {
			objSprite.image = imgSprite
			objSprite.addEventListener("mouseover", function(e) {
				ui.showEntityInfos(objSprite);
			});
			objSprite.addEventListener("mouseout", function(e) {
				ui.hideEntityInfos(objSprite);
			});
			objSprite.addEventListener("click", function(e){
				client.inputPlayer("mouse1Object",{ click:e, targObject: objSprite})
			});
			cPlayground.addChild(objSprite);
		}
	},


	loadAnimation: function(src, spritesheet){
		var imgSprite = new Image(); 
		var objSprite = this.sprite
		imgSprite.src = this.path+src; 
		imgSprite.onload = function() {

			console.log("SPRRIIIITE SHEET");
			console.log(spritesheet);
			var spriteSheet = new _.SpriteSheet({ images: [imgSprite], frames: spritesheet.frames, animations: spritesheet.animations});
			//that.image = this;
			objSprite.scaleX = 0.5;
			objSprite.scaleY = 0.5;
			objSprite.spriteSheet = spriteSheet;
			objSprite.gotoAndStop("walk");

			objSprite.addEventListener("mouseover", function(e) {
				ui.showEntityInfos(objSprite);
			});
			objSprite.addEventListener("mouseout", function(e) {
				ui.hideEntityInfos(objSprite);
			});
			objSprite.addEventListener("click", function(e){
				client.inputPlayer("mouse1Object",{ click:e, targObject: objSprite})
			});
			console.log("ADD ANIMATION")
			console.log(objSprite);
			cPlayground.addChild(objSprite);
		}
	},

	hide: function() {
		this.getSprite().visible = false;
	},

	/**
	*	Returns data for this object that is shared within the whole network. 
	*	Use this to send this object via a socket or to the database. 
	*/
	getExport: function() {
		return ({
			id: this._id,
			position: this._position,
			type: this._type,
			actions: this._actions,
		})
	},

	getSprite: function() {
		return this.sprite; 
	},

	getPosition: function() {
		return this._position;
	},

	getSector: function() {
		return this.getPosition().sector; 
	},

	getId: function() {
		return this.id; 
	},

	getisAnimation: function() {
		return this.getSprite().isAnimation;
	},

	manageClick: function(){
		
	},
	});
	phobos.SpaceObject = SpaceObject;

}());
