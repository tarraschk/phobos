(function(window){

	function Camera(position) {
		this._position = null;
		this._moving = false;
		this._move = '';
		this._borderWidth = 6;
		this._SPEED = 1;
		this.initialize();
	};

	Camera.prototype.initialize = function (position) {
		var that = this;
		this._position = (position == undefined) ? {x:0,y:0} : position;
		$(document).on('keydown', function(e){
			that.checkMoving(e);
		});
		$(document).on('keyup', function(e){
			var code = (e.keyCode ? e.keyCode : e.which);
			if(code == KEY.UP ||
				code == KEY.LEFT ||
				code == KEY.DOWN ||
				code == KEY.RIGHT)
				that._moving = false;
		});
		$(document).on('mousemove', function(e){
			that.checkMoving(e);
		});
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
		switch(e.type){
			case 'keydown':
				var code = (e.keyCode ? e.keyCode : e.which);
				switch(code){
					case KEY.UP:
						this._move = 'UP';
						this._moving = true;
						break;
					case KEY.LEFT:
						this._move = 'LEFT';
						this._moving = true;
						break;
					case KEY.DOWN:
						this._move = 'DOWN';
						this._moving = true;
						break;
					case KEY.RIGHT:
						this._move = 'RIGHT';
						this._moving = true;
						break;
				}
				break;
			case 'mousemove': 
				if(mouse.x <= this._borderWidth){
					this._move = 'LEFT';
					this._moving = true;
				}
				if(mouse.x >= window.innerWidth - this._borderWidth){
					this._move = 'RIGHT';
					this._moving = true;
				}
				if(mouse.y <= this._borderWidth + 4){
					this._move = 'UP';
					this._moving = true;
				}
				if(mouse.y >= window.innerHeight - this._borderWidth){
					this._move = 'DOWN';
					this._moving = true;
				}
				if(mouse.x > this._borderWidth &&
					mouse.x < window.innerWidth - this._borderWidth &&
					mouse.y > this._borderWidth &&
					mouse.y < window.innerHeight - this._borderWidth){
					this._moving = false;
				}
				break;
		}
	};
	Camera.prototype.move = function(dir) {
		//debug('camera '+dir);
		switch(dir){
			case 'LEFT':
				this._position.x -= this._SPEED;
				break;
			case 'RIGHT':
				this._position.x += this._SPEED;
				break;
			case 'UP':
				this._position.y -= this._SPEED;
				break;
			case 'DOWN':
				this._position.y -= this._SPEED;
				break;
		}
		console.log(this._position);
	};
	Camera.prototype.tick = function (event) {
		if(this._moving){
			this.move(this._move);
		}
	};

	window.Camera = Camera;
}(window));