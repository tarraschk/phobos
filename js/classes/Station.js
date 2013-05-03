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
		s.stage = new _.Stage("playground");
		var that = this;
		s.image.onload = function() {
			s.x = 350;//window.clientWidth /2;
			s.y = 235;
			s.stage.addChild(s);
			s.addEventListener("click", function(){
				console.log("click");
			}); 
			s.addEventListener("dblclick", function(){

			});
			s.addEventListener("onmousehover", function(){

			}); 
			s.addEventListener("mousehover", function(){

			}); 
			s.addEventListener("mouseover", function(){
				console.log("over");

			}); 
			s.addEventListener("mouseout", function(){
				console.log("out");

			});
			s.stage.update();
		}
		
	}
	window.Station = Station;

}(window));
