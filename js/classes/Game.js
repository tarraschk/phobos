(function (window) {

	function Game() {
		this._isReady = false;
		this._started = false;
		this._engine = null;

		this.initialize();
	}


// constructor:
	this.Container_initialize = this.initialize;	//unique to avoid overiding base class

	Game.prototype.initialize = function () {
		resize();
		background = new Background().load("void/secteur7.jpg");
		var s = new Station({
			src: 'stationIso.png',
			name: 'Station spatiale internationale',
			x: 0,
			y: 0
		});
		ship = new Ship({id:1, x:155,y:35});
	}

// public methods:

	Game.prototype.tick = function (event) {
	}

	window.Game = Game;

}(window));