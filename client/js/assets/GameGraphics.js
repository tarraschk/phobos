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

	g.drawLaser = function(o1, o2, weaponId) {
		var g = new _.Graphics();
		if (weaponId == 1)
			g.beginStroke(_.Graphics.getRGB(200,66,10));
		else 
			g.beginStroke(_.Graphics.getRGB(50,6,10));
		g.setStrokeStyle(5,10,10);
		var startPoint = {x: o1.x + client.getGame().getCamera().x(), y: o1.y + client.getGame().getCamera().y()};
		var endPoint = {x: o2.x + client.getGame().getCamera().x(), y: o2.y + client.getGame().getCamera().y()};
		g.moveTo(startPoint.x,startPoint.y)
		.lineTo(endPoint.x,endPoint.y).endStroke();

		var s = new _.Shape(g);
		this._container.addChild(s);
	}

	g.emptyGraphics = function(){
		this._container.removeAllChildren();
	}

	g.tick = function() {
		this._container.x = this._containerX - client.getGame().getCamera().x();
		this._container.y = this._containerY - client.getGame().getCamera().y();
		if (Math.random() < 0.2) this.emptyGraphics(); 
	}

	window.GameGraphics = GameGraphics;

}(window));
