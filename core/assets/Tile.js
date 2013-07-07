
this.phobos = this.phobos || {};

(function () {

	var Tile = function(params){
		this.initialize(params);
	}

	if (server) var t = Tile.prototype;
	else var t = Tile.prototype = new _.Bitmap();

// static public properties:
	Tile.path = 'img/tiles/';
	
// public properties:
	t._id;
	t._type;
	t._mapX;
	t._mapY;
	t._position ;
	t._fucused = false;
	t._actions ; 
	t._type ; 
// constructor:
	t.initialize = function (params) {
		console.log("NEW TILE");
		console.log(params)
		// this.shared = {}; // To erase !
		this._position = {
			x: params.x,
			y: params.y,
			z: params.z,
		}
		this.id = params.id;
		this.index = params.id;
		this.shared = {
			id: params.id,
			position: {

				x: params.x,
				y: params.y,
				z: params.z,
			},
			src: params.src

		};
		if (!server)
			this.load(params.src);
	}

// public methods:
	t.tick = function (event) {
		// this.x = this.shared.position.x - game._camera.x();
		// this.y = this.shared.position.y - game._camera.y();
		if(this._type == 1)
			this.rotate();
		if (!server) this.drawRender();
	}

	t.drawRender = function() {

		var renderCoo = utils.absoluteToStd({x:this.shared.position.x,y:this.shared.position.y}, client.getGame().getCamera()._position);
		this.x = renderCoo.x;
		this.y = renderCoo.y;
	}

	t.rotate = function(angle){

	}

	t.load = function(src){
		this.image = new Image();
		this.image.src = Tile.path+src; 
		var that = this;
		this.image.onload = function() {
			that.addEventListener("mouseover", function(e) {
				debug('over '+that._name);
				that.manageMouseOver();
			});
			that.addEventListener("mouseout", function(e) {
				debug('out of '+that._name);
				that.manageMouseOut();
			});
			that.addEventListener("click", function(e){
				that.manageClick();
			});
			cPlayground.addChild(that);
		}
	}
	t.manageMouseOver = function(){

	}
	t.manageMouseOut = function(){
		
	}

	/**
	*	Returns data for this object that is shared within the whole network. 
	*	Use this to send this object via a socket or to the database. 
	*/
	t.getExport = function() {
		return ({
			id: this._id,
			position: this._position,
			type: this._type,
			actions: this._actions,
		})
	}

	t.manageClick = function(){
		
	}
	phobos.Tile = Tile;

}());
