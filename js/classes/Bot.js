(function (window) {

	Bot = function(params){
		this.initialize(params);
	}

	var b = Bot.prototype = window.Ship.prototype;
	
// static public properties:
	Bot.path = 'img/ship/';

// public properties:
	b.IA = "wait";
	b.IARange = 50;

// constructor:


// public methods:

	b.botTick = function() {
	}

	b.botTick = b.tick;
	b.tick = function() {
		this.botTick();
		switch(this.IA) {
			case "wait":
				var closeTarget = this.getCloseEnnemy();
				console.log("d=" + utils.distance(closeTarget, this));
				if (closeTarget) {
					if (utils.distance(closeTarget, this) < this.IARange) {

					}
				}
			break;
			default:
			break;
		}
	}

	window.Bot = Bot;

}(window));