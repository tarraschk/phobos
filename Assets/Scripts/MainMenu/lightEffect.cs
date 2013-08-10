using UnityEngine;
using System.Collections;

public class lightEffect : MonoBehaviour {
	private int t = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var n = Random.value;
		Light l =(Light) this.GetComponent(typeof(Light));
		if(n > 0.5){
			
			l.enabled = false;	
		}
		else{
			l.enabled = true;
		}
	}
}
