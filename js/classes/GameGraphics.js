(function (window) {

	function GameGraphics() {
		this.initialize();
	}
	var g = GameGraphics.prototype ;
	g._container  = null;
	g._containerX = 0;
	g._containerY = 0 ; 
// constructor:

	g.initialize = function () {
		this._container = new _.Container();
		cPlayground.addChild(this._container);
	}

// public methods:

	g.drawLaser = function(o1, o2) {

		var g = new _.Graphics();

		g.beginStroke(_.Graphics.getRGB(200,66,10));
		g.setStrokeStyle(5,10,10);
		var startPoint = {x: o1.x + game._camera.x(), y: o1.y + game._camera.y()};
		var endPoint = {x: o2.x + game._camera.x(), y: o2.y + game._camera.y()};
		g.moveTo(startPoint.x,startPoint.y)
		.lineTo(endPoint.x,endPoint.y).endStroke();

		var s = new _.Shape(g);
		this._container.addChild(s);
	}

	g.emptyGraphics = function(){
		this._container.removeAllChildren();
	}

	g.tick = function() {
		this._container.x = this._containerX - game._camera.x();
		this._container.y = this._containerY - game._camera.y();
		if (Math.random() < 0.1) this.emptyGraphics(); 
	}

	window.GameGraphics = GameGraphics;

}(window));
