using UnityEngine;
using System.Collections;

public class MainGUI : MonoBehaviour {

	void OnGUI () {
		GUI.backgroundColor = Color.green;
		var pl = GameObject.Find("Player");
		Destructible destr = (Destructible) pl.GetComponent(typeof(Destructible));
		if (GUI.Button (new Rect (10,10,150,100), "Health : " + destr.energy)) {
			print ("You clicked the button!");
		}
	}
}
