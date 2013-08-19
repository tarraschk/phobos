using UnityEngine;
using System.Collections;

public class Cargohold : MonoBehaviour {
	
	public ArrayList cargoContent = new ArrayList(); 
	public Hashtable cargoOrdered = new Hashtable(); 
	public int capacityMax = 500; 
	public int capacity = 0;
	
	public bool addObjectAtCargo(GameObject item) {
		Collectable collect = (Collectable) item.GetComponent(typeof(Collectable));
		
		if (this.capacity + collect.size <= this.capacityMax && !collect.inCargo) {
			this.capacity += collect.size;
			this.addItemToCargo(item, collect);
			return true; 
		}
		else return false; 
	}
	
	public ArrayList getCargoContent() {
		return this.cargoContent; 	
	}
	
	private void addItemToCargo(GameObject item, Collectable itemCollectScript) {
		this.cargoContent.Add(item); 
		if (this.cargoOrdered.ContainsKey(itemCollectScript.prefab)) {
			Debug.Log("Has it"); 
			//this.cargoOrdered[itemCollectScript.prefab] += itemCollectScript.quantity; 
		}
		else {
			Debug.Log("Add"); 
			this.cargoOrdered.Add(itemCollectScript.prefab, itemCollectScript.quantity); 
		}
		
		/*foreach (GameObject entry in this.cargoOrdered)
    		Debug.Log (entry.name);*/
	}
}
