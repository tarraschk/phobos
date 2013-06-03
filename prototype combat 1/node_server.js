var http = require('http');
var md5 = require('MD5');

var users = {};

httpServer = http.createServer(function(req, res){
	console.log('user connected');
});
httpServer.listen(1337);

var io = require('socket.io').listen(httpServer);
//A la connection d'un client
io.sockets.on('connection', function(socket){
	var me = false;
	console.log("Nouveau utilisateur");

	for(var k in users){
		socket.emit('newuser', users[k]);
	}

	/*
	connection
	*/
	socket.on('connect', function(user){
		me = user;
		me.id = user.id;
		me.x = user.x;
		me.y = user.y;
		socket.emit('connected');
		users[me.id] = me;
		io.sockets.emit('newuser', me);
	});

	/*
	deconnection
	*/
	socket.on('disconnect', function(){
		if(!me){
			return false;
		}
		delete users[me.id];
		io.sockets.emit('disuser', me);
	});

	//un joueur a bougé
	socket.on('move', function(user){
		io.sockets.emit('othermove', user);
	});
});
