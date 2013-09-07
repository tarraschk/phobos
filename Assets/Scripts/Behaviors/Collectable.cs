using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {
	
	public int id ; 
	public int size = 10; 
	public int quantity = 1; 
	public Texture icon ; 
	public string type; 
	public bool inCargo = false; 
	public GameObject prefab ; 
	
	public void isCollected(Transform collector, bool destroy) {
		this.inCargo = true; 
		this.transform.parent = collector;
		Transform model = this.transform.FindChild(Phobos.Vars.MODEL); 
		model.renderer.enabled = false; 
		if (destroy)
			Destroy (gameObject);
	}
	
}
