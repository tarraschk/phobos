(function (window) {

	function Map(player) {
		this.initialize();
		return this;
	}
	var m = Map.prototype ;

	m._map = [];
	m._tiles = [];
// constructor:
	m.initialize = function () {
		this.generateArea()
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
		for (var k = 0 ; k < this._tiles.length ; k++) {
			this._tiles[k].tick();
		}
	}

	m.generateArea = function(){
		for (var j = 0 ; j < 2000 ; j++) {
			this._tiles[j] = new Tile({
				id:1,
				x:Math.random() * 4000,
				y:Math.random() * 4000,
				src:"iso-02-04.png",
			});
		}
	}

	m.tick = function (event) {
		this.draw();
	}
	window.Map = Map;

}(window));
 