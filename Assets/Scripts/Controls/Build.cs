using UnityEngine;
using System.Collections;

public class Build : MonoBehaviour {
	
	RaycastHit hit;
	
	public bool isBuilding = true ; 
	public bool canBuildHere = true; 
	
	public GameObject prefabBuild ; 
	
	private int i = 0; 
	
	void Update () {
		this.checkBuildingPosition(); 
	}
	
	public void checkBuildingPosition() {
		if (this.checkCollisions()) {
			this.renderer.material.color = Color.green;
		}
		else {
			this.renderer.material.color = Color.red;
		}	
	}
	
	public void build(Vector3 buildPosition, Quaternion buildRotation) {
		if (this.checkCollisions()) {
			Debug.Log ("BUILDED");	
			GameObject builded = (GameObject) Instantiate(this.prefabBuild, buildPosition, buildRotation) ; 
		}
	}
	
	private bool checkCollisions() {
		
		return this.canBuildHere; 
	}
	
	private bool isCollisionTerrain(Collider collision) {
		return (collision.name == Phobos.Vars.TERRAIN_NAME); 
	}
	
	void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.tag != gameObject.tag && !this.isCollisionTerrain(collision)) {
			this.canBuildHere = false; 
			
		}
	}
	
	void OnTriggerExit(Collider collision) {
		if (collision.gameObject.tag != gameObject.tag) 	
			this.canBuildHere = true; 
	}
}
