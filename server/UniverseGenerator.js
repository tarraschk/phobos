
(function () {

	function UniverseGenerator() {
		this.initialize();
	}

	var ug = UniverseGenerator.prototype ;

	// constructor:

		ug.initialize = function () { 
		}
		ug.generateSector = function(token) {
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
		for (var ll = 0 ; ll < 1; ll++) {
			sector.tiles[ll] = {	id:ll,position:{x:Math.random() * 2500,y:Math.random() * 2500, z: 1}, src:"iso-05-03.png",};
		}
		return sector;
		}
		
	// public methods:
	phobos.UniverseGenerator = UniverseGenerator;

}());
