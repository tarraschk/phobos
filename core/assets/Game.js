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
	g._destroyedObjectsList = []; 
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
		for (var j = 0 ; j < 0 ; j++) {
			g._tilesMap[j] = new Tile({
				id:1,
				x:Math.random() * 2500,
				y:Math.random() * 2500,
				src:"iso-02-04.png",
			});
		}
		this._gameGraphics = new GameGraphics();
	}

	g.loadSector = function(sector, shared) {
		var sectorShips = sector.ships ; 
		var sectorObjects = sector.objects;
		var sectorTiles = sector.tiles ;
		this.initObjects();
		this.initTiles(); 
		if (shared)
			this.loadSharedObjects(sectorObjects);
		else 
			this.loadObjects(sectorObjects);
		this.loadTiles(sectorTiles); 
		this.loadSectorPlayers(sectorShips);
	}

	g.initObjects = function() {
		this._objectsList = [] ; 
	}

	g.initTiles = function() {
		this._tilesList = [] ; 
	}

	g.loadSectorPlayers = function(playersData) {
		for (key in playersData) {
			if (String((key)) === key && playersData.hasOwnProperty(key)) {
					if (key != this.playerId) //Must be other players, not main player already loaded
					{
						var player = playersData[key]
						this.playerJoin(player, false); 
					}
				
			}
		}
	}

	g.loadSharedObjects = function(objects) {
		for (key in objects) {
			if (String((key)) === key && objects.hasOwnProperty(key)) {
				switch(objects[key].type) {
					case "Station":
						this._objectsList[objects[key].id] = new phobos.Station(objects[key]);
					break;
					case "Bot":
						this._objectsList[objects[key].id] = new phobos.Bot(objects[key]);
					break;
				}
			}
		}
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
	// g.loadObjects = function(objects) {
	// }

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

	g.playerAttack = function(player, target) {
		var player = this.getShipsList()[player.id];
		player.setTargetId(target.id);
		player.setHasTarget(true);
		player.setTargetType("bot");
		player.setDestination({ x:target.position.x, y: target.position.y} );
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
		var sharedData = { 
			killedShips: {}, 
			dockedShips: {}, 
			ships:{}, 
			objects:{},
			destroyedObjects:{},
		};
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
					sharedData.objects[key] = this._objectsList[key].shared ;
				}
			}
		}

		for (key in this._destroyedObjectsList) {
			if (String((key)) === key && this._destroyedObjectsList.hasOwnProperty(key)) {
				if (this._destroyedObjectsList[key].index == this._destroyedObjectsList[key].id) {
					sharedData.destroyedObjects[key] = this._destroyedObjectsList[key].shared ;
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
		return this._playerShip.getStatus();
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

	g.switchObjectToDestroyed = function(object) {
		this._destroyedObjectsList[object.id] = object;
		delete this._objectsList[object.id];
	}
	
	g.switchPlayerToKilled = function (player) {
		this._killedShipsList[player.id] = player;
		//this._shipsList.splice(player.id, 1); 

	}

	g.switchPlayerToStation = function (player) {
		this._dockedShipsList[player.id] = player;
		// this._shipsList.splice(player.id, 1); 
		delete this._shipsList[player.id];
		// this._shipsList.remove(1, 2);

	}

	phobos.Game = Game;

}());
