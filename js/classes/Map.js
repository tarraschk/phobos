(function (window) {

	function Map() {
		this.initialize();
	}
	var m = Map.prototype ;
	m._isReady = false;
	m._started = false;
	m._engine = null;
	m._playerShip = null;

// constructor:
	this.Container_initialize = this.initialize;	//unique to avoid overiding base class

	m.initialize = function () {
		resize();
		var backgroundGame = new Background().load("void/secteur7.jpg");
		m._camera = new Camera();
		
		m._station1 = new Station({
			src: 'stationIso.png',
			name: 'Station spatiale internationale',
			x: 0,
			y: 0,
			life: 150000
		});
		m._station2 = new Station({
			src: 'stationIso.png',
			name: 'Station MIR',
			x: 600,
			y: 300,
			life: 150000
		});
		m._playerShip = new Ship({
			id:1, 
			x:600,
			y:350,
			src:"spriteShip.png",
		});
		$(document).on('click', function(e){
// <<<<<<< HEAD
// 			var cooClick = utils.stdToAbsolute({	x:e.clientX, y:e.clientY}, m._camera);
// 			console.log(cooClick);
// 			m._playerShip.setDestination({x:e.clientX+m._camera.x(), y:e.clientY+m._camera.y()});
// =======
			var cooClick = utils.cameraToAbsolute({	x:e.clientX, y:e.clientY}, m._camera._position);
			
			var cooClick2 = utils.stdToAbsolute({	x:e.clientX, y:e.clientY}, m._camera._position);
			
			m._playerShip.setDestination({x:cooClick2.x, y:cooClick2.y});
// >>>>>>> 4bb7b64b24499ebc44500deae86bf578b0349d60
		});
	}

	m.launchTicker = function() {
		_.Ticker.addListener(window);
		_.Ticker.useRAF = true;
		_.Ticker.setFPS(60);
		_.Ticker.addEventListener("tick", this.tick);
	}
// public methods:

	m.tick = function (event) {
		m._playerShip.tick();
		m._camera.tick();
		m._station1.tick();
		m._station2.tick();
		renderCanvas();
	}
	window.Game = Game;

}(window));
