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
		switch (this.currentControlType) {
			case controlTypes.moving: 
				this.mousePoint(this.currentControlType); 
				this.keyboardInput(); 
			break; 
			
			case controlTypes.building: 
				this.mousePoint(this.currentControlType); 
			break; 
		}
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
			this.setControlType(controlTypes.building); 
		}
	}
	
	public void setControlType(controlTypes newControlType) {
		this.currentControlType = newControlType; 	
	}
	
	private void moveBuildingPoing(RaycastHit hit) {
		GameObject buildingPoint = GameObject.FindGameObjectWithTag(Phobos.Vars.BUILDING_PREVIEW) ; //TO INSTANTIATE BY PLAYER
		if (buildingPoint) { 
			if (Input.GetMouseButton(1) || Input.GetMouseButton(0)) 
			{	
				Build buildObj = (Build) buildingPoint.transform.GetComponent(typeof(Build)); 
				buildObj.build(); 
			}
			buildingPoint.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
		}
	}
	
	private void moveMousePoint(RaycastHit hit) {
		if (Input.GetMouseButton(1) || Input.GetMouseButton(0)) 
		{
			GameObject TargetObj = Instantiate(mouseTarget, hit.point, Quaternion.identity) as GameObject; 
			TargetObj.name = "targetInstanciated";
			TargetObj.transform.parent = GameObject.Find ("EmptyObjects").transform; 
		    var target = GameObject.Find("Player");
			if (target) {
				ShipController shipController = (ShipController) target.GetComponent(typeof(ShipController));
				shipController.setBehavior(BehaviorTypes.moving); 
				shipController.unsetTarget(); 
				Propulsors prop = (Propulsors) target.GetComponent(typeof(Propulsors));
				prop.setTargetPos(TargetObj.transform);
			}
			
		    /*target.transform.LookAt(TargetObj.transform);
			
			
			var rotation = Quaternion.LookRotation(TargetObj.transform.position - target.transform.position);
			target.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, Time.deltaTime * damping);*/
			
		}
	}
}
