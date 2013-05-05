/* Geometric functions */

/* Switch from one vector space to another */

function absoluteToCamera (position, camera) {
	return ({
		x : position.x + camera.x,
		y : position.y + camera.y,
	});
}

function StdToIsometricScreen (position) {
	return ({
		x : (Math.sqrt(2) / 2) * ( position.x - position.y),
		y : 1/(Math.sqrt(6)) * (position.x + position.y),
	});
}

function isometricScreenToStd (position) {
	return ({
		x : (Math.sqrt(2) / 2) * ( position.x - position.y),
		y : 1/(Math.sqrt(6)) * (position.x + position.y),
	});
}

function cameraToAbsolute (position, camera) {
	return ({
		x : position.x - camera.x,
		y : position.y - camera.y,
	});
}