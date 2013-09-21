using UnityEngine;
using System.Collections;
/**
 * Manages the inputs from the mouse and the keyboard. 
 * Sends commands to the shipController of the current player. 
 * */
public class Controls : MonoBehaviour {
	
	public Transform player ; 
	RaycastHit hit;
	public enum controlTypes{moving, building, docked};
	
	public controlTypes currentControlType = controlTypes.moving; 
	
	private ShipController playerController ; 
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
	 * Commands the player to attack target.  
	**/
	public void attackTarget(Transform target) {
		if (this.currentControlType ==  controlTypes.moving)
			playerController.attackOwn(target.transform); 
	}
	
	/**
	 * Commands the player to collect target.  
	**/
	public void collectTarget(Transform target) {
		if (this.currentControlType ==  controlTypes.moving)
			playerController.collectOwn(target.transform); 
	}
	
	/**
	 * Commands the player to dock to target.  
	**/
	public void dockTo(Transform target) {
		if (this.currentControlType ==  controlTypes.moving)
			playerController.dockOwn(target.transform); 
	}
	
	/**
	 * Commands the player to undock.  
	**/
	public void undock() {
		playerController.undock(); 
	}
	
	
	/**
	 * Manages the keyboards input	 
	**/
	public void keyboardInput() {
		
		if (Input.GetKeyDown (KeyCode.B)) {
			this.switchBuildingControls(); 
		}
	}
	
	/**
	 * Sets the current player controlled.
	 *
	 * */
	public void setPlayer(Transform newPlayer) {
		this.player = newPlayer ; 
		playerController = (ShipController) newPlayer.GetComponent(typeof(ShipController));
	}
	
	public Transform getPlayer() {
		return this.player; 	
	}
	
	public bool hasPlayer() {
		return (this.player != null) ; 	
	}
	
	/**
	 * Sets the current control set
	 * to something different 
	 * */
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
	/**
	 * Switch the current control type from type 1 to type 2
	 * */
	public void switchControlType(controlTypes type1, controlTypes type2) {
		if (this.currentControlType == type1) {
			this.setControlType(type2); 
		}
		else if (this.currentControlType == type2) {
			this.setControlType(type1); 
		}
	}
	
	
	/**
	 * Switch the current controls
	 * to "docked" controls. 
	 * Pop the "docked" GUI. 
	 * Used when a ship is docked to a station. 
	 * */
	public void switchDockingControls() {
		var GUIContainer = GameController.getGUIContainer(); 
		MainGUI GUIScript = (MainGUI) GUIContainer.GetComponent(typeof(MainGUI));
		GUIScript.toggleShowDockMenu(); 
		this.switchControlType(controlTypes.moving, controlTypes.docked); 
	}
	
	/**
	 * Switch the current controls to "building" controls.  
	 * Pop the "building" GUI. 
	 * Used when a ship is building stuff. 
	 * */
	public void switchBuildingControls() {
		var GUIContainer = GameController.getGUIContainer(); 
		MainGUI GUIScript = (MainGUI) GUIContainer.GetComponent(typeof(MainGUI));
		GUIScript.toggleShowBuildMenu(); 
		this.switchControlType(controlTypes.moving, controlTypes.building); 
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
				
			    var target = this.player ; 
				if (target) {
					ShipController shipController = (ShipController) target.GetComponent(typeof(ShipController));
					shipController.tryBuild(buildingPoint.transform); 
				}
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
				shipController.moveToOwn(destination); 
			}
		}
	}
}
