this.phobos = this.phobos || {};


(function () {


	var Weapon = Class.create({
		initialize: function(weaponId) {
			this._id = utils.generateId();
			this._power = 20;
			this._range = 350;
			this._weaponId = weaponId ;
			this._cooldown = new phobos.Cooldown();
		},

		tick: function(event) {
			this._cooldown.try();
			this.setReady(this._cooldown._ready);
			// if (Math.random() < 0.05) this._ready = true;
			// else this._ready = false ; 
		},

		doShoot: function(target, shooterPos) {
			if (!server)
				client.getGame()._gameGraphics.drawLaser(shooterPos, target, this._weaponId);
			this._ready = false ; 
			this._cooldown.start();
		},
		
		getRange: function() {
			return this._range ; 
		},

		isReady: function() {
			return this._ready;
		},

		setReady: function (newReady) {
			this._ready = newReady; 
		},

	});

	phobos.Weapon = Weapon;

}());