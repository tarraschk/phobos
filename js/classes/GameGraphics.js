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
		console.log(o1);
		console.log(o2);

		var g = new _.Graphics();

		g.beginStroke(_.Graphics.getRGB(200,66,10));
		g.setStrokeStyle(5,10,10);
		g.moveTo(o1.x,o1.y)
		.lineTo(o2.x,o2.y).endStroke();

		var s = new _.Shape(g);
		this._container.addChild(s);
	}

	g.tick = function() {
		this._container.x = this._containerX - game._camera.x();
		this._container.y = this._containerY - game._camera.y();
	}

	window.GameGraphics = GameGraphics;

}(window));
