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
		s.x = this.mapX - game._camera.x();
		s.y = this.mapY - game._camera.y();
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
		s.image = new Image();
		s.image.src = src; 
		var that = this;
		s.image.onload = function() {
			//s.x = 350;//window.clientWidth /2;
			//s.y = 235;
			cPlayground.addChild(s);
			s.addEventListener("mouseover", function(e) {
				debug('over '+that.name);
			});
			s.addEventListener("mouseout", function(e) {
				debug('out of '+that.name);
			});
			s.addEventListener("click", function(){
				debug('click on '+that.name);
			}); 
		}
	}
	window.Station = Station;

}(window));
