using UnityEngine;
using System.Collections;

public class HoverScript : MonoBehaviour {

	
	void OnHover (bool isOver)
	{
		Controls GameControls = GameController.getControls(); 
		GameControls.inGameClickActive = !isOver ;
	}
	void OnMouseEnter() {
	}
}
