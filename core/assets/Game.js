
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

	/* DATA ENTRY TO SPECIFY !!! */
	g.initialize = function () {
		if (server) console.log("Server");
		else console.log("Client");
		if (!server) this.initGraphics(); 
		console.log("init game");
	}

	/* DATA ENTRY TO SPECIFY !!! */
	g.initGraphics = function() {

		resize();
		backgroundGame = new Background("void/space-art-hd-473771.jpg");
		backgroundGame2 = new Background("void/asteroidlayer.png", 15);
		backgroundGame3 = new Background("void/nebulalayer.png", 25);
		this._camera = new Camera();
		for (var j = 0 ; j < 700 ; j++) {
			g._tilesMap[j] = new Tile({
				id:1,
				x:Math.random() * 2500,
				y:Math.random() * 2500,
				src:"iso-02-04.png",
			});
		}
		this._gameGraphics = new GameGraphics();
	}

	g.startUpdate = function() {

		setInterval(function(){
	        this.tick();
    	}.bind(this), 1);
	}

	g.startClientUpdate = function() {
		_.Ticker.addListener(window);
		_.Ticker.useRAF = true;
		_.Ticker.setFPS(60);
		_.Ticker.addEventListener("tick", this.tick);
	}

// public methods:

	g.tick = function () {
	    this.diffT(); 
	    this.objectsTick();
	    if (!server) this.graphicsTick() ; 
	}
	g.diffT = function() {
		t  = Date.now() ; 
	    //Work out the delta time
	    this.dt = this.lastframetime ? ( (t - this.lastframetime)/1000.0) : 0.016;
		// console.log("FPS : " + 1 / this.dt);

	        //Store the last frame time
	    this.lastframetime = t;
	}

	g.graphicsTick = function() {
		this._gameGraphics.tick();
		this._camera.tick();
		renderCanvas();
		backgroundGame.tick();
		backgroundGame2.tick();
		backgroundGame3.tick();
	}

	g.getCamera = function() {
		return this._camera;
	}

	g.objectsTick = function() {
		allowMoveClick = true ; 
		// for (key in g._shipsList) {
		// 	if (String(Number(key)) === key && g._shipsList.hasOwnProperty(key)) {
		// 		if (g._shipsList[key].index == g._shipsList[key].id) g._shipsList[key].tick();
		// 	}
		// }
		// g._station1.tick();
		// g._station2.tick();
		// g._shipsList[3].moveTo({x:-150,y:-200});
		// for (var k = 0 ; k < g._tilesMap.length ; k++) {
		// 	g._tilesMap[k].tick();
		// }
		// if (Math.random() < 0.01) console.clear();
	}
	phobos.Game = Game;

}());
