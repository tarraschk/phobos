<<<<<<< HEAD
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
			x : position.x + camera.x,
			y : position.y + camera.y,
		});
	}

	u.StdToIsometricScreen = function(position) {
		return ({
			x : (Math.sqrt(2) / 2) * ( position.x - position.y),
			y : 1/(Math.sqrt(6)) * (position.x + position.y),
		});
	}

	u.isometricScreenToStd = function(position) {
		return ({
			x : (Math.sqrt(2) / 2) * ( position.x - position.y),
			y : 1/(Math.sqrt(6)) * (position.x + position.y),
		});
	}

	u.cameraToAbsolute = function(position, camera) {
		return ({
			x : position.x - camera.x,
			y : position.y - camera.y,
		});
	}
}(window));
=======
>>>>>>> e995ae553bef147c26da4b0f21523cddf2803be4
