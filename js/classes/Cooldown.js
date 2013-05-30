this.phobos = this.phobos || {};
(function (phobos) {

	function Cooldown() {
		this.initialize();
	}
	var c = Cooldown.prototype ;

//Public propreties 
	c._date;
	c._time;
	c._ready;
// constructor:

	c.initialize = function () {
		this._time = 750;
		this._date = new Date();
		this._ready = false ; 
	}

// public methods:

	c.try = function () {
		var current = new Date();
		var interval = new Date();
		interval.setTime(current.getTime() - this._date.getTime()); 
		if (interval.getMilliseconds() >= this._time) {
			this._ready = true ; 
		}
	}

	c.start = function() {
		this._date = new Date();
		this._ready = false ; 
	}

	c.tick = function() {
		if (!this._ready) {
			this.try();
		}
	}

	phobos.Cooldown = Cooldown;

}(phobos));