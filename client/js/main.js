jQuery(document).ready(function($) {
	socket = io.connect('http://localhost:4112');
	ui = new UI();
	net = new Net();
	client = new phobos.Client();
	client.loginToServer(); 
	client.loadGameData();
	client.startGame();
	
	socket.on('loggedIn', function(data){
		console.log("received");
		console.log(data);
		client.mainPlayerLogged(data); 
	});

	socket.on('sectorPlayersLoaded', function(data){
		var shipsList =  data; 
		console.log("received ships");
		console.log(shipsList);
		client.loadSectorPlayers(shipsList); 
	});
	
	socket.on('sectorPlayersLoaded', function(data){
		var shipsList =  data; 
		console.log("received ships");
		console.log(shipsList);
		client.loadSectorPlayers(shipsList); 
	});
});
function debug(data){
	$('<div>').html(data+'<br/>').prependTo($('#debug'));
}
function resize(){
	$('canvas').each(function(){
		this.width = window.innerWidth;
		this.height = window.innerHeight;
	});
	//Prevents click. Hud won't be resized ? 
	//$('#hud').width(window.innerWidth).height(window.innerHeight);
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

