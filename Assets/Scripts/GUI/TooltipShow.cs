using UnityEngine;
using System.Collections;

public class TooltipShow : MonoBehaviour {

	void OnTooltip (bool show)
	{
		Debug.Log ("coucou");	
	}
	
	void OnMouseOver() {
		Debug.Log ("on mouse over");	
	}
}
