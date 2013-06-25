
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
	g._objectsList = [] ; 
	g._tilesList = [] ;
	g._dockedShipsList = [] ; 
	g._killedShipsList = [] ; 
	g._shipsList = [];
	g._gameGraphics = null ; 
	g._updateTime = 80 ; 

// constructor:
	this.Container_initialize = this.initialize;	//unique to avoid overiding base class

	/* DATA ENTRY TO SPECIFY !!! */
	g.initialize = function () {
		if (server) console.log("Server");
		else console.log("Client");
		if (!server) this.initGraphics(); 
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

	g.loadSector = function(sector) {
		var sectorObjects = sector.objects;
		var sectorTiles = sector.tiles ;
		this.initObjects();
		this.initTiles(); 
		this.loadObjects(sectorObjects);
		this.loadTiles(sectorTiles); 
	}

	g.initObjects = function() {
		this._objectsList = [] ; 
	}

	g.initTiles = function() {
		this._tilesList = [] ; 
	}

	g.loadObjects = function(objects) {
		for (var k = 0 ; k < objects.length ; k++) {
			switch(objects[k].type) {
				case "Station":
					this._objectsList[objects[k].id] = new phobos.Station(objects[k]);
				break;
				case "Bot":
					this._objectsList[objects[k].id] = new phobos.Bot(objects[k]);
				break;
			}
		}
		console.log(this._objectsList);
	}

	g.loadTiles = function(tiles) {

	}

	g.startUpdate = function() {

		setInterval(function(){
	        this.tick();
    	}.bind(this), this._updateTime);
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
	    this.dt = this.lastframetime ? ( (t - this.lastframetime)/1000.0) : 0.016;
		if (Math.random() < 0.01) console.log("FPS : " + 1 / this.dt);
	    this.lastframetime = t;
	}

	g.playerJoin = function(playerData, isMainPlayer) {
		this._shipsList[playerData.id] = new phobos.Ship(playerData);

		if (isMainPlayer)
			this.setPlayerShip(this._shipsList[playerData.id]); 
		return this._shipsList[playerData.id];
	}

	g.setPlayerShip = function(playerShipData) {
		this._playerShip = playerShipData;
	}

	g.graphicsTick = function() {
		this._gameGraphics.tick();
		this._camera.tick();
		renderCanvas();
		backgroundGame.tick();
		backgroundGame2.tick();
		backgroundGame3.tick();
	}

	g.getShipsList = function() {
		return this._shipsList; 
	}

	g.getObjectsList = function() {
		return this._objectsList; 
	}

	g.getTilesList = function() {
		return this._tilesList; 
	}

	g.getCamera = function() {
		return this._camera;
	}

	g.objectsTick = function() {
		allowMoveClick = true ;  
		for (key in this._shipsList) {
			if (String((key)) === key && this._shipsList.hasOwnProperty(key)) {
				if (this._shipsList[key].index == this._shipsList[key].id) {
					this._shipsList[key].tick();
				}
			}
		}

		for (key in this._objectsList) {
			if (String((key)) === key && this._objectsList.hasOwnProperty(key)) {
				if (this._objectsList[key].index == this._objectsList[key].id) {
					this._objectsList[key].tick();
				}
			}
		}
		// g._station1.tick();
		// g._station2.tick();
		// this._shipsList[3].moveTo({x:-150,y:-200});
		// for (var k = 0 ; k < g._tilesMap.length ; k++) {
		// 	g._tilesMap[k].tick();
		// }
		// if (Math.random() < 0.01) console.clear();
	}

	g.switchPlayerToKilled = function (player) {
		g._killedShipsList[player.id] = player;
		//this._shipsList.splice(player.id, 1); 

	}

	g.switchPlayerToStation = function (player) {
		g._dockedShipsList[player.id] = player;
		this._shipsList.splice(player.id, 1); 

	}

	phobos.Game = Game;

}());
