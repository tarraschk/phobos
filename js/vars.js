/*
Ficher des variables globales accessibles à l'ensemble du projet.
*/

//liste des code de touche clavier
var KEY = {
		LEFT: 37,
		UP: 38,
		RIGHT: 39,
		DOWN: 40,
		SPACE: 32,
		ENTER: 13,
		ESC: 27,
		PAUSE: 80,
		"0": 96,
		"1": 97,
		"2": 98,
		"3": 99,
		"4": 100,
		"5": 101,
		"6": 102,
		"7": 103,
		"8": 104,
		"9": 105
	},
	//objet mouse contenant les variables de déplacement de la souris
	// Déjà pris en charge avec easelJS ?
	mouse = {
		x: 0,
		y: 0,
		dx: 0,
		dy: 0,
		ox: 0,
		oy: 0,
		up: true,
		down: false
	},
	screenWidth = 960,
	screenHeight = 540,
	game = null;
	playground = new createjs.Stage("playground"); 
	background = new createjs.Stage("background");
	window._ = window.createjs;
	