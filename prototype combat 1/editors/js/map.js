
window._ = window.createjs;
$.fn.placeHolder = function(options){
    var defauts = {
        placeholder: 'placeholder'
    };
    var params = $.extend(defauts, options);
    return this.each(function(){
        params['placeholder'] = $(this).attr('data-placeholder') != undefined  ? $(this).attr('data-placeholder') : params['placeholder'];
        $(this).val(params['placeholder']);
        $(this).on('focus', function(e){
            if($(this).val() == params['placeholder'])
                $(this).val('');
        });
        $(this).on('blur', function(e){
            if($(this).val() == ''){
            	$(this).val(params['placeholder']);
            	$(this).attr('data-empty', true);
            }
            if($(this).val() == params['placeholder']){
            	$(this).attr('data-empty', true);
            }
            if($(this).val() != params['placeholder']){
            	$(this).attr('data-empty', false);
            }
        });
    });
};

var grounds = "../../img/grounds/",
	backgrounds = "../../img/backgrounds/",
	simple = grounds+"others/",
	asteroids = grounds+"asteroids/",
	fog = grounds+"fog/",
	raw = grounds+"raw/",
	bgv = backgrounds+"void/",
	bgo = backgrounds+"orbits/",
	imgPath = {
		empty: simple+"iso-01.png",
		selected: simple+"isoself.png",
		over: simple+"isoportee.png",
	},
	imgs = {},
	imgsOk = false,
	nb_loaded = 0,
	nb_paths = 3,
	cPlayground = new _.Stage("playground"),
	cBackground = new _.Stage("background"),
	// map = {};
	map = {
		"data": [
			[0,1,0,2,5,8],
			[0,5,0,1,5,3]
		]
	};
var actions = {
	switchBackground: function(el, e){
		
	},
	saveMap: function(el, e){
		var mapName = $("#mapName").val();
		if($("#mapName").attr('data-empty') == "false"){
			toJson(mapName);
		}
	},
	saveAs: function(el, e){
		var p = $('[data-popup="'+$(el).attr('data-action')+'"]')
		p.css({
			top: window.innerHeight/2 - p.innerHeight()/2,
			left: window.innerWidth/2 - p.innerWidth()/2,
		}).show(0);
	},
	openMenu: function(el, e){
		e.preventDefault();
		e.stopPropagation();
		$('.item_list:visible').slideUp(100);
		$('.menu .label.active').removeClass('active');
		var il = $('[data-menu="'+$(el).attr('data-target')+'"]');
		if(!$(el).hasClass('active')){
			il.css({
				'left': $(el).offset().left,
				'top': $(el).offset().top+$(el).innerHeight()
			});
			il.stop().slideDown(50, function(){
				$(el).addClass('active');
			});
		}
		if($(el).hasClass('active')){
			il.stop().slideUp(50, function(){
				$(el).removeClass('active');
			});
			
		}
	}
};

function resize(){
	$('#panel').css({
		'height': window.innerHeight,
	});
	$('header').css({
		"width": window.innerWidth
	});
	$('canvas').each(function(){
		this.width = window.innerWidth;
		this.height = window.innerHeight;
	});
}

function generatePaths(){
	//asteroids
	for(var i = 0 ; i < 12 ; i++){
		nb_paths++;
		if(i < 10)
			imgPath['ast'+i] = asteroids+"iso-02-0"+i+".png";
		else
			imgPath['ast'+i] = asteroids+"iso-02-"+i+".png";
	}
	//fogs
	for(var i = 1 ; i < 11 ; i++){
		nb_paths++;
		if(i < 10)
			imgPath['fog'+i] = fog+"iso-05-0"+i+".png";
		else
			imgPath['fog'+i] = fog+"iso-05-"+i+".png";
	}
	//raws blue
	for(var i = 1 ; i < 12 ; i++){
		nb_paths++;
		if(i < 10)
			imgPath['rb'+i] = raw+"iso-03-0"+i+".png";
		else
			imgPath['rb'+i] = raw+"iso-03-"+i+".png";
	}
	//raws red
	for(var i = 0 ; i < 11 ; i++){
		nb_paths++;
		if(i < 10)
			imgPath['rr'+i] = raw+"iso-07-0"+i+".png";
		else
			imgPath['rr'+i] = raw+"iso-07-"+i+".png";
	}
	//background void
	for(var i = 0 ; i < 32 ; i++){
		nb_paths++;
		imgPath['bgv'+i] = bgv+i+".jpg";
	}
	//background orbits
	for(var i = 0 ; i < 27 ; i++){
		nb_paths++;
		imgPath['bgo'+i] = bgo+i+".jpg";
	}
}

function loadImgs(){
	$('#panel .title .loading .lab').html('Loading ressources...').parent().fadeIn(100);
	for(var img in imgPath){
		// console.log(img, imgPath[img]);
		var image = new Image();
		image.src = imgPath[img];
		image.onload = function(){
			nb_loaded++;
			if(nb_loaded == nb_paths){
				imgsOk == true;
				$('#panel .title .loading').hide(0);
			}
		}
		imgs[img] = image;
	}
}

function toJson(fileName){
	$('#panel .title .loading .lab').html('Saving map...').parent().show(0);
	map['name'] = fileName;
	$.ajax({
		url: '../php/io-json.php',
		type: 'POST',
		dataType: 'json',
		data: {
			mapName: fileName,
			map: JSON.stringify(map)
		},
  		success: function(data, textStatus, xhr) {
			if(data.status == 1){
				$('#panel .title .loading .lab').html('Map saved !').parent().fadeOut(3000);
			}
			if(data.status == -1){
				$('#panel .title .loading .lab').html('Error during saving map !');
			}
		},
		error: function(xhr, textStatus, errorThrown) {
			//called when there is an error
		}
	});
}

function fromJson(path){

}

jQuery(document).ready(function($) {
	$('.action_bton').on('click', function(e){
        if(actions[$(this).attr('data-action')])
        (actions[$(this).attr('data-action')])(this, e);
    });
	$('.placeholderme').placeHolder();
	cPlayground.enableMouseOver(10);
	cPlayground.mouseMoveOutside = true;
	resize();
	generatePaths();
	loadImgs();
});
$(document).on('click', function(e){
	e.stopPropagation();
	$('.menu .label').removeClass('active');
	$('.context_menu').slideUp(100);
});
$(window).resize(function(e){
	resize();
});