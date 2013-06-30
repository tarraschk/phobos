
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
	g._sectors = [];
	g._objectsList = [] ; 
	g._tilesList = [] ;
	g._dockedShipsList = [] ; 
	g._killedShipsList = [] ; 
	g._shipsList = [];
	g._gameGraphics = null ; 
	g._updateTime = 15 ; 
	g._frame ;

// constructor:
	this.Container_initialize = this.initialize;	//unique to avoid overiding base class

	/* DATA ENTRY TO SPECIFY !!! */
	g.initialize = function () {
		this._frame = 0 ; 
		this._shipsList = [];
		this._dockedShipsList = [];
		this._killedShipsList = [];
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
		this._frame = 0 ;
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
	    this._frame++;
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

	g.setFrame = function(newFrame) {
		this._frame= newFrame;
	}

	g.graphicsTick = function() {
		this._gameGraphics.tick();
		this._camera.tick();
		renderCanvas();
		backgroundGame.tick();
		backgroundGame2.tick();
		backgroundGame3.tick();
		ui.tick();
	}

	g.getSharedData = function() {
		var sharedData = { killedShips: {}, dockedShips: {}, ships:{}, objects:{} };
		for (key in this._shipsList) {
			if (String((key)) === key && this._shipsList.hasOwnProperty(key)) {
				if (this._shipsList[key].index == this._shipsList[key].id) {
					sharedData.ships[key] = this._shipsList[key].shared ;
				}
			}
		}

		for (key in this._dockedShipsList) {
			if (String((key)) === key && this._dockedShipsList.hasOwnProperty(key)) {
				if (this._dockedShipsList[key].index == this._dockedShipsList[key].id) {
					sharedData.dockedShips[key] = this._dockedShipsList[key].shared ;
				}
			}
		}

		for (key in this._objectsList) {
			if (String((key)) === key && this._objectsList.hasOwnProperty(key)) {
				if (this._objectsList[key].index == this._objectsList[key].id) {
					// sharedData.objects[key] = this._objectsList[key].shared ;
				}
			}
		}

		return sharedData;
	}

	g.getPlayerShip = function() {
		return this._playerShip;
	}

	g.getShipsList = function() {
		return this._shipsList; 
	}

	g.getDockedShipsList = function() {
		return this._dockedShipsList; 
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

	g.getFrame = function() {
		return this._frame;
	}

	g.getMainPlayerStatus = function() {
		console.log(this._playerShip);
		return this._playerShip.getStatus();
	}

	g.objectsTick = function() {
		// if (Math.random() < 0.1) console.log(client.getGame().getShipsList());
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
		this._killedShipsList[player.id] = player;
		//this._shipsList.splice(player.id, 1); 

	}

	g.switchPlayerToStation = function (player) {
		console.log("BEFORE");
		console.log(this.getShipsList());
		console.log(this.getDockedShipsList());
		this._dockedShipsList[player.id] = player;
		// this._shipsList.splice(player.id, 1); 
		delete this._shipsList[player.id];
		// this._shipsList.remove(1, 2);
		console.log("AFTER");
		console.log(this.getShipsList());
		console.log(this.getDockedShipsList());

	}

	phobos.Game = Game;

}());
