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
		this.shared = {}; // To erase !
		this._position = {
			x: params.position.x,
			y: params.position.y,
			z: params.position.z,
			sector: params.position.sector,
		}
		if (!server) {
			this.sprite = new _.Bitmap();
			this.load(params.image.src);
		}
	},

// public methods:
	tick: function (event) {
		if (!server) this.drawRender();
	},

	drawRender: function() {
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


	getPosition: function() {
		return this._position;
	},

	getSector: function() {
		return this.getPosition().sector; 
	},

	getId: function() {
		return this.id; 
	},

	manageClick: function(){
		
	},
	});
	phobos.SpaceObject = SpaceObject;

}());
