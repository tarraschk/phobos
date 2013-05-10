(function (window) {

	function Map(player) {
		this.initialize();
	}
	var m = Map.prototype ;

	m._map = [];

// constructor:
	m.initialize = function () {
		
	}
// public methods:
	//récupère les infos de la map a 2k pixels autours de la caméra
	m.serverToMap = function(){

	}

	//mets a jours les donées locales de la map
	m.updateMap = function(){

	}

	// dessine la map a l'écran
	m.draw = function(){

	}

	m.tick = function (event) {
		this.draw();
	}
	window.Game = Game;

}(window));
 