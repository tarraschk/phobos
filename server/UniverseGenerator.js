
(function () {


	var UniverseGenerator  = Class.create({

	// constructor:

		initialize: function () { 
		},

		vonNeumanNumber: function(token, digits) {
			var ite1 = parseInt("651" + token + "245");
			var ite2 = ite1 * ite1;
			ite2 = (ite2.toString());
			var ite3 = ite2.substr(ite2.length / 2 - 2, 4);
			ite3 = parseInt(ite3);
			var ite4 = ite3 * ite3;


			ite4 = (ite4.toString());
			var ite5 = parseInt(ite4.substr(ite4.length / 2, digits));
			return (ite5);
		},
		generateSector: function(sectorId, token) {
			var countObj = 0 ;
			var collectablesNumber = this.vonNeumanNumber(token - 1,2);
			var botsNumber = this.vonNeumanNumber(token - 2,1);
			var stationsNumber = this.vonNeumanNumber(token,1);
			var tilesNumber = this.vonNeumanNumber(token + 1,2);
			console.log(collectablesNumber);
			console.log(botsNumber);
			console.log(stationsNumber);
			console.log(tilesNumber);
			var sector = {objects:[],tiles:[]};



		// 	//Collectables

		// 	for (var ii = 0 ; ii < collectablesNumber; ii++) {
		// 		var genX = this.vonNeumanNumber(token + countObj * 20, 3);
		// 		var genY = this.vonNeumanNumber(token - countObj  *20, 3);
		// 		sector.objects[countObj] = {
		// 			id:countObj,type:'Collectable', image: { src: 'Asteroid.png' },name: 'Minerai 1',position: {x: genX,y: genY, z:1, rotation: 10, sector: sectorId}, weight:10, dimensions: { width:218, height:181 }
		// 		}
		// 		countObj++;
		// 	};

		// 	//Stations
		// 	for (var ii = 0 ; ii < stationsNumber; ii++) {
		// 		var genX = this.vonNeumanNumber(token + countObj * 20, 3);
		// 		var genY = this.vonNeumanNumber(token - countObj * 20, 3);
		// 		sector.objects[countObj] = {
		// 			id:countObj,type:'Station', 
		// 			image: { src: 'Anna Cruiser.png' },
		// 			name: 'Station spatiale internationale',
		// 			position: {x: genX,y: genY, z:1, 
		// 			sector: sectorId},
		// 			life: 150000, 
		// 			dimensions: { width:218, height:181 } 
		// 		}
		// 		countObj++;
		// 	};

		// 	//Bots
		// 	for (var ii = 0 ; ii < botsNumber; ii++) {
		// 		var genX = this.vonNeumanNumber(token + countObj * 20, 3);
		// 		var genY = this.vonNeumanNumber(token - countObj  *20, 3);
		// 		sector.objects[countObj] = {
		// 			id:countObj, 
		// 			type:'Bot', 
		// 			src: 'Anna Cruiser.png',
		// 			name: 'Station spatiale internationale',
		// 			position: {x: genX,y: genY, z:1, rotation:-90, sector: sectorId},  
		// 			initPosition: {x: genX,y: genY, z:1, rotation:-90, sector: sectorId},
		// 			life: 150000, 
		// 			width:1032, 
		// 			height:620,
		// 			destination: {x:null, y:null},
		// 			limitSpeed: 1.5,
		// 			acceleration: 0.06 , 
		// 			limitRotation:0,
		// 			currentSpeed: 0 , 
		// 			rotationSpeed: 3,
		// 			hasDestination: false,
		// 			weapons: 2,
		// 			hasTarget: false , 
		// 			energy: 100,
		// 			targetType: null,
		// 			targetId: null,
		// 			AIStopRange: 600 , 
		// 			AIRange: 500,
		// 			AI:"wait",
		// 			name:null,
		// 			type:"Bot",
		// 		}
		// 		countObj++;
		// 	};

			
		// //Tiles 
		// for (var ll = 0 ; ll < tilesNumber; ll++) {
		// 	var genX = this.vonNeumanNumber(token + countObj * 20, 3);
		// 	var genY = this.vonNeumanNumber(token - countObj  *20, 3);
		// 	sector.tiles[countObj] = {	id:countObj,position:{x:genX,y:genY, z: 1}, src:"iso-05-03.png",};
		// }

		// for (var ll = 0 ; ll < 1; ll++) {
		// 	sector.tiles[ll] = {	id:ll,position:{x:Math.random() * 2500,y:Math.random() * 2500, z: 1}, src:"iso-05-03.png",};
		// }

var sector = {
			objects:[

			{id:14,type:'Collectable', image: { src: 'Asteroid.png' },name: 'Minerai 1',position: {x: 500,y: 600, z:1, rotation: 10, sector: 0}, weight:10, dimensions: { width:218, height:181 } },
			{id:14,type:'Collectable', image: { src: 'Asteroid.png' },name: 'Minerai 1',position: {x: 100,y: 300, z:1, rotation: 10, sector: 0}, weight:10, dimensions: { width:218, height:181 } },
			{id:13,type:'Collectable', image: { src: 'Asteroid.png' },name: 'Minerai 1',position: {x: 200,y: 600, z:1, rotation: 130, sector: 0}, weight:10, dimensions: { width:218, height:181 } },
			{id:12,type:'Collectable', image: { src: 'Asteroid.png' },name: 'Minerai 1',position: {x: 800,y: 100, z:1, rotation: 530, sector: 0}, weight:10, dimensions: { width:218, height:181 } },
			{id:11,type:'Collectable', image: { src: 'Asteroid.png' },name: 'Minerai 1',position: {x: 1500,y: 1000, z:1, rotation: 30, sector: 0}, weight:10, dimensions: { width:218, height:181 } },
			{id:10,type:'Collectable', image: { src: 'Asteroid.png' },name: 'Minerai 1',position: {x: 200,y: 1600, z:1, rotation: 30, sector: 0}, weight:10, dimensions: { width:218, height:181 } },
			{id:0,type:'Station', image: { src: 'Anna Cruiser.png' },name: 'Station spatiale internationale',position: {x: 1500,y: 600, z:1, sector: 0},life: 150000, dimensions: { width:218, height:181 } },
			{id:1, type:'Station', image: { src: 'stationIso.png' },name: 'Station spatiale internationale',position: {x: 500,y: 500, z:1, rotation:0, sector: 0},life: 150000, dimensions: { width:218, height:181 } },
			{
				id:6, 
				type:'Bot', 
				src: 'Anna Cruiser.png',
				name: 'Station spatiale internationale',
				position: {x: 920,y: 500, z:1, rotation:-90, sector:0},  
				initPosition: {x: 920,y: 500, z:1, rotation:-90, sector:0},
				life: 150000, 
				width:1032, 
				height:620,
				destination: {x:null, y:null},
				limitSpeed: 1.5,
				acceleration: 0.06 , 
				limitRotation:0,
				currentSpeed: 0 , 
				rotationSpeed: 3,
				hasDestination: false,
				weapons: 2,
				hasTarget: false , 
				energy: 100,
				targetType: null,
				targetId: null,
				AIStopRange: 600 , 
				AIRange: 500,
				AI:"wait",
				name:null,
				type:"Bot",
			},
			{
				id:2, 
				type:'Bot', 
				src: 'Anna Cruiser.png',
				name: 'Station spatiale internationale',
				position: {x: 1520,y: 700, z:1, rotation:-90, sector:0},  
				initPosition: {x: 1520,y: 500, z:1, rotation:-90, sector:0},
				life: 150000, 
				width:1032, 
				height:620,
				destination: {x:null, y:null},
				limitSpeed: 1.5,
				acceleration: 0.06 , 
				limitRotation:0,
				currentSpeed: 0 , 
				rotationSpeed: 3,
				hasDestination: false,
				weapons: 2,
				hasTarget: false , 
				energy: 100,
				targetType: null,
				targetId: null,
				AIStopRange: 600 , 
				AIRange: 500,
				AI:"wait",
				name:null,
				type:"Bot",
			},
			{
				id:3, 
				type:'Bot', 
				src: 'Anna Cruiser.png',
				name: 'Station spatiale internationale',
				position: {x: 520,y: 200, z:1, rotation:-90, sector:0},  
				initPosition: {x: -1520,y: 500, z:1, rotation:-90, sector:0},
				life: 150000, 
				width:1032, 
				height:620,
				destination: {x:null, y:null},
				limitSpeed: 1.5,
				acceleration: 0.06, 
				limitRotation:0,
				currentSpeed: 0 , 
				rotationSpeed: 3,
				hasDestination: false,
				weapons: 2,
				hasTarget: false , 
				energy: 100,
				targetType: null,
				targetId: null,
				AIStopRange: 600 , 
				AIRange: 500,
				AI:"wait",
				name:null,
				type:"Bot",
			},
			],
			tiles:[
			{	id:1,position:{x:Math.random() * 2500,y:Math.random() * 2500, z: 1, sector: 0}, src:"iso-02-04.png",},
			]
		};
		console.log(sector);
		// alert("error");
		return sector;
		},
		
	});  

	phobos.UniverseGenerator = UniverseGenerator;

}());
