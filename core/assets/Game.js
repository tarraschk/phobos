(function () {

	function Game() {
		this.initialize();
	}
	var g = Game.prototype ;
	g._isReady = false;
	g._started = false;
	g._engine = null;
	g._playerShip = null;
	g._universe = [];
	g._objectsList = [] ; 
	g._tilesList = [] ;
	g._dockedShipsList = [] ; 
	g._destroyedObjectsList = []; 
	g._killedShipsList = [] ; 
	// g._shipsList = [];
	g._players = [];
	g._objects = [];
	g._gameGraphics = null ; 
	g._updateTime = 15 ; 
	g._frame ;

// constructor:
	this.Container_initialize = this.initialize;	//unique to avoid overiding base class

	/* DATA ENTRY TO SPECIFY !!! */
	g.initialize = function () {
		this._frame = 0 ; 
		this._universe = [];
		this._players = [];
		this._objects = [];
		this._dockedShipsList = [];
		this._killedShipsList = [];
		if (server) console.log("Server");
		else console.log("Client");
		if (!server) this.initGraphics(); 
	}

	/* DATA ENTRY TO SPECIFY !!! */
	g.initGraphics = function() {
		resize();
		backgroundGame = new Background("");//void/far7plagiat.png
		backgroundGame2 = new Background("", 15); //void/asteroidlayer.png
		backgroundGame3 = new Background("", 25); //void/nebulalayer.png
		this._camera = new phobos.Camera();
		this._frame = 0 ;
		this._gameGraphics = new GameGraphics();
	}

	/** SECTOR MANAGEMENT AND LOADERS */

	g.sectorInitialize = function(sectorId) {
		this.getUniverse()[sectorId] = {
			objects:[],
			tiles:[],
			ships:[],
			dockedShips:[],
			destroyedObjects:[],
			killedShips:[],
		}
	}

	g.loadSector = function(sectorId, sector) {
		var sectorShips = sector.ships ; 
		var sectorObjects = sector.objects;
		var sectorTiles = sector.tiles ;
		if (!this.getUniverse()[sectorId])
			this.sectorInitialize(sectorId);
		this.loadSharedObjects(sectorId, sectorObjects);
		this.loadTiles(sectorTiles); 
		this.loadSectorPlayers(sectorShips);
	}

	g.loadSharedObjects = function(sectorId, objects) {
		var theNewObject = null ; 
		console.log(phobos);
		for (key in objects) {
			if (String((key)) === key && objects.hasOwnProperty(key)) {
				switch(objects[key].type) {
					case "Station":
						theNewObject = new phobos.Station(objects[key]);
					break;
					case "Bot":
						theNewObject = new phobos.Bot(objects[key]);
					break;
					case "Collectable":
						theNewObject = new phobos.Collectable(objects[key]);
					break;
				}
				this.objectAdd(theNewObject);
			}
		}
	}
	// g.loadObjects = function(objects) {
	// }

	g.loadTiles = function(tiles) {
		for (key in tiles) {
			if (String((key)) === key && tiles.hasOwnProperty(key)) {
			this._tilesList[tiles[key].id] = new phobos.Tile(tiles[key]);
			}
		}
	}


	/** PLAYERS MANAGEMENT AND LOADERS */


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



	/** 
	Player management and orders 
	*/

	g.playerAttack = function(playerId, targetId) {
		var player = this.getPlayers()[playerId];
		var target = this.getObjects()[targetId]; //To do attack another player !
		player.setTargetId(targetId);
		player.setHasTarget(true);
		player.setTargetType("bot");
		player.setDestination({ x:target.getPosition().x, y: target.getPosition().y} );
	}

	g.playerCollects = function(playerId, objectId) {
		var player = this.getPlayers()[playerId];
		var target = this.getObjects()[objectId];
		player.setTargetId(objectId);
		player.setHasTarget(true);
		player.setTargetType("collectable");
		player.setDestination({ x:target.getPosition().x, y: target.getPosition().y} );
	}


	g.playerJoin = function(playerData, isMainPlayer) {
		var sector = playerData.position.sector;
		var ship = new phobos.Ship(playerData); 
		this.getUniverse()[sector].ships[playerData.id] = ship;
		this._players[playerData.id] = ship;

		if (isMainPlayer) {
			this.setPlayerId(playerData.id);
			this.setPlayerShip(ship); 
		}
		return ship;
	}

	g.objectAdd = function(object) {
		var sectorId = object.getSector();
		this.getUniverse()[sectorId].objects[object.getId()] = object;
		this.getObjects()[object.getId()] = object;
	}

	g.playerLeaves = function(player) {

	}


	/**
	* Main game loops
	*/


	g.graphicsTick = function() {
		this._gameGraphics.tick();
		this._camera.tick();
		renderCanvas();
		backgroundGame.tick();
		backgroundGame2.tick();
		backgroundGame3.tick();
		ui.tick();
	}

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


	/**
	
	Loops on the entire universe's objecets : each sector, each ship, object and tile. 

	*/
	g.objectsTick = function() {
		allowMoveClick = true ;  

		for (var uniId = 0 ; uniId < this.getUniverse().length ; uniId++) {

			for (keyUniverse in this.getUniverse()[uniId].objects) {

				if (String((keyUniverse)) === keyUniverse && this.getUniverse()[uniId].objects[keyUniverse] && this.getUniverse()[uniId].objects.hasOwnProperty(keyUniverse)) {
					//Verify object (fucking javascript)
					this.getUniverse()[uniId].objects[keyUniverse].tick();
				}
			}

			for (keyUniverse in this.getUniverse()[uniId].ships) {
				if (String((keyUniverse)) === keyUniverse && this.getUniverse()[uniId].ships[keyUniverse] && this.getUniverse()[uniId].ships.hasOwnProperty(keyUniverse)) {
					//Verify object (fucking javascript)
					this.getUniverse()[uniId].ships[keyUniverse].tick();
				}
			}
			for (keyUniverse in this.getUniverse()[uniId].tiles) {
				if (String((keyUniverse)) === keyUniverse && this.getUniverse()[uniId].tiles[keyUniverse] && this.getUniverse()[uniId].tiles.hasOwnProperty(keyUniverse)) {
					//Verify object (fucking javascript)
					this.getUniverse()[uniId].tiles[keyUniverse].tick();
				}
			}

		}

		// for (key in this.getUniverse()) {
		// 	if (String((key)) === key && this.getUniverse().hasOwnProperty(key)) {
		// 		console.log("universe tick");
		// 		console.log(this.getUniverse()[key]);
		// 		for (keyUniverse in this.getUniverse()[key].objects) {
		// 			console.log("objects");
		// 			console.log(this.getUniverse()[key].objects);
		// 			// if (String((keyUniverse)) === keyUniverse) {
		// 			// 		console.log("TICKING");
		// 			// 		console.log(this.getUniverse()[key].objects[keyUniverse]);
		// 			// 		this.getUniverse()[key].objects[keyUniverse].tick();
		// 			// }
		// 		}
		// 	}
		// }

		// for (key in this._shipsList) {
		// 	if (String((key)) === key && this._shipsList.hasOwnProperty(key)) {
		// 		if (this._shipsList[key].index == this._shipsList[key].id) {
		// 			this._shipsList[key].tick();
		// 		}
		// 	}
		// }

		// for (key in this._objectsList) {
		// 	if (String((key)) === key && this._objectsList.hasOwnProperty(key)) {
		// 		if (this._objectsList[key].index == this._objectsList[key].id) {
		// 			this._objectsList[key].tick();
		// 		}
		// 	}
		// }

		// for (key in this._tilesList) {
		// 	if (String((key)) === key && this._tilesList.hasOwnProperty(key)) {
		// 		if (this._tilesList[key].index == this._tilesList[key].id) {
		// 			this._tilesList[key].tick();
		// 		}
		// 	}
		// }
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

	/** 	Change objects status */

	g.switchObjectToCargo = function(player, collectable) {
		collectable.hide();
		this.deleteObject(collectable);
	}

	g.switchObjectToDestroyed = function(object) {
		this._destroyedObjectsList[object.id] = object;
		this.deleteObject(object);
	}
	
	g.switchPlayerToKilled = function (player) {
		this._killedShipsList[player.id] = player;
		this.deletePlayer(player);

	}

	g.switchPlayerToStation = function (player) {
		this._dockedShipsList[player.id] = player;
		this.deletePlayer(player);
		// this._shipsList.remove(1, 2);

	}

	g.deletePlayer = function(player) {
		delete this.getPlayers()[player.getId()];
		delete this.getUniverse()[player.getSector()].ships[player.getId()];
	}

	g.deleteObject = function(object) {
		delete this.getObjects()[object.getId()];
		delete this.getUniverse()[object.getSector()].objects[object.getId()];
	}


	/** Share & net data */

	g.exportSector = function(sector) {
		var sharedData = { 
			tiles: {},
			killedShips: {}, 
			dockedShips: {}, 
			ships:{}, 
			objects:{},
			destroyedObjects:{},
		};
		if (this.getUniverse()[sector]) {
			var sectorExported = this.getUniverse()[sector];
			for (key in sectorExported.ships) {
				if (String((key)) === key && sectorExported.ships.hasOwnProperty(key)) {
					if (sectorExported.ships[key].index == sectorExported.ships[key].id) {
						sharedData.ships[key] = sectorExported.ships[key].shared ;
					}
				}
			}

			for (key in sectorExported.dockedShips) {
				if (String((key)) === key && sectorExported.dockedShips.hasOwnProperty(key)) {
					if (sectorExported.dockedShips[key].index == sectorExported.dockedShips[key].id) {
						sharedData.dockedShips[key] = sectorExported.dockedShips[key].shared ;
					}
				}
			}

			for (key in sectorExported.objects) {
				if (String((key)) === key && sectorExported.objects.hasOwnProperty(key)) {
					if (sectorExported.objects[key].index == sectorExported.objects[key].id) {
						sharedData.objects[key] = sectorExported.objects[key].shared ;
					}
				}
			}

			for (key in sectorExported.destroyedObjects) {
				if (String((key)) === key && sectorExported.destroyedObjects.hasOwnProperty(key)) {
					if (sectorExported.destroyedObjects[key].index == sectorExported.destroyedObjects[key].id) {
						sharedData.destroyedObjects[key] = sectorExported.destroyedObjects[key].shared ;
					}
				}
			}

			for (key in sectorExported.tiles) {
				if (String((key)) === key && sectorExported.tiles.hasOwnProperty(key)) {
					if (sectorExported.tiles[key].index == sectorExported.tiles[key].id) {
						sharedData.tiles[key] = sectorExported.tiles[key].shared ;
					}
				}
			}
		}
		else return -1 ; 
		
		return sharedData;
	}

	// g.getSharedData = function() {
	// 	var sharedData = { 
	// 		tiles: {},
	// 		killedShips: {}, 
	// 		dockedShips: {}, 
	// 		ships:{}, 
	// 		objects:{},
	// 		destroyedObjects:{},
	// 	};
	// 	for (key in this._shipsList) {
	// 		if (String((key)) === key && this._shipsList.hasOwnProperty(key)) {
	// 			if (this._shipsList[key].index == this._shipsList[key].id) {
	// 				sharedData.ships[key] = this._shipsList[key].shared ;
	// 			}
	// 		}
	// 	}

	// 	for (key in this._dockedShipsList) {
	// 		if (String((key)) === key && this._dockedShipsList.hasOwnProperty(key)) {
	// 			if (this._dockedShipsList[key].index == this._dockedShipsList[key].id) {
	// 				sharedData.dockedShips[key] = this._dockedShipsList[key].shared ;
	// 			}
	// 		}
	// 	}

	// 	for (key in this._objectsList) {
	// 		if (String((key)) === key && this._objectsList.hasOwnProperty(key)) {
	// 			if (this._objectsList[key].index == this._objectsList[key].id) {
	// 				sharedData.objects[key] = this._objectsList[key].shared ;
	// 			}
	// 		}
	// 	}

	// 	for (key in this._destroyedObjectsList) {
	// 		if (String((key)) === key && this._destroyedObjectsList.hasOwnProperty(key)) {
	// 			if (this._destroyedObjectsList[key].index == this._destroyedObjectsList[key].id) {
	// 				sharedData.destroyedObjects[key] = this._destroyedObjectsList[key].shared ;
	// 			}
	// 		}
	// 	}

	// 	for (key in this._tilesList) {
	// 		if (String((key)) === key && this._tilesList.hasOwnProperty(key)) {
	// 			if (this._tilesList[key].index == this._tilesList[key].id) {
	// 				sharedData.tiles[key] = this._tilesList[key].shared ;
	// 			}
	// 		}
	// 	}
	// 	return sharedData;
	// }




	/* Getters and setters, various */

	g.setPlayerId = function(playerId) {
		this.playerId = playerId ; 
	}

	g.setPlayerShip = function(playerShipData) {
		this._playerShip = playerShipData;
	}

	g.setFrame = function(newFrame) {
		this._frame= newFrame;
	}

	g.getPlayerShip = function() {
		return this._playerShip;
	}

	g.getDockedShipsList = function() {
		return this._dockedShipsList; 
	}

	g.getObjects = function() {
		return this._objects; 
	}

	g.getTilesList = function() {
		return this._tilesList; 
	}

	g.getPlayers = function() {
		return this._players;
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

	g.getUniverse = function() {
		return this._universe;
	}

	phobos.Game = Game;

}());
