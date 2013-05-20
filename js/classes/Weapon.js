(function (window) {

	Weapon = function(){
		this.initialize();
	}

	var w = Weapon.prototype ;

// static public properties:
	Weapon.path = 'img/objects/stations/';
	
// public properties:
	w._power = null // dommages que la station peut causer quand elle attaque;
	w._id = null;
	w._range = 350 ; 
	w._ready = false;
// constructor:
	w.initialize = function () {
		this._id = utils.generateId();
		this._power = 20;
	}

// public methods:
	w.tick = function (event) {
		if (Math.random() < 0.05) this._ready = true;
		else this._ready = false ; 
	}
	
	w.getRange = function() {
		return this._range ; 
	}

	w.isReady = function() {
		return this._ready;
	}

	w.setReady = function (newReady) {
		this._ready = newReady; 
	}

	window.Weapon = Weapon;

}(window));