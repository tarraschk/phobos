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
	g._shipsList = [];
	g._gameGraphics = null ; 

// constructor:
	this.Container_initialize = this.initialize;	//unique to avoid overiding base class

	g.initialize = function () {
		resize();
		var backgroundGame = new Background().load("void/secteur7.jpg");
		g._camera = new Camera();
		for (var j = 0 ; j < 2000 ; j++) {
			g._tilesMap[j] = new Tile({
				id:1,
				x:Math.random() * 4000,
				y:Math.random() * 4000,
				src:"iso-02-04.png",
			});
		}
		g._gameGraphics = new GameGraphics();

		g._station1 = new Station({
			src: 'stationIso.png',
			name: 'Station spatiale internationale',
			x: 0,
			y: 0,
			life: 150000
		});
		g._station2 = new Station({
			src: 'stationIso.png',
			name: 'Station MIR',
			x: 600,
			y: 300,
			life: 150000
		});
		g._playerShip = new phobos.Ship({
			name:"Testeur",
			id:2, 
			x:200,
			y:550,
			src:"spriteShip.png",
		});
		g._shipsList[1] = new phobos.Ship({
			name:"Testeur2",
			id:1, 
			x:100,
			y:350,
			src:"spriteShip.png",
		});
		g._shipsList[2] = g._playerShip;
		g._bot = new phobos.Bot({
			name:"Bot",
			id:0, 
			x:900,
			y:350,
			src:"spriteShip.png",
		});
		g._shipsList[0] = g._bot;
		console.log(g._shipsList);
		$(document).on('click', function(e){
			var cooClick = utils.cameraToAbsolute({	x:e.clientX, y:e.clientY}, g._camera._position);
			
			var cooClick2 = utils.stdToAbsolute({	x:e.clientX, y:e.clientY}, g._camera._position);
			
			g._playerShip.setDestination({x:cooClick2.x, y:cooClick2.y});
		});
	}

	g.launchTicker = function() {
		_.Ticker.addListener(window);
		_.Ticker.useRAF = true;
		_.Ticker.setFPS(30);
		_.Ticker.addEventListener("tick", this.tick);
	}
// public methods:

	g.tick = function (event) {
		for (var k = 0 ; k < g._shipsList.length ; k++) {
			g._shipsList[k].tick();
		}
		g._gameGraphics.tick();
		g._camera.tick();
		g._station1.tick();
		g._station2.tick();
		for (var k = 0 ; k < g._tilesMap.length ; k++) {
			g._tilesMap[k].tick();
		}
		renderCanvas();
		// if (Math.random() < 0.01) console.clear();
	}
	window.Game = Game;

}(window));
