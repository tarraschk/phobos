(function($){

	UI = function(params){
		this.initialize(params);
	}

	var ui = UI.prototype;

	ui.initialize = function(params){

	}

	ui.renderTargetWrapper = function() {

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

	ui.showEntityInfos = function(entity){
		var b = $(Mustache.render($('#tpl_entity_info_container').html(), {
			name: entity.name(),
			id: entity.id()
		}));
		b.appendTo($('#body')).css({
			left: mouse.x,
			top: mouse.y
		}).slideDown(300);
	}
	ui.hideEntityInfos = function(entity){
		$("#"+entity.id()).slideUp(100, function(){
			$(this).remove();
		});
	}
})(jQuery);