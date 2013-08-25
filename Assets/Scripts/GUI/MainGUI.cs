using UnityEngine;
using System.Collections;

public class MainGUI : MonoBehaviour {
	
	public Transform GUITarget ; 
	public bool active = false ; 
	
	public Destructible TargetDestr ; 
	public Cargohold TargetCargo ; 
	
	public Texture inventorySlot ; 
	
	void Start() {
	}
	
	void OnGUI () {
		if (active) {
			GUI.backgroundColor = Color.red;
			this.inventory(); 
		}
	}
	
	public void setGUITarget(Transform newPlayer) {
		this.GUITarget = newPlayer;
		TargetDestr = (Destructible) this.GUITarget.GetComponent(typeof(Destructible));
		TargetCargo = (Cargohold) this.GUITarget.GetComponent(typeof(Cargohold));	 
	}
	
	public void setActive(bool newActive) {
		this.active = newActive ; 
	}
	
	private void inventory() {
		if (!this.inventorySlot) {
			Debug.LogError("Missing inventory texture.");
			return; 
		}
		string cargoNames = ""; 
		var cargoHold = TargetCargo.getCargoContent(); 
		foreach (Transform trans in cargoHold.transform) {
			Collectable collectItem = (Collectable) trans.GetComponent(typeof(Collectable));
			cargoNames = collectItem.quantity + " "; 
			GUI.Label (new Rect (Screen.width - 120,110,64,64), new GUIContent(cargoNames, collectItem.icon));
			
		}
		foreach (Transform j in cargoHold) {
			cargoNames += j.name + "\n"; 
		}
		GUI.Box (new Rect (10,Screen.height - 50,100,30), "Health : " + TargetDestr.energy);
		GUI.Box (new Rect (Screen.width - 150,75,100,30), "Cargo : " + TargetCargo.capacity + " / " + TargetCargo.capacityMax);
		//GUI.Button (new Rect (10,170,250,100), "Cargo : " + TargetCargo.capacity + " / " + TargetCargo.capacityMax +"\n" + cargoNames);
	}
}
