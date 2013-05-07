(function($){

	UI = function(params){
		this.initialize(params);
	}

	var ui = UI.prototype;

	ui.initialize = function(params){

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