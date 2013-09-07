using UnityEngine;
using System.Collections;
/**
 * 
 * This script manages the network for this bot
 * 
*/
public class ObjectNetscript : MonoBehaviour {
	
	/**
	 * Spawn object 
	 * 
	 */
	
	public ObjectsSpawnTypes spawnType ; 
	
	void Awake()
    {
		this.setNameToID();
		this.addToDataManager(); 
		this.addToObjectsHierarchy(); 
	}
	
	/**
	 * Let's rename the object to the View ID of this bot. 
	 */
	private void setNameToID() {
		PhotonView viewScript = (PhotonView) this.GetComponent(typeof(PhotonView));
		this.setName(this.name + "#" + viewScript.viewID);
	}
	
	/**
	 * Replace the object directly onto the objects hierarchy.
	 */
	private void addToObjectsHierarchy() {
		this.transform.parent = GameObject.FindGameObjectWithTag(Phobos.Vars.OBJECTS_TAG).transform; 	
	}
	
	/**
	 * CAPITAL
	 * Add this object to the data manager for future manipulation
	 */
	private void addToDataManager() {
		PhotonView viewScript = (PhotonView) this.GetComponent(typeof(PhotonView));
		int viewID = viewScript.viewID ; 
		
		DataManager DM =  GameController.getDataManager();
		DM.netObjects.Add(viewID, gameObject.transform); 	
	}
	
	/**
	 * Sets the name. 
	 */
	private void setName(string newName) {
		gameObject.name = newName ; 	
	}
	
}
