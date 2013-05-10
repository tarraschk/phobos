(function (window) {

	Tile = function(params){
		this.initialize(params);
	}

	var t = Tile.prototype = new _.Bitmap();

// static public properties:
	Tile.path = 'img/tiles/';
	
// public properties:
	t._damages = 200 // dommages que la station peut causer quand elle attaque;
	t._life; // vie totale de la station
	t._lifeLeft; // vie restante a la station
	t._target;
	t._isInspected;
	t._mapX;
	t._mapY;
	t._name;
	t.fucused = false;
	t.scannable;
	t._id;
// constructor:
	t.initialize = function (params) {
		this._id = utils.generateId();
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
	}
	t.load = function(src){
		this.image = new Image();
		this.image.src = Tile.path+src; 
		var that = this;
		this.image.onload = function() {
			cPlayground.addChild(that);
		}
	}
	window.Tile = Tile;

}(window));
