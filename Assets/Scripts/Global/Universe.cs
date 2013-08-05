using UnityEngine;
using System.Collections;

public class Universe : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public GameObject[] getPlayers() {
		return (GameObject.FindGameObjectsWithTag("Player"));	
	}
}
