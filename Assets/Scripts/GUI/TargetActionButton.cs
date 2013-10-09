using UnityEngine;
using System.Collections;

public class TargetActionButton : MonoBehaviour {
	
	public const string ATTACK = "attack";
	public const string COLLECT = "collect";
	public const string DOCK = "dock";
	
	public string actionOfButton; 
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnPress (bool isPressed)
	{
		//Debug.Log ("CLIIIICK");
	}
	
}
