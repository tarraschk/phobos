this.phobos = this.phobos || {};


(function () {

	var Tile = Class.create(phobos.SpaceObject, {

// static public properties:


// constructor:
	initialize: function ($super, params) {
		console.log(" new tile");
		$super(params);
	},

// public methods:

	tick: function($super) {
		$super();
		console.log("tile ticking");
	},

	load: function($super){
		$super(params);
	},

	});
	phobos.Tile = Tile;

}());