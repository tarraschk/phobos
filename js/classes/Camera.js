(function(window){

	function Camera(position) {
		this._position = null;
		this.initialize();
	};

	Camera.prototype.initialize = function (position) {
		var that = this;
		this._position = (position == undefined) ? {x:0,y:0} : position;
		$(window).on('keydown', function(e){
			that.move({'type': 'key', e:e});
		});
		$(window).on('mousemove', function(e){
			that.move({'type': 'mouse', e:e})
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
	Camera.prototype.move = function(params) {
		if(params.type == "key"){
			var code = (params.e.keyCode ? params.e.keyCode : params.e.which);
			if(code == KEY.LEFT){
				debug('camera left');
				this._position.x -= 1; // * speed etc.... ?
			}
			if(code == KEY.UP){
				debug('camera up');
				this._position.y -= 1; // * speed etc.... ?
			}
			if(code == KEY.DOWN){
				debug('camera down');
				this._position.y += 1; // * speed etc.... ?
			}
			if(code == KEY.RIGHT){
				debug('camera right');
				this._position.x += 1; // * speed etc.... ?
			}
		}
		if(params.type = 'mouse'){
			if(mouse.x <= 6){
				debug('camera left');
				this._position.x -= 1; // * speed etc.... ?
			}
			if(mouse.x >= window.innerWidth - 6){
				debug('camera right');
				this._position.x += 1; // * speed etc.... ?
			}
			if(mouse.y <= 6){
				debug('camera up');
				this._position.y -= 1; // * speed etc.... ?
			}
			if(mouse.y >= window.innerHeight - 6){
				debug('camera down');
				this._position.y += 1; // * speed etc.... ?
			}
		}
	};
	Camera.prototype.tick = function (event) {

	};

	window.Camera = Camera;
}(window));