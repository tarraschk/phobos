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
		cPlayground.addChild(this._container);
		this.objectSelected = null;
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
		this.objectSelected = objectSelected;
	}

	ui.showObjectSelectedInfos = function() {
		this.showObjectData(this.objectSelected);
		this.showObjectActions(this.objectSelected);
	}

	ui.showObjectData = function(object) {
		var data = object.shared;
		console.log(data);
	}

	ui.showObjectActions = function(object) {
		var actions = object.shared.actions ; 
		console.log(actions);
		debug(actions[0]);
		$("#minichat").html("<a href='#' onclick='ui.objectSelectedAction(\"dock\")'>" + actions[0] + "</a>");
	}

	ui.objectSelectedAction = function(action) {
		client.handleClientAction(action, this.objectSelected);
	}

	ui.drawStatusBar = function(target) {

		console.log("DRAW STATUS BAR");
		console.log(target);
		var g = new _.Graphics();
		g.beginStroke(_.Graphics.getRGB(50,205,10));
		g.setStrokeStyle(5,10,10);
		startPoint = {x: target.x + client.getGame().getCamera().x(), y:target.y + client.getGame().getCamera().y() };
		if (target.image)
			var endWidth = target.image.width;
		else if (target.spriteSheet) 
			var endWidth = target.spriteSheet._frameWidth
		endPoint = {x: target.x + client.getGame().getCamera().x() +  endWidth  , y:target.y + client.getGame().getCamera().y() };

		console.log(startPoint);
		console.log(endPoint);

		g.moveTo(startPoint.x,startPoint.y)
		.lineTo(endPoint.x,endPoint.y).endStroke();

		var s = new _.Shape(g);
		this._container.addChild(s);	
	}
	ui.drawSurround = function(target) {
		var g = new _.Graphics();
	    // g.setStrokeStyle(2);
	    // g.beginStroke("#069D1A");
	    var x = target.x + client.getGame().getCamera().x();
	    var y = target.y + client.getGame().getCamera().y();
	    g.beginRadialGradientFill(["rgba(49,138,36,0.8)", "rgba(71,201,87,0.3)"], [0.4, 0.6], x, y, 0, x, y, target.image.height);
	    g.drawEllipse(x , y, target.image.width, target.image.height);
	    g.endFill();
	    var s = new _.Shape(g);
	    this._container.addChild(s);
	}

	ui.showEntityInfos = function(entity){
		console.log(entity);
		this.drawStatusBar(entity);
		this.drawSurround(entity);
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
		this.clear();
	}

	ui.tick = function() {
		this._container.x = this._containerX - client.getGame().getCamera().x();
		this._container.y = this._containerY - client.getGame().getCamera().y();
	}

})(jQuery);
