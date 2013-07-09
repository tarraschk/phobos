(function($){

	UI = function(params){
		this.initialize(params);
	}

	var ui = UI.prototype;

	ui.initialize = function(params){
		this._containerX = 0;
		this._containerY = 0 ; 
		this.renderTargetWrapper();
		this._container = new _.Container();
		this._objectSelected = null;
		this._locked = false;
		cPlayground.addChild(this._container);
	}

	ui.renderTargetWrapper = function(data) {
		//Data to define
		
	}

	ui.newStationElement = function() {
		debug("New station element");
		// var b = $(Mustache.render($('.station_main_container').html(), {
		// 	name: "station",
		// 	id: 1
		// }));
		// b.appendTo($('#body')).css({
		// 	left: 300,
		// 	top: 400
		// }).slideDown(300);
	}

	ui.clear = function() {
		this._container.removeAllChildren();
	}

	ui.setObjectSelected = function(objectSelected) {
		if (!this._locked) {
			this._objectSelected = objectSelected;
			this.setLocked(true);
		}
		else  {
			this.unselectObjectSelected();
			this.setLocked(false);
		}

	}

	ui.unselectObjectSelected = function() {
		this.clearActions();
		this._objectSelected = null;
	}

	ui.showObjectSelectedInfos = function() {
		this.showObjectData(this._objectSelected);
		this.showObjectActions(this._objectSelected);
	}

	ui.showObjectData = function(object) {
		var data = object.shared;
		console.log(data);
	}

	ui.clearActions = function() {
		$("#minichat").html("");
	}

	ui.showObjectActions = function(object) {
		var actions = object.shared.actions ; 
		console.log(actions);
		debug(actions[0]);
		$("#minichat").html("<a href='#' onclick='ui.objectSelectedAction(\"" +actions[0] +"\")'>" + actions[0] + "</a>");
	}

	ui.objectSelectedAction = function(action) {
		client.handleClientAction(action, this._objectSelected);
	}

	ui.drawStatusBar = function(target) {
		if (target.spriteSheet)
			var isSprite = true;
		else 
			var isSprite = false;

		var g = new _.Graphics();
		g.beginStroke(_.Graphics.getRGB(50,205,10));
		g.setStrokeStyle(5,10,10);
		if (!isSprite) {
			var endWidth = target.image.width;
			var startPoint = {x: target.x + client.getGame().getCamera().x(), y:target.y + client.getGame().getCamera().y() };
			var endPoint = {x: target.x + client.getGame().getCamera().x() +  endWidth  , y:target.y + client.getGame().getCamera().y() };
		}
		else {
			var endWidth = target.spriteSheet._frameWidth * target.scaleX
		
			var startPoint = {x: target.x + client.getGame().getCamera().x() - target.spriteSheet._regX / 3, y:target.y + client.getGame().getCamera().y()  - target.spriteSheet._regY / 3 };
			var endPoint = {x: target.x + client.getGame().getCamera().x() +  endWidth - target.spriteSheet._regX / 3  , y:target.y + client.getGame().getCamera().y() - target.spriteSheet._regY / 3 };
			}


		g.moveTo(startPoint.x,startPoint.y)
		.lineTo(endPoint.x,endPoint.y).endStroke();

		var s = new _.Shape(g);
		this._container.addChild(s);	
	}
	ui.drawSurround = function(target) {
		if (target.spriteSheet)
			var isSprite = true;
		else 
			var isSprite = false;
		var g = new _.Graphics();
	    g.setStrokeStyle(2);
	    g.beginStroke("#069D1A");
	    if (!isSprite) {
	    	var targetWidth = target.image.width;
	    	var targetHeight = target.image.height;
		    var x = target.x + client.getGame().getCamera().x();
		    var y = target.y + client.getGame().getCamera().y();
	    }
	    else {
	    	var targetWidth = target.spriteSheet._frameWidth * target.scaleX;
	    	var targetHeight = target.spriteSheet._frameHeight * target.scaleY;
		    var x = target.x - target.spriteSheet._regX / 3 + client.getGame().getCamera().x();
		    var y = target.y - target.spriteSheet._regY / 3+ client.getGame().getCamera().y();
	    }
	    // g.beginRadialGradientFill(["rgba(49,138,36,0.8)", "rgba(71,201,87,0.3)"], [0.4, 0.6], x, y, 0, x, y, target.image.height);
	    g.drawEllipse(x , y, targetWidth, targetHeight);
	    g.endFill();
	    var s = new _.Shape(g);
	    this._container.addChild(s);
	}

	ui.setLocked = function(locked) {
		this._locked = locked;
	}

	ui.showEntityInfos = function(entity){
		if (!this._locked) {
			this.drawStatusBar(entity);
			this.drawSurround(entity);
		}
		// var b = $(Mustache.render($('#tpl_entity_info_container').html(), {
		// 	name: entity.name(),
		// 	id: entity.id()
		// }));
		// b.appendTo($('#body')).css({
		// 	left: mouse.x,
		// 	top: mouse.y
		// }).slideDown(300);
	}
	ui.hideEntityInfos = function(entity){
		// $("#"+entity.id()).slideUp(100, function(){
		// 	$(this).remove();
		// });
		if (!this._locked)
			this.clear();
	}

	ui.tick = function() {
		this._container.x = this._containerX - client.getGame().getCamera().x();
		this._container.y = this._containerY - client.getGame().getCamera().y();
	}

})(jQuery);
