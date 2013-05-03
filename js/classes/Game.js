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
		background = new Background();
		background.load("void/secteur7.jpg");
	}

// public methods:

	Game.prototype.tick = function (event) {
	}

	window.Game = Game;

}(window));