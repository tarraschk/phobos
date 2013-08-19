using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

	RaycastHit hit;
	public enum controlTypes{moving, building};
	private float raycastLength = 5000; 
	
	public int  damping = 6;
	public GameObject mouseTarget; 
	public controlTypes currentControlType = controlTypes.moving; 
	
	void Update () {
		/*switch (this.currentControlType) {
			case controlTypes.moving: 
				this.mousePoint(this.currentControlType); 
			break; 
			
			case controlTypes.building: 
				this.mousePoint(this.currentControlType); 
			break; 
		}*/
		this.mousePoint(this.currentControlType); 
		this.keyboardInput(); 
	}
	
	/**
	 * Manages the mouse controls, moving the Player	 
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
	
	public void keyboardInput() {
		if (Input.GetKeyDown (KeyCode.B)) {
			this.switchControlType(controlTypes.moving, controlTypes.building); 
		}
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
		Universe.clearAllBuildingPreview(); 
	}
	
	private void clearMovingType() {
		Universe.switchCameraFollow(false); 
	}
	
	private void setMovingType() {
		Universe.switchCameraFollow(true); 
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
			GameObject TargetObj = Instantiate(mouseTarget, hit.point, Quaternion.identity) as GameObject; 
			TargetObj.name = "targetInstanciated";
			TargetObj.transform.parent = GameObject.Find ("EmptyObjects").transform; 
		    var target = GameObject.FindGameObjectWithTag(Phobos.Vars.PLAYER_TAG);
			if (target) {
				ShipController shipController = (ShipController) target.GetComponent(typeof(ShipController));
				shipController.setBehavior(BehaviorTypes.moving); 
				shipController.unsetTarget(); 
				Propulsors prop = (Propulsors) target.GetComponent(typeof(Propulsors));
				prop.setTargetPos(TargetObj.transform);
			}
		}
	}
}
