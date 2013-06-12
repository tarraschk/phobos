(function (window) {

	Background = function(src, scale){
		this.initialize(src, scale);
	}

	var bg = Background.prototype = new _.Bitmap();

// static public properties:
	Background.path = 'img/backgrounds/';

// public properties:
// constructor:
	bg.Container_initialize = bg.initialize;	//unique to avoid overiding base class

	bg.initialize = function (src, scale) {
		this.posXinit = 0;
		this.posYinit = 0;
		this.scaleFactor = scale;
		this.load(src);
	}

// public methods:

	bg.tick = function (event) {
		this.x = this.posXinit - client.getGame().getCamera()._position.x / this.scaleFactor + screenWidth / 2 - this.width / 2;
		this.y = this.posYinit - client.getGame().getCamera()._position.y / this.scaleFactor - this.height / 4 ;
	}

	bg.setBackgroundSrc = function(newSrc) {

	}

	bg.load = function(src, scale){
		var src = Background.path+src;
		this.image = new Image() ;
		this.image.src = src; 
		this.scaleX = 1 ; 
		this.scaleY = 1 ; 
		this.width = 1800 ; 
		this.height = 1000 ;
		var that = this
		this.image.onload = function() {
			cBackground.addChild(that);
			cBackground.update();
		}
	}
	window.Background = Background;

}(window));
