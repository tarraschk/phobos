this.phobos = this.phobos || {};

(function () {

	Station = function(params){
		this.initialize(params);
	}

	if (server) var s = Station.prototype ;
	else var s = Station.prototype = new _.Bitmap();

// static public properties:
	Station.path = 'img/objects/stations/';
	
// public properties:
	s._damages = 200 // dommages que la station peut causer quand elle attaque;
	s._life; // vie totale de la station
	s._lifeLeft; // vie restante a la station
	s._target;
	s._isInspected;
	s._targetZ;
	s._mapZ;
	s._name;
	s.fucused = false;
	s.scannable;
	s._id;
	s.shared = {};
	s.local = {};
// constructor:
	s.initialize = function (params) {
		this.id = params.id;
		this.index = params.id;
		this._id = utils.generateId();
		this._targetZ = this._id;
		this._name = params.name;
		this.shared = { 
			id: params.id,
			index: params.id,
			position: {x: params.position.x, y: params.position.y, z:params.position.z, sector: params.position.sector, },
			type:"Station",
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
	s.getLifeInPercent = function(){
		return this._lifeLeft*100/this._life;
	}
	s.setMapCoords = function(params){
		this._mapX = params.x;
		this._mapY = params.y;
	}
	s.drawRender = function() {
		console.log("tick");
		var renderCoo = utils.absoluteToStd({x:this.shared.position.x,y:this.shared.position.y}, this.local.env.getGame().getCamera()._position);
		this.x = renderCoo.x;
		this.y = renderCoo.y;
	}
	s.tick = function () {
		this.shared.position.x = this.shared.position.x + 0.01;
		if (!server) this.drawRender();
	}
	s.takeDamage = function(shooter, d){
		this._target = shooter;
		this._lifeLeft -= d;
	}
	s.id = function(id){
		if(id != undefined){
			this._id = id;
			return this;
		}
		else{
			return this._id;
		}
	}
	s.load = function(){
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

	s.getPosition = function() {
		return this.getShared().position;
	}

	s.getSector = function() {
		return this.getPosition().sector; 
	}

	s.getId = function() {
		return this.id; 
	}

	s.getShared = function() {
		return this.shared;
	}
	s.name = function(name){
		if(name != undefined){
			this._name = name;
			return this;
		}
		else{
			return this._name;
		}
	}
	s.isInspected = function(is){
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
