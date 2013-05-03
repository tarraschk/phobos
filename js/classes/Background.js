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

<<<<<<< HEAD
	Background.prototype.load = function(src){
		var src = this._path+src;
		console.log('loading background', src);
		this._imgTmp = new Image();
		this._imgTmp.src = src;
		that = this;
		this._imgTmp.onload = function(){
			that._image  = new _.Bitmap(this)
			that._stage.addChild(that._image);
			that._stage.update();
		}
		console.log('loaded');
	}

	Background.prototype.tick = function(){
		
	}
	window.Background = Background;
}(window));

=======
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
>>>>>>> d0cd5b312a6c00a437bf9c252ad8aa2ea6fc911a
