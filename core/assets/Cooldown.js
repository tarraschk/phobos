
this.phobos = this.phobos || {};

(function () {

var Cooldown = Class.create({

	initialize: function () {
		this._time = 750;
		this._date = new Date();
		this._ready = false ; 
	},

// public methods:

	try: function () {
		var current = new Date();
		var interval = new Date();
		interval.setTime(current.getTime() - this._date.getTime()); 
		if (interval.getMilliseconds() >= this._time) {
			this._ready = true ; 
		}
	},

	start: function() {
		this._date = new Date();
		this._ready = false ; 
	},

	tick: function() {
		if (!this._ready) {
			this.try();
		}
	},
});
	phobos.Cooldown = Cooldown;

}());