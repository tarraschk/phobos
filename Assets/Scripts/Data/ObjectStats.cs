using UnityEngine;
using System.Collections;

public class ObjectStats : Photon.MonoBehaviour {
	
	public string name = ""; 
	public int id = 0; 
	public int level = 1; 
	public string descr = "Des méchants."; 
	
	void Awake() {
	
		this.level = 1 ; 
	}
}
