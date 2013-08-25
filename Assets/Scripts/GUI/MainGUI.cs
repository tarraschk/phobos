using UnityEngine;
using System.Collections;

public class MainGUI : MonoBehaviour {
	
	public Transform GUITarget ; 
	public bool active = false ; 
	
	public Destructible TargetDestr ; 
	public Cargohold TargetCargo ; 
	
	void Start() {
	}
	
	void OnGUI () {
		if (active) {
			GUI.backgroundColor = Color.green;
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
		//if (GUI.Button (new Rect (10,10,150,100), "Health : " + TargetDestr.energy)) {
			//print ("You clicked the button!");
		//}
		string cargoNames = ""; 
		var cargoHold = TargetCargo.getCargoContent(); 
		foreach (Transform trans in cargoHold.transform) {
			Collectable collectItem = (Collectable) trans.GetComponent(typeof(Collectable));
			cargoNames += collectItem.quantity + " x " + trans.name + "\n"; 
		}
		foreach (Transform j in cargoHold) {
			cargoNames += j.name + "\n"; 
		}
		//GUI.Button (new Rect (10,170,250,100), "Cargo : " + TargetCargo.capacity + " / " + TargetCargo.capacityMax +"\n" + cargoNames);
	}
}
