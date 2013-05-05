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
}(window));