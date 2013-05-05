(function (window) {

	function Game() {
		this.initialize();
	}
	var g = Game.prototype ;
	g._isReady = false;
	g._started = false;
	g._engine = null;
	g._playerShip = null;

// constructor:
	this.Container_initialize = this.initialize;	//unique to avoid overiding base class

	g.initialize = function () {
		resize();
		var backgroundGame = new Background().load("void/secteur7.jpg");
		g._camera = new Camera();
		g._station = new Station({
			src: 'stationIso.png',
			name: 'Station spatiale internationale',
			x: 0,
			y: 0
		});
		g._playerShip = new Ship({
			id:1, 
			x:255,
			y:35,
			src:"spriteShip.png",
		});
		$(document).on('click', function(e){
			g._playerShip.setDestination({x:e.clientX, y:e.clientY});
		});
	}

	g.launchTicker = function() {
		_.Ticker.addListener(window);
		_.Ticker.useRAF = true;
		_.Ticker.setFPS(60);
		_.Ticker.addEventListener("tick", this.tick);
	}
// public methods:

	g.tick = function (event) {
		g._playerShip.tick();
		g._camera.tick();
		g._station.tick();
		renderCanvas();
	}
	window.Game = Game;

}(window));