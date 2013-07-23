this.phobos = this.phobos || {};


(function () {

	var SpaceObject = Class.create({

// static public properties:
	

// constructor:
	initialize: function (params) {
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
			this.path = "img/";
			this.load(params.src);
		}
	},

// public methods:
	tick: function (event) {
		console.log("im ticking");
		// this.x = this.shared.position.x - game._camera.x();
		// this.y = this.shared.position.y - game._camera.y();
		if(this._type == 1)
			this.rotate();
		if (!server) this.drawRender();
	},

	drawRender: function() {
		var renderCoo = utils.absoluteToStd({x:this.getPosition().x,y:this.getPosition().x.y}, client.getGame().getCamera().getPosition());
		this.sprite.x = renderCoo.x;
		this.sprite.y = renderCoo.y;
	},

	rotate: function(angle){

	},

	load: function(src){
		this.sprite.image = new Image();
		this.sprite.image.src = this.path+src; 

		this.sprite.image.onload = function() {
			this.sprite.addEventListener("mouseover", function(e) {
				debug('over '+that._name);
			});
			this.sprite.addEventListener("mouseout", function(e) {
				debug('out of '+that._name);
			});
			this.sprite.addEventListener("click", function(e){
				that.manageClick();
			});
			cPlayground.addChild(this.sprite);
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

	manageClick: function(){
		
	},
	});
	phobos.SpaceObject = SpaceObject;

}());
