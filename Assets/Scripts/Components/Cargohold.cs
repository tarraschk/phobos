using UnityEngine;
using System.Collections;

public class Cargohold : MonoBehaviour {
	
	public ArrayList cargoContent = new ArrayList(); 
	public Hashtable cargoOrdered = new Hashtable(); 
	public int capacityMax = 500; 
	public int capacity = 0;
	
	public bool addObjectAtCargo(GameObject item, Transform cargoContainer) {
		Collectable collect = (Collectable) item.GetComponent(typeof(Collectable));
		
		if (this.capacity + collect.size <= this.capacityMax && !collect.inCargo) {
			this.capacity += collect.size;
			this.addItemToCargo(item, collect, cargoContainer);
			return true; 
		}
		else return false; 
	}
	
	/*public ArrayList getCargoContent() {
		return this.cargoContent; 	
	}*/
	
	public Transform getCargoContent() {
		var i = 0; 
		Transform cargoBay = transform.FindChild(Phobos.Vars.CARGOBAY); 
		return cargoBay; 
		/*Transform[] cargoResult= {} ; 
		foreach (Transform trans in cargoBay.transform) {
			cargoResult[i] = trans; 
			i++; 
		}
		return cargoResult; */	
	}
	
	private void addItemToCargo(GameObject item, Collectable itemCollectScript, Transform cargoContainer) {
		Collectable collectItem = (Collectable) item.GetComponent(typeof(Collectable));
		
		if (this.containerHasItem(cargoContainer, item)) {
			Transform itemSlot = containerFindItemSlot(cargoContainer, item) ; 
			Collectable itemSlotCollect = (Collectable) itemSlot.GetComponent(typeof(Collectable));
			itemSlotCollect.quantity += collectItem.quantity; 
			collectItem.isCollected(cargoContainer, true); 
			Debug.Log(itemSlotCollect.quantity); 
		}
		else {
			collectItem.isCollected(cargoContainer, false); 
			this.cargoContent.Add(item); 
		}
		
	}
	
	private Transform containerFindItemSlot(Transform cargoContainer, GameObject item) {
		return cargoContainer.FindChild(item.name); 
	}
		
	private bool containerHasItem(Transform cargoContainer, GameObject item) {
		var cargoItem = cargoContainer.FindChild(item.name); 
		if (cargoItem) {
			Collectable collect = (Collectable) cargoItem.GetComponent(typeof(Collectable));
			Collectable collectItem = (Collectable) item.GetComponent(typeof(Collectable));
			if (collect.prefab.GetType() == collectItem.prefab.GetType())
				return true; 
			else 
				return false; 
		}
		else return false; 
	}
}
