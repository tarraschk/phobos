
(function () {

	function UniverseGenerator() {
		this.initialize();
	}

	var ug = UniverseGenerator.prototype ;

	// constructor:

		ug.initialize = function () { 
		}

		ug.vonNeumanNumber = function(token, digits) {
			var ite1 = parseInt("651" + token + "245");
			var ite2 = ite1 * ite1;
			ite2 = (ite2.toString());
			var ite3 = ite2.substr(ite2.length / 2 - 2, 4);
			ite3 = parseInt(ite3);
			var ite4 = ite3 * ite3;


			ite4 = (ite4.toString());
			var ite5 = ite4.substr(ite4.length / 2, digits);
			return (ite5);
		}
		ug.generateSector = function(sectorId, token) {
			var countObj = 0 ;
			var collectablesNumber = this.vonNeumanNumber(token - 1,2);
			var botsNumber = this.vonNeumanNumber(token - 2,1);
			var stationsNumber = this.vonNeumanNumber(token,1);
			var tilesNumber = this.vonNeumanNumber(token + 1,1);
			console.log(collectablesNumber);
			console.log(botsNumber);
			console.log(stationsNumber);
			console.log(tilesNumber);
			var sector = {objects:[],tiles:[]};



			//Collectables

			for (var ii = 0 ; ii < collectablesNumber; ii++) {
				var genX = this.vonNeumanNumber(token + ll * 20, 2);
				var genY = this.vonNeumanNumber(token + ll  *20, 2);
				sector.objects[countObj] = {
					id:countObj,type:'Collectable', image: { src: 'Asteroid.png' },name: 'Minerai 1',position: {x: genX,y: genY, z:1, rotation: 10, sector: sectorId}, weight:10, dimensions: { width:218, height:181 }
				}
				countObj++;
			};

			//Stations
			for (var ii = 0 ; ii < stationsNumber; ii++) {
				var genX = this.vonNeumanNumber(token + ll * 20, 2);
				var genY = this.vonNeumanNumber(token + ll  *20, 2);
				sector.objects[countObj] = {
					id:countObj,type:'Station', 
					image: { src: 'Anna Cruiser.png' },
					name: 'Station spatiale internationale',
					position: {x: genX,y: genY, z:1, 
					sector: sectorId},
					life: 150000, 
					dimensions: { width:218, height:181 } 
				}
				countObj++;
			};

			//Bots
			for (var ii = 0 ; ii < botsNumber; ii++) {
				var genX = this.vonNeumanNumber(token + ll * 20, 2);
				var genY = this.vonNeumanNumber(token + ll  *20, 2);
				sector.objects[countObj] = {
					id:countObj, 
					type:'Bot', 
					src: 'Anna Cruiser.png',
					name: 'Station spatiale internationale',
					position: {x: genX,y: genY, z:1, rotation:-90, sector: sectorId},  
					initPosition: {x: genX,y: genY, z:1, rotation:-90, sector: sectorId},
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
				}
				countObj++;
			};

			
		//Tiles 
		for (var ll = 0 ; ll < tilesNumber; ll++) {
			var genX = this.vonNeumanNumber(token + ll * 20, 2);
			var genY = this.vonNeumanNumber(token + ll  *20, 2);
			sector.tiles[ll] = {	id:ll,position:{x:genX,y:genY, z: 1}, src:"iso-05-03.png",};
		}

		// for (var ll = 0 ; ll < 1; ll++) {
		// 	sector.tiles[ll] = {	id:ll,position:{x:Math.random() * 2500,y:Math.random() * 2500, z: 1}, src:"iso-05-03.png",};
		// }
		return sector;
		}
		
	// public methods:
	phobos.UniverseGenerator = UniverseGenerator;

}());
