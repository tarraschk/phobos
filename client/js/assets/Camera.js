
phobos = this.phobos || {};


(function(){

	function Camera(position) {
		this._position = null;
		this._borderWidth = 6;
		this._SPEED = 30;
		this.initialize();
		this._keyIsLeft = false;
		this._mouseIsLeft;
		this._keyIsRight = false;
		this._mouseIsRight;
		this._keyIsUp = false;
		this._mouseIsUp;
		this._keyIsDown = false;
		this._mouseIsDown;
	};

	Camera.prototype.initialize = function (position) {
		var that = this;
		this._position = (position == undefined) ? {x:0,y:0} : position;
		$(document).on('keydown', function(e){
			var code = (e.keyCode ? e.keyCode : e.which);
			if(code == KEY.UP) that._keyIsUp = true;
			if(code == KEY.DOWN) that._keyIsDown = true;
			if(code == KEY.LEFT) that._keyIsLeft = true;
			if(code == KEY.RIGHT) that._keyIsRight = true;
		});
		$(document).on('keyup', function(e){
			var code = (e.keyCode ? e.keyCode : e.which);
			if(code == KEY.UP) that._keyIsUp = false;
			if(code == KEY.DOWN) that._keyIsDown = false;
			if(code == KEY.LEFT) that._keyIsLeft = false;
			if(code == KEY.RIGHT) that._keyIsRight = false;
		});
		/*$(document).on('mousemove', function(e){
			that.checkMoving(e);
		});*/
	};
	Camera.prototype.x = function(x) {
		if(x != undefined){
			this._position.x = x;
			return this;
		}
		else{
			return this._position.x;
		}
	};
	Camera.prototype.y = function(y) {
		if(y != undefined){
			this._position.y = y;
			return this;
		}
		else{
			return this._position.y;
		}
	};
	// public methods:
	Camera.prototype.checkMoving = function(e) {
		if(mouse.x <= this._borderWidth){
			this._mouseIsLeft = true;
		}
		if(mouse.x > this._borderWidth){
			this._mouseIsLeft = false;
		}
		if(mouse.x >= window.innerWidth - this._borderWidth){
			this._mouseIsRight = true;
		}
		if(mouse.x < window.innerWidth - this._borderWidth){
			this._mouseIsRight = false;
		}
		if(mouse.y <= this._borderWidth + 4){
			this._mouseIsUp = true;
		}
		if(mouse.y > this._borderWidth + 4){
			this._mouseIsUp = false;
		}
		if(mouse.y >= window.innerHeight - this._borderWidth){
			this._mouseIsDown = true;
		}
		if(mouse.y < window.innerHeight - this._borderWidth){
			this._mouseIsDown = false;
		}
	};
	Camera.prototype.tick = function (event) {
		if(this._keyIsRight || this._mouseIsRight) this._position.x += this._SPEED;
		if(this._keyIsLeft || this._mouseIsLeft) this._position.x -= this._SPEED;
		if(this._keyIsUp || this._mouseIsUp) this._position.y -= this._SPEED;
		if(this._keyIsDown || this._mouseIsDown) this._position.y += this._SPEED;

		//Center on player
		// this._position.x = client.getGame().getPlayerShip().x;
		// this._position.y = client.getGame().getPlayerShip().y;

		//Vibration de la mort que personne ne comprend
		/*var newCoo = utils.stdToIsometricScreen(this._position);
		this._position.x = newCoo.x ; 
		this._position.y = newCoo.y ; */
	};

	phobos.Camera = Camera;
}());
