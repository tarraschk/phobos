(function (window) {

	Station = function(params){
		this.initialize(params);
	}

	var s = Station.prototype = new _.Bitmap();

// static public properties:
	Station.path = 'img/objects/stations/';
	
// public properties:
	s.mapX;
	s.mapY;
	s.name;
	s.fucused = false;
	s.scannable;
// constructor:
	s.initialize = function (params) {
		this.name = params.name;
		this.setMapCoords({x: params.x, y: params.y});
		this.load(params.src);
	}

// public methods:
	s.setMapCoords = function(params){
		this.mapX = params.x;
		this.mapY = params.y;
	}
	s.tick = function (event) {

	}

	s.setBackgroundSrc = function(newSrc) {

	}

	s.load = function(src){
		var src = Station.path+src;
		s.image = new Image();
		s.image.src = src; 
		var that = this;
		s.image.onload = function() {
			s.x = 350;//window.clientWidth /2;
			s.y = 235;
			cPlayground.addChild(s);
			s.addEventListener("mouseover", function(e) {
				debug(that.width);
				debug(that.x);
			});

			s.addEventListener("mouseout", function(e) {
				debug('out of '+that.name);
			});
			s.addEventListener("click", function(){
				debug('click on '+that.name)
			}); 
			cPlayground.update();
		}
		
	}
	window.Station = Station;

}(window));
