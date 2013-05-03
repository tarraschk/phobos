(function (window) {

	Background = function(){
		this.initialize();
	}

	var bg = Background.prototype = new _.Bitmap();

// static public properties:
	Background.path = 'img/backgrounds/';

// public properties:
// constructor:
	bg.Container_initialize = bg.initialize;	//unique to avoid overiding base class

	bg.initialize = function () {

	}

// public methods:

	bg.tick = function (event) {
	}

	bg.setBackgroundSrc = function(newSrc) {

	}

	bg.load = function(src){
		var src = Background.path+src;
		bg.image = new Image() ;
		bg.image.src = src; 
		bg.scaleX = 1 ; 
		bg.scaleY = 1 ; 
		bg.width = 1800 ; 
		bg.height = 1000 ;
		bg.stage = new createjs.Stage("background"); 
		bg.image.onload = function() {
			bg.stage.addChild(bg);
			bg.stage.update();
		}
	}
	window.Background = Background;

}(window));
