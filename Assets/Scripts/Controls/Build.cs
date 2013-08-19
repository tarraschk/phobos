using UnityEngine;
using System.Collections;

public class Build : MonoBehaviour {
	
	RaycastHit hit;
	
	public bool isBuilding = true ; 
	public bool canBuildHere = true; 
	
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
	
	public void build() {
		if (this.checkCollisions()) {
			Debug.Log ("BUILDED");	
		}
	}
	
	private bool checkCollisions() {
		
		return this.canBuildHere; 
	}
	
	private bool isCollisionTerrain(Collider collision) {
		return (collision.name == Phobos.Vars.TERRAIN_NAME); 
	}
	
	void OnTriggerEnter(Collider collision) {
		i++; 
		Debug.Log (collision.gameObject.name);
		Debug.Log (gameObject.tag);
		if (collision.gameObject.tag != gameObject.tag && !this.isCollisionTerrain(collision)) {
			Debug.Log ("Enter" + i );
			this.canBuildHere = false; 
			
		}
	}
	
	void OnTriggerExit(Collider collision) {
		Debug.Log ("Exit");	
		if (collision.gameObject.tag != gameObject.tag) 	
			this.canBuildHere = true; 
	}
}
