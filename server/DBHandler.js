
(function () {

	function DBHandler() {
		this.initialize();
	}

	var db = DBHandler.prototype ;

	db.mongoose = require('mongoose');
	db.connectionString = "mongodb://localhost/test";

	// constructor:

		db.initialize = function () { 
			this.connect();
		}
		db.connect = function() {
			this.mongoose.connect(this.connectionString);
			console.log("CONNEXION SUCCESS");
		}

		
	// public methods:
	phobos.DBHandler = DBHandler;

}());
