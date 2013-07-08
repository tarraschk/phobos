this.phobos = this.phobos || {};

(function () {

	Collectable = function(params){
		this.initialize(params);
	}

	if (server) var c = Collectable.prototype ;
	else var c = Collectable.prototype = new _.BitmapAnimation();

// static public properties:
	Station.path = 'img/objects/collectables/';
	
// public properties:
	s._id;
	s.shared = {};
	s.local = {};
// constructor:
	s.initialize = function (params) {
		this.id = params.id;
		this.index = params.id;
		this.shared = { 
			id: params.id,
			index: params.id,
			position: {x: params.position.x, y: params.position.y, z: params.position.z },
			type:"Collectable",
			actions: ["dock"],
			dimensions: params.dimensions,
			image: {
				src:params.image.src,
				dim:500 //To do
			}
		 };
		if (!server) this.load();
		this._life = this._lifeLeft = params.life;
		if (server) this.local.env = server;
		else this.local.env = client;
	}

// public methods:
	c.getLifeInPercent = function(){
		return this._lifeLeft*100/this._life;
	}
	c.setMapCoords = function(params){
		this._mapX = params.x;
		this._mapY = params.y;
	}
	c.drawRender = function() {
		var renderCoo = utils.absoluteToStd({x:this.shared.position.x,y:this.shared.position.y}, this.local.env.getGame().getCamera()._position);
		this.x = renderCoo.x;
		this.y = renderCoo.y;
	}
	c.tick = function () {
		this.shared.position.x = this.shared.position.x + 0.01;
		if (!server) this.drawRender();
	}
	c.takeDamage = function(shooter, d){
		this._target = shooter;
		this._lifeLeft -= d;
	}
	c.id = function(id){
		if(id != undefined){
			this._id = id;
			return this;
		}
		else{
			return this._id;
		}
	}
	c.load = function(){
		if (!server) {
			this.image = new Image();
			this.image.src = Station.path+this.shared.image.src; 
			var that = this;
			this.image.onload = function() {
				that.addEventListener("mouseover", function(e) {
					ui.showEntityInfos(that);
				});
				that.addEventListener("mouseout", function(e) {
					ui.hideEntityInfos(that);
				});
				that.addEventListener("click", function(e){
					client.inputPlayer("mouse1Object",{ click:e, targObject: that})
				});
				cPlayground.addChild(that);
			}
		}
	}
	c.getShared = function() {
		return this.shared;
	}
	c.name = function(name){
		if(name != undefined){
			this._name = name;
			return this;
		}
		else{
			return this._name;
		}
	}
	c.isInspected = function(is){
		if(is != undefined){
			this._isInspected = is;
			return this;
		}
		else{
			return this._isInspected;
		}
	}
	phobos.Station = Station;

}());
