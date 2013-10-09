using UnityEngine;
using System.Collections;
using SimpleJSON; 

public class ContainLoot : Photon.MonoBehaviour {
	
	
	public GameObject loot;
	private ObjectStats lootStats; 
	
	/*
	 * When loot is created, we pick up random item for this class of item
	 */
	void Awake () { 
		this.lootStats = (ObjectStats) this.GetComponent(typeof(ObjectStats));
		this.pickRandomLoot(); 
	}
	
	private void pickRandomLoot() {
		var currentLevel = this.lootStats.level.ToString(); 
		var dataParser = new Phobos.JSONReader(); 
		SimpleJSON.JSONClass objectData = dataParser.readAndParseJSON(Phobos.DataPaths.LOOTS);
		JSONNode objectsAvailable = (objectData["levels"][currentLevel]); 
		string pick = (string) Random.Range(0, objectsAvailable.Count - 1).ToString(); 
		var pickedItem = objectsAvailable[pick]; 
		string pickedItemLocation = (string) pickedItem["prefab"]; 
		this.loot = (GameObject) Resources.Load (pickedItemLocation) ; 
	}
	
	public void activateLootSpawn(int quantity) {
		
		if (PhotonNetwork.isMasterClient && PhotonNetwork.room != null)
        {
			this.spawnLoot(); 
			
		}
	}
	
	private void spawnLoot() {
		var current = gameObject; 
		
		Vector3 pos = this.transform.position; 
		Quaternion rot = this.transform.rotation; 
        int id = PhotonNetwork.AllocateViewID();
		DataManager dataScript = GameController.getDataManager();
		dataScript.addObjectToScene("Resources/Crystal", pos, rot, ObjectsSpawnTypes.collectable); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
