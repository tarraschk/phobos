using UnityEngine;
using System.Collections;

public class Cargohold : MonoBehaviour {
	
	public ArrayList cargoContent = new ArrayList(); 
	public int capacityMax = 500; 
	public int capacity = 0;
	
	void Update () {
	
	}
	
	public void addObjectAtCargo(GameObject item) {
		this.cargoContent.Add(item); 	
		Debug.Log (this.cargoContent[0]);
	}
	
	public ArrayList getCargoContent() {
		return this.cargoContent; 	
	}
}
