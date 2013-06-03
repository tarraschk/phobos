this.phobos = this.phobos || {};


(function () {

	Weapon = function(owner, weaponId){
		this.initialize(owner, weaponId);
	}

	var w = Weapon.prototype ;

// static public properties:
	Weapon.path = 'img/objects/stations/';
	
// public properties:
	w._owner = null ; 
	w._power = null // dommages que la station peut causer quand elle attaque;
	w._id = null;
	w._weaponId = null ; 
	w._range = 350 ; 
	w._ready = false;
	w._cooldown = null;
// constructor:
	w.initialize = function (owner, weaponId) {
		this._owner = owner ;
		this._id = utils.generateId();
		this._power = 20;
		this._weaponId = weaponId ;
		this._cooldown = new phobos.Cooldown();
	}

// public methods:
	w.tick = function (event) {
		this._cooldown.try();
		this.setReady(this._cooldown._ready);
		// if (Math.random() < 0.05) this._ready = true;
		// else this._ready = false ; 
	}

	w.doShoot = function(target) {
		// game._gameGraphics.drawLaser(this._owner, target, this._weaponId);
		this._ready = false ; 
		this._cooldown.start();
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

	phobos.Weapon = Weapon;

}());