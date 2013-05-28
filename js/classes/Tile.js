(function (window) {

	Tile = function(params){
		this.initialize(params);
	}

	var t = Tile.prototype = new _.Bitmap();

// static public properties:
	Tile.path = 'img/tiles/';
	
// public properties:
	t._type;
	t._mapX;
	t._mapY;
	t._fucused = false;
	t._scannable;
	t._collectable;
	t._rotationSpeed; // env 3-4 deg/s
	t._id;
// constructor:
	t.initialize = function (params) {
		this.setMapCoords({x: params.x, y: params.y});
		this.load(params.src);
	}

// public methods:
	t.setMapCoords = function(params){
		this._mapX = params.x;
		this._mapY = params.y;
	}
	t.tick = function (event) {
		this.x = this._mapX - game._camera.x();
		this.y = this._mapY - game._camera.y();
		if(this._type == 1)
			this.rotate();
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
	t.manageClick = function(){
		
	}
	window.Tile = Tile;

}(window));
