
this.phobos = this.phobos || {};

(function(){
	Utils = function(){ }

	var u = Utils.prototype;

	u.generateId = function(){
		var l = 11;
		var id = "";
		var abc = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		for(var i = 0 ; i < (l - 1) ; i++){
			id += abc[Math.floor(Math.random()*61)];
		}

		return id;
	}

	/* Geometric functions */

	/* Switch from one vector space to another */



	u.absoluteToCamera = function(position, camera) {
		return ({
			x : position.x - camera.x,
			y : position.y - camera.y,
		});
	}

	u.stdToIsometricScreen = function(position) {
		return ({
			x : (Math.sqrt(2) / 2) * ( position.x - position.y),
			y : 1/(Math.sqrt(6)) * (position.x + position.y),
		});
	}

	u.isometricScreenToStd = function(position) {
		return ({
			x : (1/Math.sqrt(2)) * position.x + (Math.sqrt(6)/2) * position.y,
			y : ((Math.sqrt(6) / 2) * position.y - ( 1 / Math.sqrt(2) ) * position.x),
		});
	}

	u.cameraToAbsolute = function(position, camera) {
		return ({
			x : position.x + camera.x,
			y : position.y + camera.y,
		});
	}
	u.absoluteToStd = function(position, camera) {
		return ((this.absoluteToCamera(this.stdToIsometricScreen(position), camera)));
	}
	u.stdToAbsolute = function (position, camera) {
		return (this.isometricScreenToStd (this.cameraToAbsolute(position, camera)));
	}

	u.isSameZ = function (o1,o2) {
		if (o1.shared && o2.shared)
			o1 = o1.shared;
			o2 = o2.shared
		return (o1.position.z == o2.position.z);
	}

	u.distance = function (o1, o2) {
		if (o1.shared || o2.shared) {
			o1 = o1.shared;
			o2 = o2.shared;
		}
		return (Math.sqrt(Math.pow((o2.position.x - o1.position.x), 2) + Math.pow((o2.position.y - o1.position.y), 2)));
		
	}

	u.getDiffPosition = function(o1, o2) {
		return ({dX: Math.abs(o2.x - o1.x), dY: Math.abs(o2.y - o1.y)}); 
	}

	phobos.Utils = Utils;

}());