using UnityEngine;
using System.Collections;

/**
 * Allows an object to carry items.
 * 
 *            WARNING 
 * Please note that the collected items are stocked in a 
 * child object of the current object, named "Cargo" (namespace Phobos.Vars.CARGO) 
 * The cargo hold IS this "Cargo" object, containing all items as children in it's hierarchy. 
 */
public class Cargohold : MonoBehaviour {
	
	//A representation of the cargo's content (may be not up to date)
	public ArrayList cargoContent = new ArrayList(); 
	
	//How much this cargo can hold
	public int capacityMax = 500; 
	
	//The current capacity cargo is holding
	public int capacity = 0;
	
	/**
	 * Adds an item to the cargo if we can carry this item. 
	 */
	public bool addObjectAtCargo(Transform item, Transform cargoContainer) {
		Collectable collect = (Collectable) item.GetComponent(typeof(Collectable));
		
		if (this.capacity + collect.size <= this.capacityMax && !collect.inCargo) {
			this.capacity += collect.size;
			this.addItemToCargo(item, collect, cargoContainer);
			return true; 
		}
		else return false; 
	}
	
	/**
	 * Returns the cargo bay content. 
	 * The Cargo bay is a child object of current object, 
	 * containing all the cargo's content in it's hierarchy. 
	 */
	public Transform getCargoContent() {
		var i = 0; 
		Transform cargoBay = transform.FindChild(Phobos.Vars.CARGOBAY); 
		return cargoBay; 
	}
	
	/**
	 * Moves the collected item to the cargo.
	 * If we already have such an Item in our cargo hold, we destroy the collected item
	 * and add it's quantity to the item we already have
	 * 
	 * If we don't have it, we just add it to the cargo hold. 
	 */
	private void addItemToCargo(Transform item, Collectable itemCollectScript, Transform cargoContainer) {
		Collectable collectItem = (Collectable) item.GetComponent(typeof(Collectable));
		int collectItemId = collectItem.id ; 
		if (this.containerHasItem(cargoContainer, item, collectItemId)) {
			Transform itemSlot = containerFindItemSlot(cargoContainer, collectItemId) ; 
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
	
	/**
	 * Finds an item in the cargo hold
	 */
	private Transform containerFindItemSlot(Transform cargoContainer, int itemId) {
		foreach (Transform child in cargoContainer.transform)
		{
			Collectable collect = (Collectable) child.GetComponent(typeof(Collectable));
			if (collect.id == itemId) 
				return child.transform ; 
		}
		return null ; 
	}
		
	/**
	 * Checks if we have an Item type in our cargo hold. 
	 * Uses the id of the collectable script
	 */
	private bool containerHasItem(Transform cargoContainer, Transform item, int itemId) {
		//var cargoItem = cargoContainer.FindChild(item.name); 
		
		foreach (Transform child in cargoContainer.transform)
		{
			Collectable collect = (Collectable) child.GetComponent(typeof(Collectable));
			if (collect.id == itemId) 
				return true ; 
		}
		return false ; 
		/*
		
		if (cargoItem) {
			Collectable collect = (Collectable) cargoItem.GetComponent(typeof(Collectable));
			Collectable collectItem = (Collectable) item.GetComponent(typeof(Collectable));
			if (collect.prefab.GetType() == collectItem.prefab.GetType())
				return true; 
			else 
				return false; 
		}
		else return false; */
	}
}
