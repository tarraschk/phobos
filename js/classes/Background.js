(function (window) {

	Background = function(){
		this._path = 'img/backgrounds/';
		this._stage = new _.Stage('background');
		this._image;
		this._stage.scaleX = 1;
		this._stage.scaleY = 1;
		this._stage.width = 1;
		this._stage.height = 1;
		return this;
	}

	Background.prototype.load = function(src){
		var src = this._path+src;
		console.log('loading background', src);
		this._imgTmp = new Image();
		this._imgTmp.src = src;
		that = this;
		this._imgTmp.onload = function(){
			that._image  = new _.Bitmap(this)
			that._stage.addChild(that._image);
			that._stage.update();
		}
		console.log('loaded');
	}

	Background.prototype.tick = function(){
		
	}
	window.Background = Background;
}(window));

