(function($){

	UI = function(params){
		this.initialize(params);
	}

	var ui = UI.prototype;

	ui.initialize = function(params){
		this.renderTargetWrapper();
		this._container = new _.Container();
		cPlayground.addChild(this._container);
	}

	ui.renderTargetWrapper = function(data) {
		//Data to define
		
	}

	ui.newStationElement = function() {
		var b = $(Mustache.render($('.station_main_container').html(), {
			name: "station",
			id: 1
		}));
		b.appendTo($('#body')).css({
			left: 300,
			top: 400
		}).slideDown(300);
	}

	ui.clear = function() {
		this._container.removeAllChildren();
	}

	ui.drawStatusBar = function(target) {
		var g = new _.Graphics();
		g.beginStroke(_.Graphics.getRGB(50,6,10));
		g.setStrokeStyle(5,10,10);
		g.drawCircle(target.x,target.y,30);
		startPoint = {x: target.x, y:target.y};
		endPoint = {x: target.x + 200, y:target.y + 50};
		g.moveTo(startPoint.x,startPoint.y)
		.lineTo(endPoint.x,endPoint.y).endStroke();

		var s = new _.Shape(g);
		this._container.addChild(s);
	}

	ui.showEntityInfos = function(entity){
		console.log("Show entity");
		console.log(entity);
		this.drawStatusBar(entity);
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
})(jQuery);