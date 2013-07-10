(function () {

	function Universe() {
		this.initialize();
	}
	var u = Universe.prototype ;
	u._isReady = false;
	u._started = false;
	u._engine = null;
	u._playerShip = null;
	u._sectors = [];
	u._objectsList = [] ; 
	u._tilesList = [] ;
	u._dockedShipsList = [] ; 
	u._destroyedObjectsList = []; 
	u._killedShipsList = [] ; 
	u._shipsList = [];
	u._players = [];
	u._gameGraphics = null ; 
	u._updateTime = 15 ; 
	u._frame ;

// constructor:
	this.Container_initialize = this.initialize;	//unique to avoid overiding base class

	/* DATA ENTRY TO SPECIFY !!! */
	u.initialize = function () {
		this._frame = 0 ; 
		this._players = [];
		this._shipsList = [];
		this._dockedShipsList = [];
		this._killedShipsList = [];
	}

	phobos.Universe = Universe;

}());
