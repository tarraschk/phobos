(function(window){
	Utils = function(){

	}

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

	u.distance = function (o1, o2) {
		return (Math.sqrt(Math.pow((o2.position.x - o1.position.x), 2) + Math.pow((o2.position.y - o1.position.y), 2)));
	}

}(window));