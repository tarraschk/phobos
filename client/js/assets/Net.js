this.phobos = this.phobos || {};

(function(window){
	Net = function(socket){
		this.initialize(socket);
	}

	var net = Net.prototype;

	//a local queue of messages we delay if faking latency
	net._messages = [];
	net._socket ;

	//constructor
	net.initialize = function(socket){
		this._socket = socket;
	}


	net.sendMessage = function(message, data) {
		console.log("SEND MESSAGE");
		console.log(message);
		console.log(data);
    	socket.emit(message, data);
	}

	phobos.Net = Net;

}());
