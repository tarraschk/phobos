using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	public int size = 10; 
	public Texture icon ; 
	public string type; 
	public bool inCargo = false; 
	
	public void isCollected(Transform collector) {
		this.inCargo = true; 
		this.transform.parent = collector;
		Transform model = this.transform.FindChild(Phobos.Vars.MODEL); 
		model.renderer.enabled = false; 
		//Destroy (gameObject);
	}
	
}
