using UnityEngine;
using System.Collections;

public class MainGUI : MonoBehaviour {
	
	public GameObject GUITarget ; 
	public Destructible TargetDestr ; 
	public Cargohold TargetCargo ; 
	
	void Start() {
		this.GUITarget = Universe.getPlayer (); 
		TargetDestr = (Destructible) this.GUITarget.GetComponent(typeof(Destructible));
		TargetCargo = (Cargohold) this.GUITarget.GetComponent(typeof(Cargohold));	
	}
	
	void OnGUI () {
		GUI.backgroundColor = Color.green;
		this.inventory(); 
		
	}
	
	private void inventory() {
		if (GUI.Button (new Rect (10,10,150,100), "Health : " + TargetDestr.energy)) {
			print ("You clicked the button!");
		}
		string cargoNames = ""; 
		var cargoHold = TargetCargo.getCargoContent(); 
		foreach (Transform trans in cargoHold.transform) {
			Collectable collectItem = (Collectable) trans.GetComponent(typeof(Collectable));
			cargoNames += collectItem.quantity + " x " + trans.name + "\n"; 
		}
		/*foreach (Transform j in cargoHold) {
			cargoNames += j.name + "\n"; 
		}*/
		GUI.Button (new Rect (10,270,250,100), "Cargo : " + TargetCargo.capacity + " / " + TargetCargo.capacityMax +"\n" + cargoNames);
	}
}
