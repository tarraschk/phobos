stage = new createjs.Stage("background");

(function (window) {

	Background = function(){
		this._path = 'img/backgrounds/';
		this._stage = new _.Stage('background');
		this._image;
		this._stage.scaleX = 1;
		this._stage.scaleY = 1;
		this._stage.width = 1;
		this._stage.height = 1;
		return this;
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
		console.log('loading background');
		var src = Background.path+src;
		bg.image = new Image() ;
		bg.image.src = src; 
		bg.scaleX = 1 ; 
		bg.scaleY = 1 ; 
		bg.width = 1800 ; 
		bg.height = 1000 ;
		bg.stage = new createjs.Stage("background"); 
		bg.image.onload = function() {
			console.log("enculé"); 

			bg.stage.addChild(bg);
			bg.stage.update();
		}


		console.log('loaded');
	}
	window.Background = Background;

}(window));