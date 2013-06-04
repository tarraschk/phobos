
(function () {

	function Game() {
		this.initialize();
	}
	var g = Game.prototype ;
	g._isReady = false;
	g._started = false;
	g._engine = null;
	g._playerShip = null;
	g._tilesMap = []; 
	g._dockedShipsList = [] ; 
	g._killedShipsList = [] ; 
	g._shipsList = [];
	g._gameGraphics = null ; 

// constructor:
	this.Container_initialize = this.initialize;	//unique to avoid overiding base class

	g.initialize = function () {
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
			x:1500,
			y:1050,
			src:"spriteShip.png",
		});
		g._shipsList[1] = new phobos.Ship({
			name:"Testeur2",
			id:1, 
			x:2000,
			y:1350,
			src:"spriteShip.png",
		});
		g._shipsList[2] = g._playerShip;
		g._bot = new phobos.Bot({
			name:"Bot",
			id:0, 
			x:1900,
			y:1350,
			src:"spriteShip.png",
		});
		g._shipsList[3] = new phobos.Bot({
			name:"Bot2",
			id:3, 
			x:1500,
			y:3350,
			src:"spriteShip.png",
		});
		g._shipsList[0] = g._bot;
		this.launchTicker();
	}

	g.launchTicker = function() {
		setInterval(function(){
	        this.tick();
    	}.bind(this), 15);
		// _.Ticker.addListener(window);
		// _.Ticker.useRAF = true;
		// _.Ticker.setFPS(60);
		// _.Ticker.addEventListener("tick", this.tick);
	}
// public methods:

	g.tick = function (event) {
		allowMoveClick = true ; 
		for (key in g._shipsList) {
			if (String(Number(key)) === key && g._shipsList.hasOwnProperty(key)) {
				if (g._shipsList[key].index == g._shipsList[key].id) g._shipsList[key].tick();
			}
		}
		// g._gameGraphics.tick();
		// g._camera.tick();
		g._station1.tick();
		g._station2.tick();
		g._shipsList[3].moveTo({x:-150,y:-200});
		for (var k = 0 ; k < g._tilesMap.length ; k++) {
			g._tilesMap[k].tick();
		}
		// renderCanvas();
		// backgroundGame.tick();
		// backgroundGame2.tick();
		// backgroundGame3.tick();
		// if (Math.random() < 0.01) console.clear();
	}

	g.switchPlayerToKilled = function (player) {
		console.log("Before kill");
		console.log(player.name);
		g._killedShipsList[player.id] = player;
		//g._shipsList.splice(player.id, 1); 
		console.log("after kill");
		console.log(g._shipsList);

	}

	g.switchPlayerToStation = function (player) {
		g._dockedShipsList[player.id] = player;
		g._shipsList.splice(player.id, 1); 

	}
	phobos.Game = Game;

}());
