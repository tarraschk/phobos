using UnityEngine;
using System.Collections;

public class MainGUI : MonoBehaviour {

	void OnGUI () {
		GUI.backgroundColor = Color.green;
		var pl = GameObject.Find("Player");
		Destructible destr = (Destructible) pl.GetComponent(typeof(Destructible));
		Cargohold cargo = (Cargohold) pl.GetComponent(typeof(Cargohold));
		if (GUI.Button (new Rect (10,10,150,100), "Health : " + destr.energy)) {
			print ("You clicked the button!");
		}
		string cargoNames = ""; 
		var cargoHold = cargo.getCargoContent(); 
		foreach (GameObject j in cargoHold) {
			cargoNames += j.name + "\n"; 
		}
		GUI.Button (new Rect (10,270,250,100), "Cargo : " + cargo.capacity + " / " + cargo.capacityMax +"\n" + cargoNames);
	}
}
