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
	var KEY_DOWN	= 40;
	var KEY_UP		= 38;
	var KEY_LEFT	= 37;
	var KEY_RIGHT	= 39;
	/***********************
	* Variables globales **
	************************/
	var socket = io.connect('http://localhost:1337');
	var imgShip = new Image(); 
	var stage = new createjs.Stage("gameCanvas");
	var players = [];
	var currentPlayer;
	var playerRunning = false;
	var plIndex = 0; 
	var circle;
	var hasPressedUp = false;
	var hasPressedDown = false;
	var hasPressedRight = false;
	var hasPressedLeft = false;
	
	function ServerIO() {
		
		this.connect = function() {
			socket.emit('playerLogin', {}); 
		}
		this.syncPlayers = function() {
			socket.emit('loadPlayers');
			socket.on('playersLoaded', function(playersData) {
				players = playersData;
			});
		} 
		this.send = function(toSend, args, callback, callbackData) {
			console.log("emit !"); 
			socket.emit(toSend, args) ; 
			if (callback) {
				socket.on(callback, function(callbackData) {
					//return (callbackData);
				});
			}	
		}
		socket.on('newPlayer', function(playersData) {
			plIndex++;
			console.log("new player : ");
			console.log(playersData); 
			var newPlayer = new Ship(playersData) ; 
			playerRunning = true;
			players[plIndex] = newPlayer;
		}); 
		socket.on('playerLogged', function(playersData) {
			plIndex++;
			console.log("logged : ");
			console.log(playersData); 
			currentPlayer = new Ship(playersData) ; 
			playerRunning = true;
			players[plIndex] = currentPlayer;
			console.log("return this ");
			console.log(currentPlayer);
			return currentPlayer;
		});
		
		this.getNewShip = function() {
			console.log("get it !") ;
			console.log(currentPlayer);
			return currentPlayer;
		}
		
		this.cycleServer = function() {
			this.syncPlayers();
			if (Math.random() < 0.05) {
				socket.emit('tickPlayer', {
					/*index: this.currentShip.index,
					username: this.currentShip.shipImg.name,
					x: this.currentShip.shipImg.x,
					y: this.currentShip.shipImg.y */
				}); 
			}
		}
	}
	
	function Ship(shipData)
	{
		this.cycleShip = function() {
		}
		this.cyclePlayer = function() {
			if (hasPressedUp) {
				this.shipImg.y -= 5 ; 
			}
			else if (hasPressedDown) {
				this.shipImg.y += 5 ; 
			}
			else {
			}
			if (hasPressedLeft) {
				this.shipImg.x -= 5 ; 
			}
			else if (hasPressedRight) {
				this.shipImg.x += 5 ; 
			}
			else {
			}
		}
		/*
		*Constructeur 
		*/
		var spriteSheet = new createjs.SpriteSheet({
			// image to use
			images: [imgShip], 
			// width, height & registration point of each sprite
			frames: {width: 75, height: 95, regX: 0, regY: 0}, 
			animations: {    
				walk: [0, 1, "walk"]
			}
		});
		this.index = shipData.id; 
		this.shipImg = new createjs.Shape();
		this.shipImg.graphics.beginFill("red").drawCircle(0, 0, 40);
		//Set position of Shape instance.
		this.shipImg.x = shipData.x;
		this.shipImg.y = shipData.y;
		this.doLine = false;
		this.shipImg.name = "default";
		this.shipImg.direction = 90;
		this.shipImg.vX = 0;
		this.shipImg.vY = 0; 
		this.shipImg.rotation = 0;
		this.shipImg.scaleX = 1;
		this.shipImg.scaleY = 1;
		this.shipImg.currentFrame = 0;
		stage.addChild(this.shipImg);	
		stage.update();
	}




	/************************
	* Pseudo classe Game **
	*************************
	Génère une partie de jeu 
	@param int essaimCount
	*/
	function Game()
	{
		this.loadPlayers = function () {
			var players = currentIO.send('loadPlayers', null, 'playersLoaded');
		}
	
		/* Lance la partie courante : génère les essaims, lance les cycle des jeu.*/
		this.launch = function() {
			stage = new createjs.Stage("gameCanvas");
			imgShip.src = "img/ship.png";	
			currentIO = new ServerIO() ;
			currentIO.connect();
			this.loadPlayers();
			mainShip = currentPlayer; 
			
			createjs.Ticker.addListener(window);
			createjs.Ticker.useRAF = true;
			createjs.Ticker.setFPS(60);
			createjs.Ticker.addEventListener("tick", handleTick);
		}
		/* Game cycle */
		function handleTick(event) {
			if (playerRunning) {
				mainShip = currentPlayer; 
				stage.update();	
				for (var i = 1 ; i < players.length ; i++) {
					if (players[i] != currentPlayer) {
						players[i].cycleShip();
					}
					else players[i].cyclePlayer();
				}
				currentIO.cycleServer();
			}
		}
		var playersCount;
		var currentIO;
		var mainShip; 
	}

	/************************
	* Entrée du programme  **
	*************************
	*/
	function init()
	{
		myGame = new Game();
		myGame.launch();
	}

	/*************************
	* Cycle du programme    **
	**************************/

	function tick() 
	{	
		console.log("tick");
		myGame.tick();
	}

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
	init() ; 
})(jQuery); 