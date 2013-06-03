(function (window) {

	Station = function(params){
		this.initialize(params);
	}

	var s = Station.prototype = new _.Bitmap();

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
// constructor:
	s.initialize = function (params) {
		this._id = utils.generateId();
		this._targetZ = this._id;
		this._name = params.name;
		this.setMapCoords({x: params.x, y: params.y});
		this.load(params.src);
		this._life = this._lifeLeft = params.life;
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
		var renderCoo = utils.absoluteToStd({x:this._mapX,y:this._mapY}, game._camera._position);
		this.x = renderCoo.x;
		this.y = renderCoo.y;
	}
	s.tick = function (event) {
		this._mapX = this._mapX + 0.001;
		this.drawRender();
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
		this.image = new Image();
		this.image.src = Station.path+src; 
		var that = this;
		this.image.onload = function() {
			that.addEventListener("mouseover", function(e) {
				debug('over '+that._name);
				that.manageMouseOver();
			});
			that.addEventListener("mouseout", function(e) {
				debug('out of '+that._name);
				that.manageMouseOut();
			});
			that.addEventListener("click", function(e){
				that.manageClick();
			});
			cPlayground.addChild(that);

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
	s.manageClick = function(){
		allowMoveClick = false ; 
		debug('arrimage '+this._name);
		game._playerShip.dockTo(this);
		//afficher dans le hud en tant que cible potentielle
	}
	//Affiche le nom et la barre de vie de la station
	s.manageMouseOver = function(){
		ui.showEntityInfos(this);
	}
	//cache le nom et la barre de vie de la station
	s.manageMouseOut = function(){
		ui.hideEntityInfos(this);
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
	window.Station = Station;

}(window));
