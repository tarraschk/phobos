(function (window) {

	Station = function(params){
		this.initialize(params);
	}

	var s = Station.prototype = new _.Bitmap();

// static public properties:
	Station.path = 'img/objects/stations/';
	
// public properties:
	s._mapX;
	s._mapY;
	s._name;
	s.fucused = false;
	s.scannable;
// constructor:
	s.initialize = function (params) {
		this._name = params.name;
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

	s.setBackgroundSrc = function(newSrc) {

	}
	s.onOver = function(){
		//display name
	}
	s.onClick = function(){
		//gerer le clic
	}
	s.load = function(src){
		var src = Station.path+src;
		this.image = new Image();
		this.image.src = src; 
		var that = this;
		this.image.onload = function() {
			//s.x = 350;//window.clientWidth /2;
			//s.y = 235;
			cPlayground.addChild(that);
			that.addEventListener("mouseover", function(e) {
				debug('over '+that._name);
			});
			that.addEventListener("mouseout", function(e) {
				debug('out of '+that._name);
			});
			that.addEventListener("click", function(e){
				console.log('click listener', e);
				debug('click on '+that._name);
				that.manageClick();
			}); 
		}
	}
	s.name = function(name){
		if(name != undefined){
			this._name = name;
			return this;
		}
		else{
			return this._name;
		}
	}
	s.manageClick = function(){
		var n = this.name();
		debug('click on '+n);
	}
	window.Station = Station;

}(window));
