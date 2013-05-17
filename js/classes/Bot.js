this.phobos = this.phobos || {};
(function () {

	var Bo = function(params){
		console.log("caca");
		this.initialize(params);
	}
		console.log(phobos);
		console.log(_);

	var b = Bo.prototype = new phobos.Ship();
	
// static public properties:
	Bot.path = 'img/ship/';

// public properties:
	b.IA = "wait";
	b.IARange = 50;

// constructor:


// public methods:
	b.Ship_initialize = b.initialize;
	b.initialize = function(params) {
		this.Ship_initialize();
		console.log("init bot");
	}

	b.botTick = b.tick;
	b.tick = function() {
		console.log("bot tick");
		console.log(this);
		this.botTick();
		switch(this.IA) {
			case "wait":
				var closeTarget = this.getCloseEnnemy();
				if (closeTarget) {
					if (utils.distance(closeTarget, this) < this.IARange) {

					}
				}
			break;
			default:
			break;
		}
	}

	phobos.Bot = Bo;

}());