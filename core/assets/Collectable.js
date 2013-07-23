
this.phobos = this.phobos || {};


(function () {

	var Collectable = Class.create(phobos.SpaceObject, {

// static public properties:
	

// constructor:
	initialize: function($super,params) {
		this.local = {};
		if (server) this.local.env = server;
		else { 
			this.local.env = client;
			this.path = "img/objects/collectables/";
		}

		$super(params);

		this.shared = { 
			id: params.id,
			index: params.id,
			position: {x: params.position.x, y: params.position.y, z: params.position.z, rotation: params.position.rotation,
			sector: params.position.sector,  },
			type:"Collectable",
			name: params.name,
			weight:params.weight,
			actions: ["collect"],
			dimensions: params.dimensions,
			image: {
				animation: params.image.animation,
				src:params.image.src,
				dim:500 ,//To do,
				spritesheet: params.image.spritesheet,
			}
		 };
	},

// public methods:

	tick: function($super) {
		this.shared.position.rotation = this.shared.position.rotation + 1.5;
		$super();
	},

	});
	phobos.Collectable = Collectable;

}());
