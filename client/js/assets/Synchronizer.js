
phobos = this.phobos || {};


(function () {

	function Synchronizer() {
		this.initialize();
	}
	var s = Synchronizer.prototype ;


	// constructor:

	s.initialize = function () { 

	}

	
// public methods:


	c.diffShip = function(shipServ, shipClient, nowServ, nowClient) {
		var positionServ = shipServ.shared.position;
		var positionClient = shipClient.shared.position;
		var dPos = utils.getDiffPosition(positionServ, positionClient);
		var dT = nowClient - nowServ;

	}

	c.sync = function(frameServer, serverData) {
		var frameClient = this.getGame().getFrame();

		servShips = serverData.ships;
		servObjects = serverData.objects;


		for (key in servShips) {
			if (String((key)) === key && servShips.hasOwnProperty(key)) {
				if (servShips[key]) {
					if (servShips[key].index == servShips[key].id) {
						this.diffShip(servShips[key], this.getGame().getShipsList()[key], frameServer, frameClient);
						this.getGame().getShipsList()[key].shared.position = servShips[key].shared.position;
					}
				}
			}
		}
		for (key in servObjects) {
			if (String((key)) === key && servObjects.hasOwnProperty(key)) {
				if (servObjects[key]) {
					if (servObjects[key].index == servObjects[key].id) {
					// servObjects[key].tick();
					}
				}
			}
		}
	}

	phobos.Synchronizer = Synchronizer;

}());
