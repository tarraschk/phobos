using UnityEngine;
using System.Collections;

public class Cargohold : MonoBehaviour {
	
	public ArrayList cargoContent = new ArrayList(); 
	public int capacityMax = 500; 
	public int capacity = 0;
	
	public bool addObjectAtCargo(GameObject item) {
		Collectable collect = (Collectable) item.GetComponent(typeof(Collectable));
		if (this.capacity + collect.size <= this.capacityMax) {
			this.capacity += collect.size;
			this.cargoContent.Add(item); 	
			return true; 
		}
		else return false; 
	}
	
	public ArrayList getCargoContent() {
		return this.cargoContent; 	
	}
}
