(function($){

	UI = function(params){
		this.initialize(params);
	}

	var ui = UI.prototype;

	ui.initialize = function(params){

	}

	ui.openEntityInfos = function(entity){
		console.log(entity);
		var b = $(Mustache.render($('#tpl_entity_info_container').html(), {
			name: entity.name()
		}));
		b.appendTo($('body')).css({
			
		})
	}
})(jQuery);