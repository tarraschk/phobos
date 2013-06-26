jQuery(document).ready(function($) {
	socket = io.connect('http://localhost:8080');
	// socket = io.connect('http://phobosproto.jit.su');
	ui = new UI();
	net = new Net();
	client = new phobos.Client();
	client.loginToServer(); 
	client.createPingTimer(); 
	client.createServerLoop(); 
	client.loadGameData();
	client.startGame();
	
	socket.on('loggedIn', function(data){
		client.mainPlayerLogged(data); 
	});

	socket.on('sectorPlayersLoaded', function(shipsList){
		client.loadSectorPlayers(shipsList); 
	});

	socket.on('setBotBehavior', function(data) {
		client.setBotBehavior(data.newBehavior, data.bot, data.data);
	})

	socket.on('sectorLoaded', function(sector){
		client.loadSector(sector); 
	});

	socket.on('sectorDataLoaded', function(sector){
		client.loadSector(sector); 
	});

	socket.on('playerMove', function(move){
		client.onPlayerMove(move); 
	});

	socket.on('newPlayerLoggedIn', function(player) {
		client.newPlayerLogged(player); 
	});

	socket.on('pong', function() {
		client.onPong();

	})

	socket.on('sync', function(data) {
		console.log("sync");
		console.log(data);

	})
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

