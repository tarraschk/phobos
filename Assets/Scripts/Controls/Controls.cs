using UnityEngine;
using System.Collections;
/**
 * Manages the inputs from the mouse and the keyboard. 
 * Sends commands to the shipController of the current player. 
 * */
public class Controls : MonoBehaviour {
	
	public Transform player ; 
	RaycastHit hit;
	public enum controlTypes{moving, building};
	
	public controlTypes currentControlType = controlTypes.moving; 
	
	
	private float raycastLength = 5000; 
	
	void Update () {
		if (this.hasPlayer() ) {
			this.mousePoint(this.currentControlType); 
			this.keyboardInput();
		}
	}
	
	/**
	 * Manages the mouse controls, depending on the current mouse controltype	 
	**/
	public void mousePoint(controlTypes mouseControlType) {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
		if (Physics.Raycast(ray, out hit, raycastLength, Phobos.Vars.TERRAIN_LAYER)) {
			if (hit.collider.name == "TerrainMain")
			{
				switch (mouseControlType) {
					case controlTypes.moving: 
						this.moveMousePoint(hit); 
					break;	
					case controlTypes.building:
						this.moveBuildingPoing(hit); 
					
					break; 
				}
			}
		}
		Debug.DrawRay(ray.origin, ray.direction * raycastLength, Color.yellow);
	}
	
	/**
	 * Manages the keyboards input	 
	**/
	
	public void keyboardInput() {
		if (Input.GetKeyDown (KeyCode.B)) {
			this.switchControlType(controlTypes.moving, controlTypes.building); 
		}
	}
	
	/**
	 * Sets the current player controlled.
	 *
	 * */
	public void setPlayer(Transform newPlayer) {
		this.player = newPlayer ; 
		//Set the camera for this player. 	
		var cameraContainer = GameController.getCameraContainer(); 
		UniverseCamera cms = (UniverseCamera) cameraContainer.GetComponent(typeof(UniverseCamera));
		cms.setFollowObject(newPlayer); 
		
		//And set the GUI for this player too 
		var GUIContainer = GameController.getGUIContainer(); 
		MainGUI GUIScript = (MainGUI) GUIContainer.GetComponent(typeof(MainGUI));
		GUIScript.setGUITarget(newPlayer); 
		GUIScript.setActive(true); 
	}
	
	public bool hasPlayer() {
		return (this.player != null) ; 	
	}
	
	public void setControlType(controlTypes newControlType) {
		if (newControlType != controlTypes.building) {
			this.clearBuildingType(); 
		}
		if (newControlType != controlTypes.moving) {	
			this.clearMovingType();
		}
		if (newControlType == controlTypes.moving) {
			this.setMovingType(); 	
		}
		this.currentControlType = newControlType; 	
	}
	
	public void switchControlType(controlTypes type1, controlTypes type2) {
		if (this.currentControlType == type1) {
			this.setControlType(type2); 
		}
		else if (this.currentControlType == type2) {
			this.setControlType(type1); 
		}
	}
	
	private void clearBuildingType() {
		GameController.clearAllBuildingPreview(); 
	}
	
	private void clearMovingType() {
		GameController.switchCameraFollow(false); 
	}
	
	private void setMovingType() {
		GameController.switchCameraFollow(true); 
	}
	
	private void moveBuildingPoing(RaycastHit hit) {
		GameObject buildingPoint = GameObject.FindGameObjectWithTag(Phobos.Vars.BUILDING_PREVIEW) ; //TO INSTANTIATE BY PLAYER
		if (buildingPoint) { 
			buildingPoint.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
			if (Input.GetMouseButton(1) || Input.GetMouseButton(0)) 
			{	
				Build buildObj = (Build) buildingPoint.transform.GetComponent(typeof(Build)); 
				buildObj.build(buildingPoint.transform.position, buildingPoint.transform.rotation); 
			}
		}
	}
	
	private void moveMousePoint(RaycastHit hit) {
		if (Input.GetMouseButton(1) || Input.GetMouseButton(0)) 
		{
			Vector3 destination = hit.point; 
		    var target = this.player ; 
			if (target) {
				ShipController shipController = (ShipController) target.GetComponent(typeof(ShipController));
				shipController.moveTo(destination); 
			}
		}
	}
}
