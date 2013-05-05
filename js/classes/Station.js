(function (window) {

	Station = function(params){
		this.initialize(params);
	}

	var s = Station.prototype = new _.Bitmap();

// static public properties:
	Station.path = 'img/objects/stations/';
	
// public properties:
	s._isInspected;
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
		this.image = new Image();
		this.image.src = Station.path+src; 
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
		
	}
	s.manageMouseOver = function(){
		ui.openEntityInfos(this);
	}
	s.manageMouseOut = function(){
		
	}
	s.isInspected = function(is){
		if(is != undefined){
			this._isInspected = is;
			return this;
		}
		else{
			return this._isInspected;
		}
	}
	window.Station = Station;

}(window));
