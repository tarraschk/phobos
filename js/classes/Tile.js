(function (window) {

	Tile = function(params){
		this.initialize(params);
	}

	var s = Tile.prototype = new _.Bitmap();

// static public properties:
	Tile.path = 'img/tiles/';
	
// public properties:
	s._damages = 200 // dommages que la station peut causer quand elle attaque;
	s._life; // vie totale de la station
	s._lifeLeft; // vie restante a la station
	s._target;
	s._isInspected;
	s._mapX;
	s._mapY;
	s._name;
	s.fucused = false;
	s.scannable;
	s._id;
// constructor:
	s.initialize = function (params) {
		this._id = utils.generateId();
		this.setMapCoords({x: params.x, y: params.y});
		this.load(params.src);
	}

// public methods:
	s.setMapCoords = function(params){
		this._mapX = params.x;
		this._mapY = params.y;
	}
	s.tick = function (event) {
		this.x = this._mapX - game._camera.x();
		this.y = this._mapY - game._camera.y();
	}
	s.load = function(src){
		this.image = new Image();
		this.image.src = Tile.path+src; 
		var that = this;
		this.image.onload = function() {
			cPlayground.addChild(that);
		}
	}
	window.Tile = Tile;

}(window));
