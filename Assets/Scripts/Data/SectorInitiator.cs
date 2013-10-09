using UnityEngine;
using System.Collections;
using SimpleJSON; 

public class SectorInitiator : MonoBehaviour {

	private Phobos.JSONReader dataParser ; 
	public string sectorDataFolder = "C:/Users/Public/Documents/Unity Projects/Phobos/Assets/Resources/GameData/Sector/";
	public string sectorDataLocation ; 
	private SimpleJSON.JSONClass sectorData; 
	private DataManager sectorDataManager ; 
	
	/**
	 * Spawn sector initiator
	 * 
	 */
    void Awake()
    {
		this.sectorDataManager = GameController.getDataManager(); 
		this.sectorDataLocation = this.sectorDataFolder + Application.loadedLevelName + ".json"; 
    }
	
	/**
	 * WHEN JOINED ROOM : 
	 * We spawn object data 
	 * ONLY IF WE ARE THE MASTER
	 * Or else, this script is useless : scene is already on place
	 */
    
    void OnJoinedRoom()
    {
		if (PhotonNetwork.isMasterClient) {
			//We have to spawn the local data in the scene. 
			this.readAndInitSectorData(); 
		}      
    }
	
	private void readAndInitSectorData() {
		this.dataParser = new Phobos.JSONReader(); 
		this.sectorData = this.dataParser.readAndParseJSON(this.sectorDataLocation);
		this.sectorData = (JSONClass) this.sectorData[Application.loadedLevelName]; 
		Debug.Log (this.sectorData);
		JSONNode spawnpoints = this.sectorData["spawnpoints"];
		JSONNode objects = this.sectorData["objects"];
		JSONNode resources = this.sectorData["resources"];
		JSONNode decoration = this.sectorData["decoration"];
		this.initSpawnpoints(spawnpoints); 
		this.initObjects(objects); 
	}
	
	private void initObjects(JSONNode objects) {
		SimpleJSON.JSONNode currentObj ; 
		Debug.Log (objects);
		//Foreach equipment port, we add the turret. 
		for(var i = 0 ; i < objects.Count ; i++) {
			currentObj = objects[i];
			this.initObjectFromJSON(currentObj); 
		}
	}
	
	private void initObjectFromJSON(JSONNode initObject) {
		string objPrefab;
		string objXStr;
		string objYStr;
		string objZStr;
		string objSpawnTypeStr;
		ObjectsSpawnTypes objSpawnType ; 
		int objX;
		int objY;
		int objZ;
		Vector3 pos ; 
		objPrefab = (string) initObject["prefab"]; 
		objSpawnTypeStr = (string) initObject["spawntype"]; 
		objXStr = (string) initObject["position"]["x"]; 
		objYStr = (string) initObject["position"]["y"]; 
		objZStr = (string) initObject["position"]["z"]; 
		objX = int.Parse(objXStr); 
		objY = int.Parse(objYStr); 
		objZ = int.Parse(objZStr); 
		pos = new Vector3(objX, objZ, objY); 
		objSpawnType = Phobos.MainLib.stringToSpawnType(objSpawnTypeStr); 
		Debug.Log (objSpawnType); 
		this.sectorDataManager.addObjectToScene(objPrefab, pos, Phobos.Vars.ROTATION_DEFAULT, objSpawnType); 
	}
	
	
	private void initSpawnpoints(JSONNode spawnpoints) {
		string SPX;
		string SPY;
		SimpleJSON.JSONNode currentSP ; 
		//Foreach equipment port, we add the turret. 
		for(var i = 0 ; i < spawnpoints.Count ; i++) {
			currentSP = spawnpoints[i];
			Debug.Log (currentSP);
			SPX = (string) currentSP["x"]; 
			SPY = (string) currentSP["y"];
			Debug.Log (SPX);
			Debug.Log (SPY);
		}
	}
	
	
	private void initResources(JSONNode resources) {
		
		
	}
	
	private void initDecoration(JSONNode decoration) {
		
		
	}
}
