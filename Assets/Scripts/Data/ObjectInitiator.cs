using UnityEngine;
using System.Collections;
using SimpleJSON; 

//Initiate's the object with remote data. 

public class ObjectInitiator : MonoBehaviour {

	private Phobos.JSONReader dataParser ; 
	public bool isPlayerInitiator = false; 
	public string objectDataLocation = "C:/Users/Public/Documents/Unity Projects/Phobos/Assets/Resources/GameData/Crafting/Recipes/player1.json"; 
	
	private GameController gameController ; 
	private Destructible playerDestr ; 
	private Crafter playerCraft ; 
	private Cargohold playerCargo ; 
	private PlayerStats playerStats ; 
	private ShipController playerController ; 
	private Controls playerControls ; 
	private Turrets playerTurrets ; 
	
	//When object is initialized, we consider we have the JSON. 
	void Awake () {
		this.initControllers(); 
		this.dataParser = new Phobos.JSONReader(); 
		SimpleJSON.JSONClass objectData = this.dataParser.readAndParseJSON(this.objectDataLocation);
		JSONNode ship = objectData["ship"];
		JSONNode shipModel = ship["model"];
		JSONNode shipEquipment = ship["equipment"];
		if (this.isPlayerInitiator) {
			JSONNode player = objectData["player"];
			this.setPlayerStats(player); 
		}
		this.setShipStats(shipModel); 
		this.setShipEquipment(shipEquipment); 
	}
	
	private void initControllers() {
		playerCraft = (Crafter) this.GetComponent(typeof(Crafter)); 
		playerController = (ShipController) this.GetComponent(typeof(ShipController));
		playerDestr = (Destructible) this.GetComponent(typeof(Destructible));
		playerCargo = (Cargohold) this.GetComponent(typeof(Cargohold));	 
		playerStats = (PlayerStats) this.GetComponent(typeof(PlayerStats));		
		playerTurrets = (Turrets) this.GetComponent(typeof(Turrets));		
	}
	
	private void setPlayerStats(SimpleJSON.JSONNode player) {
		string levelStr = (string) player["stats"]["level"]; 
		int level = int.Parse(levelStr); 
		string currentXPStr = (string) player["stats"]["currentXP"]; 
		int currentXP = int.Parse(levelStr); 
		string requiredXPStr = (string) player["stats"]["requiredXP"]; 
		int requiredXP = int.Parse(levelStr); 
		playerStats.nick = (string) player["nick"]; 
		playerStats.level = level;
		playerStats.currentXP = currentXP; 
		playerStats.requiredXP = requiredXP ;
	}
	
	private void setShipStats(SimpleJSON.JSONNode shipModel) {
		string cargo = (string) shipModel["cargo"]; 
		string energy = (string) shipModel["energy"]; 
		string modelName = (string) shipModel["name"]; 
		this.loadModel(modelName); 
		
	}
	
	private void loadModel(string model) {
		GameObject theModel = (GameObject) Resources.Load ("Prefabs/Ships/" + model + "/Model") ; 
		
		GameObject instModel = (GameObject) Instantiate(theModel, (this.transform.position), theModel.transform.rotation) ;
		instModel.transform.parent = gameObject.transform; 
		instModel.name = Phobos.Vars.MODEL; 
	}
	
	private void setShipEquipment (SimpleJSON.JSONNode shipEquipment) {
		playerTurrets.initiateEquipment(); 
		string addedPort;
		SimpleJSON.JSONNode currentEquip ; 
		//Foreach equipment port, we add the turret. 
		for(var i = 0 ; i < Phobos.Gameplay.EQUIPMENT_MAX ; i++) {
			currentEquip = shipEquipment[Phobos.Vars.PORT + i]; 
			addedPort = (string) currentEquip; 
			if (currentEquip != null) {
				playerTurrets.pushEquipment(addedPort); 
			}
		}
	}
}
