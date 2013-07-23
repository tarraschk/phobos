
(function () {

	var DBHandler = Class.create({

	// constructor:

		initialize: function () { 
			this._mongoose = require('mongoose');
			this._connectionString = "mongodb://localhost/test";
			this.connect();
		},
		connect: function() {
			this.mongoose.connect(this.connectionString);
			console.log("CONNEXION SUCCESS");
		},
		
	});
	// public methods:
	phobos.DBHandler = DBHandler;
}());
