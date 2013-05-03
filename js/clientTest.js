(function($) {
	app = { 
    stage: null, 
    canvas: null, 
    layers: [], 
    tool: "", 
    callbacks: {}, 
    selection: { 
        x: -1, y: -1 
    }, 
    renameLayer: 0, 
    undoBuffer: [], 
    redoBuffer: [] 
	}
	/**************
	* Constantes **
	***************/
	var ROTATIONSPEED = 1.5
	var STAGE_WIDTH = 1000;
	var STAGE_HEIGHT = 500;
	var SPEED = 0.03;
	var KEY_DOWN	= 40;
	var KEY_UP		= 38;
	var KEY_LEFT	= 37;
	var KEY_RIGHT	= 39;
	var hasPressedUp = false;
	var hasPressedDown = false;
	var hasPressedRight = false;
	var hasPressedLeft = false;
	var terrain = [];
	/***********************
	* Variables globales **
	************************/
	var socket = io.connect('http://localhost:1337');
	var imgShip = new Image(); 
	//imgShip.src = "img/ship/Image_01.jpge39bff3d-384c-49fe-8b2c-45f2a5d42192Large-1.jpg";
	imgShip.src = "img/ship/spriteShip.png";
	backgroundSrc = "img/backgrounds/secteur7.jpg";
	//imgShip.src = "img/ship.png";
	var canvas = document.getElementById("playground"); 
	canvas.width  = window.innerWidth;
	canvas.height = window.innerHeight;
	var stage = new createjs.Stage("playground");
	var globalSpaceData = [] ; 
	var spaceObjects = [] ; 
	var gameBackground;
	var currentPlayer;
	var players = [];
	var playersCount = 0;
	var currentPlayerIndex = 0;
	var playerRunning = false;
	var gameCamera ;
	var UIEventGraphics; 
	
	function ServerIO() {
		
		this.connect = function(nickname) {
			socket.emit('playerLogin', nickname); 
		}
	
		socket.on('newPlayer', function(playersData) {
			console.log("new :"); console.log(playersData);
			new Ship(playersData);
		}); 
		socket.on('playerLogged', function(newPlayerData, playersData) {
			console.log("logged in :"); console.log(newPlayerData);
			console.log(playersData);
			initPlayer(newPlayerData);
			loadPlayers (playersData);
		});

		socket.on('loadObjects', function(objectsData) {
			console.log("Load objects : "); 
			console.log(objectsData); 
			for (var k = 0 ; k < objectsData.length ; k++) {
				console.log("new objectsData"); 
				spaceObjects[k] = new Object(objectsData[k]); 
				console.log(spaceObjects[k]); 
			}
		});
		socket.on('loadSpace', function(spaceData) {
			console.log("Load space : "); 
			console.log(spaceData); 
			var spaceTiles = spaceData;
			gameBackground = new Background();
			for (var k = 0 ; k < spaceTiles.length ; k++) {
				globalSpaceData[k] = new Tile(spaceTiles[k]); 
			}
		});

		socket.on('getInfoFromServer', function(playersData) {
			if (playersData.id != currentPlayerIndex) {
				for (key in players) {
					if (String(Number(key)) === key && players.hasOwnProperty(key)) {
						if (players[key].index == playersData.id)	players[key].getInfoFromServer(playersData);
					}
				}
			}
		})
	}

	function UIContainer(UIData) {
		this.construct = function() {
			this.contain = new createjs.Container() ; 
			this.graphics = new createjs.Graphics();
			this.bar = new createjs.Shape(this.graphics);
			if (UIData) {
				this.posXinit = UIData.x;
				this.posYinit = UIData.y;
			}
			else {
				this.posXinit = 0;
				this.posYinit = 0;
			}
			stage.addChild(this.contain); 

		}

		this.addChild = function(child) {
			this.contain.addChild(child); 
		}

		this.drawBar = function(width, posX, posY, value) {
			this.graphics.clear(); 
			console.log(width); 
			this.posXinit = posX;
			this.posYinit = posY; 
			this.graphics.beginFill("#ff0000").drawRect(posX, posY, width, 8);
			this.contain.addChild(this.bar); 
		}

		this.removeAllChildren = function() {
			console.log("do remove"); 
			this.contain.removeAllChildren(); 
		}

		this.tick = function() {
			//this.bar.x = this.posXinit - gameCamera.posX 
			//this.bar.y = this.posYinit - gameCamera.posY
		}

		this.construct(); 
	}

	function Background() {
		this.setBackground = function (backroundSrc) {
			//this.bckSrc = backroundSrc;

		}
		this.initialize = function () {
			this.bckSrc = backgroundSrc;
			this.backgroundBmp = new createjs.Bitmap(this.bckSrc );
			//this.backgroundBmp.style.zIndex = 1 ; 
			stage.addChild(this.backgroundBmp);	
			this.posXinit = 0 ; 
			this.posYinit = 0  ;
			this.scaleFactor = 7 ; 
			this.width = 1800 ;
			this.height = 1000 ; 
		}
		this.tick = function() {
			this.backgroundBmp.x = this.posXinit - gameCamera.posX / this.scaleFactor + STAGE_WIDTH / 2 - this.width / 2;
			this.backgroundBmp.y = this.posYinit - gameCamera.posY / this.scaleFactor - this.height / 4 ;
		}

		this.initialize();
	}

	function Camera() {
		this.setCamera = function(newX, newY) {
			this.posX = newX;
			this.posY = newY;
		}
		this.moveCamera = function(Xspeed, Yspeed) {
			this.posX += Xspeed;
			this.posY += Yspeed;
		}
		this.tick = function() {
			this.setCamera(players[currentPlayerIndex].drawX - STAGE_WIDTH /2 , players[currentPlayerIndex].drawY - STAGE_HEIGHT /2);
		}
		this.posX = 0;
		this.posY = 0;
	}

	function Object(objectData) {
		this.setImage = function (imageSrc) {
			this.imgSrc = imageSrc;

		}
		this.initialize = function (objectData) {
			this.imgSrc = objectData.imgSrc;
			this.objectBmp = new createjs.Bitmap(this.imgSrc );
			this.objectBmp.x = objectData.x; 
			this.objectBmp.y = objectData.y; 
			stage.addChild(this.objectBmp);	
			this.posXinit = objectData.x; 
			this.posYinit = objectData.y  ;
			this.objectBmp.addEventListener("click", handleMouseEvent); 
			this.objectBmp.addEventListener("dblclick", handleMouseEvent);
			this.objectBmp.addEventListener("mouseover", handleMouseEvent); 
			this.objectBmp.addEventListener("mouseout", handleMouseEvent);
		}
		this.tick = function() {
			this.objectBmp.x = this.posXinit - gameCamera.posX 
			this.objectBmp.y = this.posYinit - gameCamera.posY
			this.objectBmp.rotation += 0.001 ; 
		}

		this.initialize(objectData);
	}

	function Tile(tileData) {
		this.setImage = function (imageSrc) {
			this.imgSrc = imageSrc;

		}
		this.initialize = function (tileData) {
			this.imgSrc = tileData.imgSrc;
			this.tileBmp = new createjs.Bitmap(this.imgSrc );
			this.tileBmp.x = tileData.x; 
			this.tileBmp.y = tileData.y; 
			stage.addChild(this.tileBmp);	
			this.posXinit = tileData.x; 
			this.posYinit = tileData.y;
			this.tileBmp.addEventListener("click", handleMouseEvent); 
			this.tileBmp.addEventListener("dblclick", handleMouseEvent);
			this.tileBmp.addEventListener("onmousehover", handleMouseEvent); 
			this.tileBmp.addEventListener("mousehover", handleMouseEvent); 
			this.tileBmp.addEventListener("mouseover", handleMouseEvent); 
			this.tileBmp.addEventListener("mouseout", handleMouseEvent);
		}
		this.tick = function() {
			this.tileBmp.x = this.posXinit - gameCamera.posX 
			this.tileBmp.y = this.posYinit - gameCamera.posY
			this.tileBmp.rotation += 0.001 ; 
		}

		this.initialize(tileData);
	}

	function Ship(shipData)
	{
		this.sendInfoToServer = function () {
			socket.emit('playerData', {
				name: this.name,
				x: this.posX,
				y: this.posY,
				o: this.posO,
				id: this.index
			}); 
		}
		this.getInfoFromServer = function(serverData) {
			this.name = serverData.name,
			this.posX = serverData.x;
			this.posY = serverData.y;
			this.posO = serverData.o;
			this.index = serverData.id;
		}
		this.movePlayerX = function (speed) {
			this.posX += speed ; 
		}
		this.movePlayerY = function (speed) {
			this.posY += speed ; 
		}
		this.rotate = function (rotationSpeed) {
			this.posO += rotationSpeed;
		}
		this.cycleNonPlayer = function() {
			//this.sendInfoToServer() ; 
			this.drawObject();
		}
		this.rotationScreen = function() {
			//this.shipImg = "img/ship/Image_01.jpge39bff3d-384c-49fe-8b2c-45f2a5d42192Large-2.jpg";
			this.shipImg.gotoAndPlay("walk");
			this.shipImg.currentAnimationFrame = Math.abs((Math.round((this.posO % 360) / 12)));

			//this.shipImg.rotation = this.posO;
		}
		this.stopRotate = function() {
			//this.shipImg = "img/ship/Image_01.jpge39bff3d-384c-49fe-8b2c-45f2a5d42192Large-2.jpg";
		}
		this.drawObject = function() {

			this.rotationScreen();
			this.posX += Math.sin((this.posO)*(Math.PI/-180)) * this.throttle;
			this.posY += Math.cos((this.posO)*(Math.PI/-180)) * this.throttle;

			//this.shipImg.x = this.posX ;
			//this.shipImg.y = this.posY ;
			//this.shipImg.rotation = this.posO;
			this.shipImg.x = (Math.sqrt(2) / 2) * ( this.posX - this.posY);
			this.shipImg.y = 1/(Math.sqrt(6)) * (this.posX + this.posY);

			this.drawX = this.shipImg.x;
			this.drawY = this.shipImg.y;

			this.shipImg.x -= gameCamera.posX;
			this.shipImg.y -= gameCamera.posY;
		}
		this.throttleBrake = function(throttleSpeed) {
			this.throttle += throttleSpeed;
		}
		this.cyclePlayer = function() {
			if (hasPressedUp) {
				this.throttleBrake(SPEED);
				//this.movePlayerY(-SPEED) ; 
			}
			else if (hasPressedDown) {
				this.throttleBrake(-SPEED);
				//this.movePlayerY(SPEED) ; 
			}
			else {
			}
			if (hasPressedLeft) {
				this.rotate(-ROTATIONSPEED);
				//this.movePlayerX(-SPEED) ;  
			}
			else if (hasPressedRight) {
				this.rotate(ROTATIONSPEED);
				//this.movePlayerX(SPEED) ; 
			}
			else {
				this.stopRotate();
			}
			this.sendInfoToServer() ; 
			this.drawObject();
		//this.shipImg.skewX += 1;
		}
		/*
		*Constructeur 
		*/
		var spriteSheet = new createjs.SpriteSheet({
			// image to use
			images: [imgShip], 
			// width, height & registration point of each sprite
			frames: {width: 120, height: 120, regX: 60, regY: 60, vX:0.5, currentAnimationFrame: 27}, 
			animations: {    
				walk: [0, 30, "walk"]
			}
		});
		this.index = shipData.id; 
		//this.shipImg = new createjs.Shape();
		this.shipImg = new createjs.BitmapAnimation(spriteSheet);
		this.shipImg.gotoAndStop("walk");
		//this.shipImg.graphics.beginFill("red").drawRect(-15, -15, 30, 30);
		//Set position of Shape instance.
		this.posX = shipData.x;
		this.posY = shipData.y;
		this.drawX = shipData.x;
		this.drawY = shipData.y;
		this.name = shipData.name; 
		this.posO = 0;
		//this.shipImg.x = shipData.x;
		//this.shipImg.y = shipData.y;
		this.doLine = false;
		this.currentFrame = 1;
		this.shipImg.currentAnimationFrame = 29;
		this.shipImg.name = "default";
		this.shipImg.rotation = 0;
		this.shipImg.direction = 90;
		this.shipImg.vX = 0;
		this.shipImg.vY = 0; 
		this.shipImg.scaleX = 0.4;
		this.shipImg.scaleY = 0.4;

		this.throttle = 0;
		stage.addChild(this.shipImg);	
		console.log(this.shipImg.currentAnimation);
		stage.update();
		players[this.index] = this;
		playersCount++;
	}
	function loadPlayers(playersData) {
		for (var j = 0 ; j < playersData.length ; j++) {
			if (playersData[j].id != currentPlayerIndex) 
				players[playersData[j].id] = new Ship(playersData[j]);
		}
	}
	function initPlayer(playerData) {
		gameCamera = new Camera();
		var playerId = playerData.id
		new Ship({ x:  playerData.x, y: playerData.y, id: playerId});
		currentPlayerIndex = playerId;
		UIEventGraphics = new UIContainer();
	}
	function handleTick(event) {
		for (key in players) {
			if (String(Number(key)) === key && players.hasOwnProperty(key)) {
				if (players[key].index == currentPlayerIndex)	players[key].cyclePlayer();
				else players[key].cycleNonPlayer();
			}
		}
		for (var i = 0 ; i < globalSpaceData.length ; i++) {
			globalSpaceData[i].tick(); 
		}
		for (var i = 0 ; i < spaceObjects.length ; i++) {
			spaceObjects[i].tick(); 
		}
		globalGameTick(); 
		stage.update();
	}
	function globalGameTick() {
		UIEventGraphics.tick(); 
		gameCamera.tick();
		gameBackground.tick();
	}
	function handleMouseEvent(event) {
		console.log(event); 
		var shape; 
		var target = event.target
		switch (event.type) {
			case "mouseover":
				console.log(event.target); 
				UIEventGraphics.drawBar(event.target.image.width, event.target.x, event.target.y, 100); 
			break;
			case "mouseout":
				console.log("remouve"); 
				UIEventGraphics.removeAllChildren(); 

			break; 
			case "click":
				alert("click !"); 
			break; 
			default: 
			break; 
		}
	}

	$("#connect").submit(function() {
		var nickname = $("#nickname").val(); 
		if (nickname && nickname.length > 3)
		{
			$("#connectionForm").fadeOut(0000, function() {
				var server = new ServerIO() ;
				server.connect(nickname);
				stage.enableMouseOver(10);
				createjs.Ticker.addListener(window);
				createjs.Ticker.useRAF = true;
				createjs.Ticker.setFPS(60);
				createjs.Ticker.addEventListener("tick", handleTick);
				$("#gameCanvas").fadeIn(0000); 
			}); 
		}
		return false; 
	});

	/***********************************
	* Fonctions d'entrées / sortie    **
	************************************/

	/* Evenement : touche relachée */
	document.onkeyup = function(e) {
		var code = e.KeyCode ? e.KeyCode : e.which;
		if(code == KEY_UP){
			hasPressedUp = false;
		}
		if(code == KEY_DOWN){
			hasPressedDown = false;
		}
		if(code == KEY_LEFT){
			hasPressedLeft = false;
		}
		if(code == KEY_RIGHT){
			hasPressedRight = false;
		}
	}
	/* Evenement : touche enfoncée */
	document.onkeydown = function(e){
		var code = e.KeyCode ? e.KeyCode : e.which;
		if(code == KEY_UP){
			hasPressedUp = true;
			hasPressedDown = false;
		}
		if(code == KEY_DOWN){
			hasPressedDown = true;
			hasPressedUp = false;
		}
		if(code == KEY_LEFT){
			hasPressedLeft = true;
			hasPressedRight = false;
		}
		if(code == KEY_RIGHT){
			hasPressedRight = true;
			hasPressedLeft = false;
		}
	}; 	

})(jQuery); 