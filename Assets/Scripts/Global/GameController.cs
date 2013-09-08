using UnityEngine;
using System.Collections;
/**
 * 
 * The GameController is used for all accessions / get functions
 * To access the content of the game. Most  important functions
 * can be used as static methods. 
*/
public class GameController : MonoBehaviour  {
	
	/** 
	 * get all Players (objects with tag player), as an array, of this scene. 
	 * */
	public static GameObject[] getPlayers() {
		return (GameObject.FindGameObjectsWithTag(Phobos.Vars.PLAYER_TAG));	
	}
	
	/** 
	 * get the player container, containing all players.  
	 */
	public static GameObject getPlayerContainer() {
		return (GameObject.FindGameObjectWithTag(Phobos.Vars.PLAYERS_TAG));	
	}
	
	/** 
	 * Find the current universe of this scene. 
	 */
	public static GameObject findUniverse() {
		return  GameObject.FindGameObjectWithTag(Phobos.Vars.UNIVERSE_TAG); 	
	}
	
	/** 
	 * get the current player we can control, if there is any. 
	 * */
	public Transform getPlayer() {
		Controls controlScript = (Controls) this.GetComponent(typeof(Controls));
		if (controlScript.player) 
			return controlScript.player; 
		else return null ; 	
	}
	/**
	 * Sets the current player of the world
	 * We can control that one !
	 */
	public void setPlayer(Transform newPlayer) {
		Controls controlsScr = (Controls) this.GetComponent(typeof(Controls));
		controlsScr.setPlayer(newPlayer); 
		
		//Set the camera for this player. 	
		var cameraContainer = this.getCameraContainer(); 
		UniverseCamera cms = (UniverseCamera) cameraContainer.GetComponent(typeof(UniverseCamera));
		cms.setFollowObject(newPlayer); 
		
		//And set the GUI for this player too 
		var GUIContainer = this.getGUIContainer(); 
		MainGUI GUIScript = (MainGUI) GUIContainer.GetComponent(typeof(MainGUI));
		GUIScript.setGUITarget(newPlayer); 
		GUIScript.setActive(true); 
	}
	
	/**
	 * Clears all building preview's, 
	 * to use when switching to another control type than "building"
	 * */
	public static void clearAllBuildingPreview() {
		var allBuildingsPreview = GameObject.FindGameObjectsWithTag(Phobos.Vars.BUILDING_PREVIEW);
		foreach (GameObject build in allBuildingsPreview) {
			GameObject.Destroy(build); 	
		}
	}
	
	public static void createBuildingPreview(string buildingPrefab) {
		Vector3 pos = new Vector3(0, 0, 0);
		Quaternion rot = new Quaternion(0, 0, 0, 0); 
		GameObject buildPreview = (GameObject) Instantiate(Resources.Load ("Prefabs/Objects/BuildingPreview/" + buildingPrefab), pos, rot); 
		buildPreview.tag = Phobos.Vars.BUILDING_PREVIEW;
		buildPreview.name = buildingPrefab; 
	}	
	
	public static Controls getControls() {
		var Universe = GameObject.FindGameObjectWithTag(Phobos.Vars.UNIVERSE_TAG);
		return ((Controls) Universe.GetComponent(typeof(Controls)));
	}
	
	public static DataManager getDataManager() {
		var Universe = GameObject.FindGameObjectWithTag(Phobos.Vars.UNIVERSE_TAG);
		return ((DataManager) Universe.GetComponent(typeof(DataManager)));
	}
	
	public static GameObject getMainCamera() {
		var mainCamera = GameObject.FindGameObjectWithTag(Phobos.Vars.MAIN_CAMERA_TAG);
		return mainCamera;
	}
	
	public static GameObject getGUIContainer() {
		return  GameObject.FindGameObjectWithTag(Phobos.Vars.GUI_CONTAINER); 	
	}
	
	public static GameObject getCameraContainer() {
		return GameObject.FindGameObjectWithTag(Phobos.Vars.CAMERA_CONTAINER);
	}
	
	public static void switchCameraFollow(bool newFollow) {	
		GameObject mainCamera = GameController.getCameraContainer();
		if (mainCamera) {
			UniverseCamera univCam = (UniverseCamera) mainCamera.GetComponent(typeof(UniverseCamera));
			univCam.followingObject = newFollow ; 
		}
	}
	
	public static void switchSector(string newSector) {
		string newRoomName = "TestRoom" + newSector;
		PhotonNetwork.LeaveRoom(); 
		Application.LoadLevel(newSector); 
	}	
}
