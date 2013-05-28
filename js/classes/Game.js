(function (window) {

	function Game() {
		this.initialize();
	}
	var g = Game.prototype ;
	g._isReady = false;
	g._started = false;
	g._engine = null;
	g._playerShip = null;
	g._tilesMap = []; 

// constructor:
	this.Container_initialize = this.initialize;	//unique to avoid overiding base class

	g.initialize = function () {
		resize();
		var backgroundGame = new Background().load("void/secteur7.jpg");
		g._camera = new Camera();
		g._map = new Map();
		
		g._bot = new Bot({
			name:"Bot",
			id:1, 
			x:400,
			y:350,
			src:"spriteShip.png",
		});
		g._station = new Station({
			src: 'stationIso.png',
			name: 'Station MIR',
			x: 600,
			y: 300,
			life: 150000
		});
		g._playerShip = new Ship({
			name:"Testeur",
			id:1, 
			x:600,
			y:350,
			src:"spriteShip.png",
		});
		
		
		$(document).on('click', function(e){
			var cooClick = utils.cameraToAbsolute({	x:e.clientX, y:e.clientY}, g._camera._position);
			
			var cooClick2 = utils.stdToAbsolute({	x:e.clientX, y:e.clientY}, g._camera._position);
			
			g._playerShip.setDestination({x:cooClick2.x, y:cooClick2.y});
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
		g._camera.tick();
		g._map.tick();
		g._station.tick();
		g._playerShip.tick();
		g._bot.tick();
		renderCanvas();
	}
	window.Game = Game;

}(window));
