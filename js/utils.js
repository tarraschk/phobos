/* Geometric functions */

/* Switch from one vector space to another */

function absoluteToCamera (position, camera) {
	return ({
		x : position.x + camera.x,
		y : position.y + camera.y,
	});
}

function stdToIsometricScreen (position) {
	return ({
		x : (Math.sqrt(2) / 2) * ( position.x - position.y),
		y : 1/(Math.sqrt(6)) * (position.x + position.y),
	});
}

function isometricScreenToStd (position) {
	return ({
		x : (1/Math.sqrt(2)) * position.x - (Math.sqrt(6)/2) * position.y,
		y : - ((Math.sqrt(6) / 2) * position.y - ( 1 / Math.sqrt(2) ) * position.x),
	});
}

function cameraToAbsolute (position, camera) {
	return ({
		x : position.x - camera.x,
		y : position.y - camera.y,
	});
}
function absoluteToStd (position, camera) {
	return (stdToIsometricScreen(absoluteToCamera(position, camera)));
}
function stdToAbsolute (position, camera) {
	return (cameraToAbsolute(stdToIsometricScreen (position), camera));
}