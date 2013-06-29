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
	s._mapX;
	s._mapY;
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
		console.log("station"); 
		this._id = utils.generateId();
		this._targetZ = this._id;
		this._name = params.name;
		this.shared.position = {x: params.x, y: params.y};
		this.setMapCoords({x: params.x, y: params.y});
		if (!server) this.load(params.src);
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
		var renderCoo = utils.absoluteToStd({x:this._mapX,y:this._mapY}, this.local.env.getGame().getCamera()._position);
		this.x = renderCoo.x;
		this.y = renderCoo.y;
	}
	s.tick = function () {
		this._mapX = this._mapX + 0.001;
		if (!server) this.drawRender();
	}
	s.shoot = function(target){
		if(utils.range(target, this) < 200){// si la cible est assez près on lui tire dessus

		}
		else{
			this._target = null;
		}
	}
	s.takeDamage = function(shooter, d){
		this._target = shooter;
		this._lifeLeft -= d;
	}
	s.setBackgroundSrc = function(newSrc) {

	}
	s.onOver = function(){
		//display name
	}
	s.onClick = function(){
		//gerer le clic
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
	s.load = function(src){
		if (!server) {
			this.image = new Image();
			this.image.src = Station.path+src; 
			var that = this;
			this.image.onload = function() {
				that.addEventListener("mouseover", function(e) {
					ui.showEntityInfos(that);
				});
				that.addEventListener("mouseout", function(e) {
					ui.hideEntityInfos(that);
				});
				that.addEventListener("click", function(e){
					allowMoveClick = false ; 
					debug('arrimage '+that._name);
					client.getGame().getPlayerShip().dockTo(that);
				});
				cPlayground.addChild(that);
			}
		}
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
