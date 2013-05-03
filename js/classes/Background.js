(function (window) {

	function Background() {
		this.initialize();
	}

	var bg = Background.prototype = new _.Bitmap();

// static public properties:
	Background.path = 'img/backgrounds/';

// public properties:
// constructor:
	bg.Container_initialize = bg.initialize;	//unique to avoid overiding base class

	bg.initialize = function () {
		bg.stage = new createjs.Stage("background");
	}

// public methods:

	bg.tick = function (event) {
	}
	bg.load = function(src){
		console.log('loading background');
		var src = Background.path+src;
		bg.image = new Image().src = src ;
		bg.stage.addChild(bg.image);
		bg.stage.update();
		console.log('loaded');
	}
	window.Background = Background;

}(window));

Background = function(){
	this._path = 'img/backgrounds/';
}