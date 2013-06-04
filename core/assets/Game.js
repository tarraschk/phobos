
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
	}

	g.startUpdate = function() {

		setInterval(function(){
	        this.tick();
    	}.bind(this), 1);
	}

// public methods:

	g.tick = function (event) {
		t  = Date.now() ; 
	    //Work out the delta time
	    this.dt = this.lastframetime ? ( (t - this.lastframetime)/1000.0) : 0.016;
		console.log(this.dt);

	        //Store the last frame time
	    this.lastframetime = t;

	    this.objectsTick();
	}
	g.objectsTick = function() {
		allowMoveClick = true ; 
		// for (key in g._shipsList) {
		// 	if (String(Number(key)) === key && g._shipsList.hasOwnProperty(key)) {
		// 		if (g._shipsList[key].index == g._shipsList[key].id) g._shipsList[key].tick();
		// 	}
		// }
		// // g._gameGraphics.tick();
		// // g._camera.tick();
		// g._station1.tick();
		// g._station2.tick();
		// g._shipsList[3].moveTo({x:-150,y:-200});
		// for (var k = 0 ; k < g._tilesMap.length ; k++) {
		// 	g._tilesMap[k].tick();
		// }
		// renderCanvas();
		// backgroundGame.tick();
		// backgroundGame2.tick();
		// backgroundGame3.tick();
		// if (Math.random() < 0.01) console.clear();
	}
	phobos.Game = Game;

}());
