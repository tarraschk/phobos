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
		ui = new UI();
		g._station1 = new Station({
			src: 'stationIso.png',
			name: 'Station spatiale internationale',
			x: 0,
			y: 0
		});
		g._station2 = new Station({
			src: 'stationIso.png',
			name: 'Station MIR',
			x: 600,
			y: 300
		});
		g._playerShip = new Ship({
			id:1, 
			x:100,
			y:30,
			src:"spriteShip.png",
		});
		$(document).on('click', function(e){
			var cooClick = utils.cameraToAbsolute({	x:e.clientX, y:e.clientY}, g._camera._position);
			console.log("Click sur : ");console.log({	x:e.clientX, y:e.clientY});
			console.log("Camera : ");console.log(cooClick);
			var cooClick2 = utils.stdToAbsolute({	x:e.clientX, y:e.clientY}, g._camera._position);
			console.log("iso : ");
			console.log(cooClick2);
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
		g._playerShip.tick();
		g._camera.tick();
		g._station1.tick();
		g._station2.tick();
		renderCanvas();
	}
	window.Game = Game;

}(window));
