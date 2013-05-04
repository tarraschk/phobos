jQuery(document).ready(function($) {
	game = new Game();
	game.launchTicker();
});
function debug(data){
	$('<div>').html(data+'<br/>').prependTo($('#debug'));
}
function resize(){

	$('canvas').each(function(){
		this.width = window.innerWidth;
		this.height = window.innerHeight;
	});
	/*$('canvas').width(window.innerWidth);
	$('canvas').height(window.innerHeight);*/
	/*var canvas = document.getElementById("background"); 
	canvas.width  = window.innerWidth;
	canvas.height = window.innerHeight;*/

}

function handleTick() {
}

function renderCanvas() {
	cBackground.update();
	cPlayground.update();
}

$(document).on('mousemove', function(e){
	e.preventDefault();
	var x = e.clientX;
	var y = e.clientY;
	mouse.dx = x - mouse.ox;
	mouse.dy = y - mouse.oy;
	mouse.x = e.clientX;
	mouse.y = e.clientY;
	mouse.ox = x;
	mouse.oy = y;

});
$(document).on('mouseup', function(e){
	e.preventDefault();
	mouse.x = e.clientX;
	mouse.y = e.clientY;
	mouse.down = true;
	mouse.up = false;
});
$(document).on('mousedown', function(e){
	e.preventDefault();
	mouse.x = e.clientX;
	mouse.y = e.clientY;
	mouse.down = false;
	mouse.up = true;
});
$(window).on('resize', function(){
	//resize();
});
$(document).on('click', function(e){
	e.preventDefault();
	return false;
});