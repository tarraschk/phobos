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

	ui.drawStatusBar = function(target) {

		console.log("DRAW STATUS BAR");
		var g = new _.Graphics();
		g.beginStroke(_.Graphics.getRGB(50,6,10));
		g.setStrokeStyle(5,10,10);
		startPoint = {x: target.x, y:target.y};
		endPoint = {x: target.x + target.width, y:target.y };
		g.moveTo(startPoint.x,startPoint.y)
		.lineTo(endPoint.x,endPoint.y).endStroke();
		var s = new _.Shape(g);
		this._container.addChild(s);	
	}
	ui.drawSurround = function(target) {
		var g = new _.Bitmap("img/ui/surround.png");
		var surroundDimensions = {w:296,h:167};
		g.x = target.x + client.getGame().getCamera().x();
		g.y = target.y + client.getGame().getCamera().y();
		if (target.image) {
		console.log(target.image.width );
		console.log(target.image.height );
			g.scaleX = 1.2 * target.image.width / surroundDimensions.w;
			g.scaleY = 1.1 * target.image.height / surroundDimensions.h;
		}
		else if (target.spriteSheet) {
			console.log(target.spriteSheet);
			g.scaleX = 1.2 * target.spriteSheet._regX / surroundDimensions.w;
			g.scaleY = 1.1 * target.spriteSheet._regY / surroundDimensions.h;
		}
		// g.beginStroke(_.Graphics.getRGB(50,6,10));
		// g.setStrokeStyle(5,10,10);
		// g.drawCircle(target.x ,target.y ,300);
		// startPoint = {x: target.x, y:target.y};
		// endPoint = {x: target.x + 200, y:target.y + 50};
		// g.moveTo(startPoint.x,startPoint.y)
		// .lineTo(endPoint.x,endPoint.y).endStroke();

		// var s = new _.Shape(g);
		this._container.addChild(g);
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