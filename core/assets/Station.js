
this.phobos = this.phobos || {};


(function () {

	var Station = Class.create(phobos.SpaceObject, {

// static public properties:
	

// constructor:
	initialize: function ($super, params) {
		console.log("load station");
		console.log(params);
		 this.local = {};
		if (server) this.local.env = server;
		else { 
			this.local.env = client;
			this.path = "img/objects/stations/";
		}
		$super(params);

		this.shared = { 
			id: params.id,
			index: params.id,
			position: {x: params.position.x, y: params.position.y, z:params.position.z, sector: params.position.sector, },
			type:params.type,
			actions: params.actions,
			dimensions: params.dimensions,
			image: {
				src:params.image.src,
				dim:500 //To do
			}
		 };
	},

// public methods:
	tick: function ($super) {
		this.getShared().position.x = this.getShared().position.x + 0.01;
		$super();
	},


	getShared: function() {
		return this.shared;
	},

});

	phobos.Station = Station;

}());
